﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCircle : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(Mathf.PingPong(Time.time, 4) - 2, -1);
    }
}