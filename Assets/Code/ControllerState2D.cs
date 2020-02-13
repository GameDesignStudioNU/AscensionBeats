using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit to 3DBuzz on YouTube for tutorial/inspiration to serve as basis for this code.

public class ControllerState2D
{
    public bool IsCollidingRight { get; set; }
    public bool IsCollidingLeft { get; set; }
    public bool IsCollidingAbove { get; set; }
    public bool IsCollidingBelow { get; set; }
    public bool IsGrounded { get { return IsCollidingBelow; } }
    public bool IsWallSliding { get; set; } //{ return (IsCollidingRight || IsCollidingLeft) && !IsCollidingBelow; } }


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
