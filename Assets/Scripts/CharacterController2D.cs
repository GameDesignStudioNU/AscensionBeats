using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit to 3DBuzz on YouTube for tutorial/inspiration to serve as basis for this code.

public class CharacterController2D : MonoBehaviour
{
    // Constants
    private const float SkinWidth = .02f;
    private const int TotalHorizontalRays = 8;
    private const int TotalVerticalRays = 4;
    private const float WallInteractWidth = .5f;

    // Public parameters
    public LayerMask PlatformMask;
    public ControllerParameters2D DefaultParameters;

    public ControllerState2D State { get; private set; }
    public Vector2 Velocity { get { return _velocity; } }

    public bool CanJump { get { return State.IsGrounded; } }
    public bool CanWallJump { get
        {
            var boxCastHit = Physics2D.OverlapBox(_transform.position, new Vector2(2f, 1f), 0f, PlatformMask);
            return (Mathf.Abs(wallTouchCoord - _transform.position.x) < WallInteractWidth && !State.IsGrounded) && boxCastHit;
        } }
    public bool CanDash { get
        {
            return State.DashButtonCooldown <= 0 && dashCharged;
        } }
    public bool HandleCollisions { get; set; }
    public ControllerParameters2D Parameters { get { return _overrideParameters ?? DefaultParameters; } }
    public GameObject PlatformStandingOn { get; private set; }

    // Aliasing
    private Vector2 _velocity;
    private Transform _transform;
    private Vector3 _localScale;
    private BoxCollider2D _boxCollider;
    private ControllerParameters2D _overrideParameters;
    private float _jumpIn;
    private Vector3
        _raycastTopLeft,
        _raycastBottomLeft,
        _raycastBottomRight;

    private float
        _verticalDistanceBetweenRays,
        _HorizontalDistanceBetweenRays;

    private float wallTouchCoord;
    private bool lastWallTouchRight;
    public bool lastWallJumpRight;
    public bool dashCharged = true;



    // Methods
    public void Awake()
    {
        HandleCollisions = true;
        State = new ControllerState2D();
        _transform = transform;
        _localScale = transform.localScale;
        _boxCollider = GetComponent<BoxCollider2D>();

        var colliderWidth = _boxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2 * SkinWidth);
        _HorizontalDistanceBetweenRays = colliderWidth / (TotalVerticalRays - 1);

