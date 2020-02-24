using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit to 3DBuzz on YouTube for tutorial/inspiration to serve as basis for this code.

public class ControllerState2D
{
    public float ButtonPressLiveTime = .1f;
    private float AxisOverlap = 2f;

    // Physical controller bindings
    public bool RightButtonPress { get
        {
            return Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow); 
        } }
    public bool RightButtonHold { get
        {
            return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || (Input.GetAxis("Horizontal") > .1f) && Mathf.Abs(Input.GetAxis("Vertical")) < AxisOverlap * Mathf.Abs(Input.GetAxis("Horizontal"));
        } }
    public bool LeftButtonPress { get
        {
            return Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        } }
    public bool LeftButtonHold { get
        {
            return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < -.1f && Mathf.Abs(Input.GetAxis("Vertical")) < AxisOverlap * Mathf.Abs(Input.GetAxis("Horizontal"));
        } }
    public bool UpButtonPress { get
        {
            return Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        } }
    public bool UpButtonHold { get
        {
            return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("Vertical") < -.1f && Mathf.Abs(Input.GetAxis("Horizontal")) < AxisOverlap * Mathf.Abs(Input.GetAxis("Vertical"));
        } }
    public bool DownButtonPress { get
        {
            return Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
        } }
    public bool DownButtonHold
    { get
        {
            return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("Vertical") > .1f && Mathf.Abs(Input.GetAxis("Horizontal")) < AxisOverlap * Mathf.Abs(Input.GetAxis("Vertical"));
        } }
    public float JumpButtonTimer { get; set; }
    public bool JumpButtonAction { get
        {
            return JumpButtonPress || JumpButtonTimer > 0;
        } }
    public bool JumpButtonPress { get
        {
           return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("Jump");
        }
    }
    public bool JumpButtonHold
    {
        get
        {
            return Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.C) || Input.GetButton("Jump");
        }
    }
    public bool ClingButtonPress { get
        {
            return Input.GetKeyDown(KeyCode.LeftShift);
        } }
    public bool ClingButtonHold { get
        {
            return Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("Cling") > 0;
        } }
    public float DashButtonCooldown { get; set; }
    public bool DashButtonPress { get
        {
            return Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Dash");
        } }
    public bool DashButtonHold { get
        {
            return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.X) || Input.GetButton("Dash");
        } }

    public bool IsCollidingRight { get; set; }
    public bool IsCollidingLeft { get; set; }
    public bool IsCollidingAbove { get; set; }
    public bool IsCollidingBelow { get; set; }
    public bool IsGrounded { get { return IsCollidingBelow; } }
    public bool IsJumping { get; set; }
    public bool IsWallSliding { get; set; } 
    public bool IsWallJumping { get; set; }
    public bool IsClinging { get; set; }
    public bool IsRunning { get { return IsGrounded && (LeftButtonHold || RightButtonHold); } }
    public bool IsDashing { get; set; }


    public bool HasCollisions { get { return IsCollidingRight || IsCollidingLeft || IsCollidingAbove || IsCollidingBelow; } }

    public void Reset()
    {
        IsCollidingRight =
            IsCollidingLeft =
            IsCollidingAbove =
            IsCollidingBelow = false;
    }

    public override string ToString()
    {
        return string.Format(
            "(Controller: r:{0} l:{1} a:{2} b:{3} ws:{4})",
            IsCollidingRight,
            IsCollidingLeft,
            IsCollidingAbove,
            IsCollidingBelow,
            IsWallSliding);
    }

}
