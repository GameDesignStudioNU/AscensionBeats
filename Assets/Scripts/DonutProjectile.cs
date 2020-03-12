using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutProjectile : HostileEntity
{
    private Vector2 moveDir;
    private float speed;
    private float timer;

    void Start()
    {
        speed = 4f;
        timer = 0f;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Boundary") {
            Deactivate();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 3f) {
            timer = 0f;
            Deactivate();
        }
        transform.Translate(moveDir * speed * Time.deltaTime);
    }

    public void SetMoveDir(Vector2 dir) {
        moveDir = dir;
    }

    public override void SetStartPosition() {
        return;
    }
}
