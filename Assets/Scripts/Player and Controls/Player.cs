using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Credit to 3DBuzz on YouTube for tutorial/inspiration to serve as basis for this code.

public class Player : MonoBehaviour
{
    // Fields and components
    private bool _isFacingRight;
    private CharacterController2D _controller;
    private float _normalizedHorizontalSpeed;
    private Animator _animator;

    public float MaxSpeed = 8f;
    public float SpeedAccelerationOnGround = 10f;
    public float SpeedAccelerationInAir = 5f;

    public bool Invincible = false;

    // Respawn point
    private Vector2 respawn_coords;
    public bool reloading;

    void Start()
    {
        _controller = GetComponent<CharacterController2D>();
        _isFacingRight = transform.localScale.x > 0;
        _animator = GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        if (FindObjectOfType<GameManager>().NOT_PAUSED)
        {
            HandleInput();
            Animator();
            var movementFactor = _controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;
            if (!_controller.State.IsDashing)
                _controller.SetForceHorizontal(Mathf.Lerp(_controller.Velocity.x, _normalizedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor));

            if ((_controller.CanDash || _controller.State.IsDashing) && GetComponentInChildren<ParticleSystem>().isStopped)
            {
                GetComponentInChildren<ParticleSystem>().Play();
            }
            else if (!_controller.CanDash && !_controller.State.IsDashing)
            {
                GetComponentInChildren<ParticleSystem>().Stop();
            }
        } 
    }

    private void HandleInput()
    {
        // Left and right movement
        if (_controller.State.RightButtonHold && !_controller.State.IsWallJumping && !_controller.State.IsClinging && !_controller.State.IsDashing)
        {
            _normalizedHorizontalSpeed = 1;
            if (!_isFacingRight)
                Flip();
        }
        else if (_controller.State.LeftButtonHold && !_controller.State.IsWallJumping && !_controller.State.IsClinging && !_controller.State.IsDashing)
        {
            _normalizedHorizontalSpeed = -1;
            if (_isFacingRight)
                Flip();
        }
        // Wall jumping horizontal movement
        else if (_controller.State.IsWallJumping)
        {
            _normalizedHorizontalSpeed = _controller.lastWallJumpRight ? -1 : 1;
        }
        else
        {
            _normalizedHorizontalSpeed = 0;
        }

        // Jumping and Wall jumping
        if (_controller.State.JumpButtonPress && !_controller.CanJump)
        {
            _controller.State.JumpButtonTimer = _controller.State.ButtonPressLiveTime;
        }

        if (_controller.CanJump && _controller.State.JumpButtonAction)
        {
            _controller.Jump();
            _controller.State.JumpButtonTimer = 0;
        }
        else if (_controller.CanWallJump && _controller.State.JumpButtonAction && !_controller.State.DashButtonHold)
        {
            _controller.WallJump();
            _controller.State.JumpButtonTimer = 0;
        }

        // Clinging
        _controller.State.IsClinging = _controller.CanCling && _controller.State.ClingButtonHold && !_controller.State.IsWallJumping && !_controller.State.IsDashing;

        if (_controller.State.IsClinging)
        {
            if (_controller.lastWallTouchRight != _isFacingRight)
            {
                Flip();
            }
        }

        if (_controller.Velocity.x > .01f && !_isFacingRight)
            Flip();
        if (_controller.Velocity.x < -.01f && _isFacingRight)
            Flip();

        // Dashing        
        if (_controller.State.DashButtonPress && _controller.CanDash)
        {
            var _dashFaceRight = _controller.State.IsClinging ? !_isFacingRight : _isFacingRight;
            _controller.Dash(_dashFaceRight);
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        _isFacingRight = transform.localScale.x > 0;
    }

    private void Animator()
    {
        _animator.SetBool("IsRunning", _controller.State.IsRunning && !_controller.State.IsDashing);
        _animator.SetBool("IsJumping", (_controller.State.IsJumping || _controller.State.IsWallJumping) && !_controller.State.IsWallSliding && _controller.Velocity.y > 0);
        _animator.SetBool("IsFalling", !_controller.State.IsClinging && !_controller.State.IsGrounded && !_controller.State.IsWallSliding && !_controller.State.IsDashing && _controller.Velocity.y < 0);
        _animator.SetBool("IsClinging", _controller.State.IsClinging && !_controller.State.IsClimbing);
        _animator.SetBool("IsWallSliding", _controller.State.IsWallSliding);
        _animator.SetBool("IsDashing", _controller.State.IsDashing);
        _animator.SetBool("IsClimbing", _controller.State.IsClimbing);
    }


    void OnTriggerStay2D(Collider2D col) 
    {
        if (col.gameObject.tag == "Obstacle" && !_controller.State.IsDashing && !Invincible && !reloading)
        { 
            if (GetComponent<PlayerHealth>().enabled)
            {
                GetComponent<PlayerHealth>().ChangeHealth(-20);
            }
            else
            {
                reloading = true;
                FindObjectOfType<GameManager>().deathCount++;
                FindObjectOfType<GameManager>().Reload();
                //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            }

        }
        if (col.gameObject.tag == "Void" && !Invincible && !reloading)
        {
            FindObjectOfType<GameManager>().deathCount++;
            FindObjectOfType<GameManager>().Reload();
            //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

            reloading = true;
        }
    }

}
