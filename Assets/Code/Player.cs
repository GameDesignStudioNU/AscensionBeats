using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// Credit to 3DBuzz on YouTube for tutorial/inspiration to serve as basis for this code.

public class Player : MonoBehaviour
{
    // Fields and components
    private bool _isFacingRight;
    private CharacterController2D _controller;
    private float _normalizedHorizontalSpeed;

    public float MaxSpeed = 8f;
    public float SpeedAccelerationOnGround = 10f;
    public float SpeedAccelerationInAir = 5f;

    // Respawn point
    private Vector2 respawn_coords;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController2D>();
        _isFacingRight = transform.localScale.x > 0;
    }

    public void Update()
    {
        HandleInput();

        var movementFactor = _controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;
        _controller.SetForceHorizontal(Mathf.Lerp(_controller.Velocity.x, _normalizedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor));
    }

    private void HandleInput()
    {
        // Left and right movement
        if (Input.GetKey(KeyCode.D) && !(_controller.State.IsWallJumping && _controller.Velocity.x < 0))
        {
            _normalizedHorizontalSpeed = 1;
            if (!_isFacingRight)
                Flip();
        }
        else if (Input.GetKey(KeyCode.A) && !(_controller.State.IsWallJumping && _controller.Velocity.x > 0))
        {
            _normalizedHorizontalSpeed = -1;
            if (_isFacingRight)
                Flip();
        }
        else
        {
            _normalizedHorizontalSpeed = 0;
        }

        if (_controller.CanJump && Input.GetKeyDown(KeyCode.Space))
        {
            _controller.Jump();
        }

        if (_controller.CanWallJump && Input.GetKeyDown(KeyCode.Space))
        {
            _controller.WallJump();
        }

    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        _isFacingRight = transform.localScale.x > 0;
    }

//    private void groundcheck()
//    {
//        grounded = false;
//        collider2d collide = physics2d.overlapcircle(new vector2(transform.position.x - .1f, transform.position.y - .8f * player_height), .3f * player_width, _mask);
//        if (collide)
//        {
//            grounded = true;
//        }
//    }

//    private void wallcheck()
//    {
//        collider2d collide = physics2d.overlapbox(transform.position, new vector2(1.05f * player_width, .8f * player_height), 0f, _mask);
//        if (collide)
//        {
//            debug.log("this is working");
//            _rb.velocity = new vector2(0, _rb.velocity.y);
//        }
//    }

//    public void setrespawn(vector2 new_respawn_coords)
//    {
//        respawn_coords = new_respawn_coords;
//    }

//    public void killed()
//    {
//        transform.position = respawn_coords;
//    }

}