        var colliderHeight = _boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2 * SkinWidth);
        _verticalDistanceBetweenRays = colliderHeight / (TotalHorizontalRays - 1);

    }

    public void FixedUpdate()
    {
        // Jumping and WallJumping stuff
        _jumpIn -= Time.deltaTime;
        
        if (State.JumpButtonTimer > 0)
            State.JumpButtonTimer -= Time.fixedDeltaTime;
        if (State.DashButtonCooldown > 0)
            State.DashButtonCooldown -= Time.fixedDeltaTime;
        else if ((State.DashButtonCooldown  <= 0) && !State.IsJumping && !State.IsWallJumping)
        {
            State.IsDashing = false;
            if (_velocity.y > .05f)
            {
                SetForceVertical(Mathf.Lerp(_velocity.y, 0, Time.deltaTime * 10f));
            }
        }

        if (State.IsJumping && !State.JumpButtonHold && _velocity.y > 0)
        {
            //SetForceVertical(Mathf.Lerp(_velocity.y, 0, Time.deltaTime * 15f));
        }
        if (State.IsWallJumping)
        {
            if (_velocity.y > 0)
                SetForceVertical(Mathf.Lerp(_velocity.y, 0, Time.deltaTime * 4f));
            else
                State.IsWallJumping = false;
        }

        // State checks
        WallSlideCheck();
        ClingCheck();
        if (!State.IsWallSliding && !State.IsClinging && !State.IsDashing)
        {
            _velocity.y += Parameters.Gravity * Time.deltaTime;
        }

        // Terminal Velocity
        if (_velocity.y < -30f)
        {
            SetForceVertical(Mathf.Lerp(_velocity.y, -30f, Time.deltaTime * 10f));
        }
        // Move!
        Move(Velocity * Time.deltaTime);

    }

    public void AddForce(Vector2 force)
    {
        _velocity += force;
    }

    public void SetForce(Vector2 force)
    {
        _velocity = force;
    }

    public void SetForceHorizontal(float x)
    {
        _velocity = new Vector2(x, _velocity.y);
    }

    public void SetForceVertical(float y)
    {
        _velocity = new Vector2(_velocity.x, y);
    }

    // Movement
    public void Jump()
    {
        // TODO: Noving platforms
        if (_jumpIn < 0)
        {
            AddForce(new Vector2(0, Parameters.JumpMagnitude));            
            _jumpIn = Parameters.JumpFrequency;
            State.IsJumping = true;
            State.IsCollidingBelow = false;
        }
        
    }

    public void WallJump()
    {
        var jumpDirection = lastWallTouchRight ? -1f : 1f;
        lastWallJumpRight = lastWallTouchRight;
        //AddForce(new Vector2(Parameters.WallJumpMagnitudeHor * jumpDirection, Parameters.WallJumpMagnitudeVert));
        _velocity = new Vector2(0f, 0f);
        SetForce(new Vector2(0, Parameters.WallJumpMagnitudeVert));
        State.IsWallJumping = true;
    }
    
    public void WallSlideCheck()
    {
        if (!State.IsGrounded &&
            (Mathf.Abs(_transform.position.x - wallTouchCoord) < .01f) &&
            ((lastWallTouchRight && State.RightButtonHold) || (!lastWallTouchRight && State.LeftButtonHold)) &&
            !State.IsClinging)
        {
            State.IsWallSliding = true;
            if (_velocity.y < 0)
                SetForceVertical(Mathf.Lerp(_velocity.y, Parameters.WallSlideSpeed, Time.deltaTime * Parameters.WallSlideAcceleration));
            else
                _velocity.y += Parameters.Gravity * Time.deltaTime;
            //var stickVel = State.IsCollidingRight ? .5f : -.5f;
            //AddForce(new Vector2(stickVel, 0));
        }
        else
        {
            State.IsWallSliding = false;
        }
    }

    public void ClingCheck()
    {
        if (State.IsClinging == true)
        {
            SetForceVertical(Mathf.Lerp(_velocity.y, 0, Time.deltaTime * 20f));
            State.IsJumping = false;
            State.IsWallJumping = false;
        } 
    }

    public void Dash(bool _isFacingRight)
    {
        float verticalDash = 0f;
        float horizontalDash = 0f;

        if (!(State.UpButtonHold || State.DownButtonHold || State.RightButtonHold || State.LeftButtonHold))
        {
            horizontalDash = _isFacingRight ? 1f : -1f;
        }
        else
        {
            if (State.UpButtonHold)
            {
                verticalDash = 1f;
            }
            else if (State.DownButtonHold)
            {
                verticalDash = -1f;
            }

            if (State.RightButtonHold)
            {
                horizontalDash = 1f;
            }
            else if (State.LeftButtonHold)
            {
                horizontalDash = -1f;
            }
        }
        
        State.IsDashing = true;
        State.IsJumping = false;
        State.IsWallJumping = false;
        State.DashButtonCooldown = Parameters.DashCooldown;
        dashCharged = false;

        if (horizontalDash != 0 && verticalDash != 0)
        {
            horizontalDash /= Mathf.Sqrt(2);
            verticalDash /= Mathf.Sqrt(2);
        }
        SetForce(new Vector2(horizontalDash * Parameters.DashForce, verticalDash * Parameters.DashForce));
        

    }

    public void Move(Vector2 deltaMovement)
    {
        var wasGrounded = State.IsCollidingBelow;
        State.Reset();

        if (HandleCollisions)
        {
            HandlePlatforms();
            CalculateRayOrigins();

            if (Mathf.Abs(deltaMovement.x) > .001f)
            {
                MoveHorizontally(ref deltaMovement);
            }

            MoveVertically(ref deltaMovement);
        }

        _transform.Translate(deltaMovement, Space.World);

        // TODO: Moving platform code

        if (Time.deltaTime > 0)
        {
            _velocity = deltaMovement / Time.deltaTime;
        }

        // Clamp velocities to max velocity!
        _velocity.x = Mathf.Min(_velocity.x, Parameters.MaxVelocity.x);
        _velocity.y = Mathf.Min(_velocity.y, Parameters.MaxVelocity.y);
    }

    private void MoveHorizontally(ref Vector2 deltaMovement)
    {
        var isGoingRight = deltaMovement.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;
        var rayDirection = isGoingRight ? Vector2.right : Vector2.left;
        var rayOrigin = isGoingRight ? _raycastBottomRight : _raycastBottomLeft;

        for (var i = 0; i < TotalHorizontalRays; i++)
        {
            var rayVectorPos = new Vector2(rayOrigin.x, rayOrigin.y + (i * _verticalDistanceBetweenRays));
            Debug.DrawRay(rayVectorPos, rayDirection * rayDistance, Color.red);

            var rayCastHit = Physics2D.Raycast(rayVectorPos, rayDirection, rayDistance, PlatformMask);

            if (!rayCastHit)
                continue;


            deltaMovement.x = rayCastHit.point.x - rayVectorPos.x;
            rayDistance = Mathf.Abs(deltaMovement.x);

            if (isGoingRight)
            {
                deltaMovement.x -= SkinWidth;
                State.IsCollidingRight = true;
                lastWallTouchRight = true;
                wallTouchCoord = _transform.position.x;
            }
            else
            {
                deltaMovement.x += SkinWidth;
                State.IsCollidingLeft = true;
                lastWallTouchRight = false;
                wallTouchCoord = _transform.position.x;
            }

            if (rayDistance < SkinWidth + .0001f)
            {
                break;
            }
        }
    }

    private void MoveVertically(ref Vector2 deltaMovement)
    {
        var isGoingUp = deltaMovement.y > 0;
        var rayDistance = Mathf.Abs(deltaMovement.y) + SkinWidth;
        var rayDirection = isGoingUp ? Vector2.up : Vector2.down;
        var rayOrigin = isGoingUp ? _raycastTopLeft : _raycastBottomLeft;

        rayOrigin.x += deltaMovement.x;

        var standingOnDistance = float.MaxValue;

        for (var i = 0; i < TotalVerticalRays; i++)
        {
            var rayVectorPos = new Vector2(rayOrigin.x + (i * _HorizontalDistanceBetweenRays), rayOrigin.y);
            Debug.DrawRay(rayVectorPos, rayDirection * rayDistance, Color.red);

            var rayCastHit = Physics2D.Raycast(rayVectorPos, rayDirection, rayDistance, PlatformMask);

            if (!rayCastHit)
                continue;

            if (!isGoingUp)
            {
                var verticalDistanceToHit = _transform.position.y - rayCastHit.point.y;
                if (verticalDistanceToHit < standingOnDistance)
                {
                    standingOnDistance = verticalDistanceToHit;
                    PlatformStandingOn = rayCastHit.collider.gameObject;
                }
            }

            deltaMovement.y = rayCastHit.point.y - rayVectorPos.y;
            rayDistance = Mathf.Abs(deltaMovement.y);

            if (isGoingUp)
            {
                deltaMovement.y -= SkinWidth;
                State.IsCollidingAbove = true;
            }
            else
            {
                deltaMovement.y += SkinWidth;
                State.IsCollidingBelow = true;
                State.IsJumping = false;
                dashCharged = true;
            }

            if (rayDistance < SkinWidth + .0001f)
            {
                break;
            }
        }
    }

    private void HandlePlatforms()
    {

    }

    private void CalculateRayOrigins()
    {
        var halfSize = new Vector2(_boxCollider.size.x * Mathf.Abs(_localScale.x), _boxCollider.size.y * Mathf.Abs(_localScale.y)) / 2;
        var center = new Vector2(_boxCollider.offset.x * _localScale.x, _boxCollider.offset.y * _localScale.y);

        _raycastTopLeft =  (Vector2) _transform.position + new Vector2(center.x - halfSize.x + SkinWidth, center.y + halfSize.y - SkinWidth);
        _raycastBottomLeft = (Vector2)_transform.position + new Vector2(center.x - halfSize.x + SkinWidth, center.y - halfSize.y + SkinWidth);
        _raycastBottomRight = (Vector2)_transform.position + new Vector2(center.x + halfSize.x - SkinWidth, center.y - halfSize.y + SkinWidth);
    }

    // Collider stuff
    public void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        
    }


}
