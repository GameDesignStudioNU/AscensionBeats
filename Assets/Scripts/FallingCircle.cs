using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCircle : HostileEntity
{
    private Rigidbody2D rb;
    private int direction;
    private float yVel;
    // Start is called before the first frame update
    void Start()
    {
        direction = Random.Range(0,2);
        if(direction >= 1) direction *= -1;
        
        yVel = -1 * Random.Range(1f, 10f);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float xVel = Mathf.PingPong(Time.time, 4) - 2;
        xVel = (direction == 0) ? xVel : -1 * xVel;
        rb.velocity = new Vector2(xVel, yVel);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Boundary") {
            Deactivate();
        }
    }

    public override Vector2 GetStartPosition() {
        float spawnX = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
        float spawnY = (Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y) - 1;
        return new Vector2(spawnX, spawnY);
    }
}
