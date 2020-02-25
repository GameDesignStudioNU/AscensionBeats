using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : HostileEntity
{
    private float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        lifeTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime;
        if(lifeTime >= 3) {
            lifeTime = 0;
            Deactivate();
        }
    }

    public override Vector2 GetStartPosition() {
        float spawnX = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
        float spawnY = (Camera.main.ScreenToWorldPoint(new Vector2(0,0)).y) + 1f;
        return new Vector2(spawnX, spawnY);
    }
}
