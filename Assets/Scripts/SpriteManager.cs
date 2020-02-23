using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Rigidbody2D _rb;
    private float epsilon = .05f;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if (_rb.velocity.x < -epsilon)
        {
            sprite.flipX = true;
        }

        if (_rb.velocity.x > epsilon)
        {
            sprite.flipX = false;
        }
    }
}