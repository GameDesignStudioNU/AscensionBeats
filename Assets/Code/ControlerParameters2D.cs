using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// Credit to 3DBuzz on YouTube for tutorial/inspiration to serve as basis for this code.

[Serializable]
public class ControllerParameters2D
{
    public Vector2 MaxVelocity = new Vector2(float.MaxValue, float.MaxValue);

    public float Gravity = -30f;

    public float JumpFrequency = .25f;

    public float JumpMagnitude = 15f;

    public float WallJumpMagnitudeHor = 15f;

    public float WallJumpMagnitudeVert = 15f;

    public float WallSlideSpeed = -4f;

    public float WallSlideAcceleration = 10f;



}
