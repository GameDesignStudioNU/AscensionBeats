using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashGem : MonoBehaviour
{
    public float respawnTimeSet = 3.5f;
    public float respawnTime;
    
    void Update()
    {
        if (respawnTime > 0)
        {
            respawnTime -= Time.fixedDeltaTime;
        }
        else
        {
            gameObject.GetComponent<Renderer>().enabled = true;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Player>())
        {
            CharacterController2D _controller = collision.gameObject.GetComponentInParent<CharacterController2D>();
            if (!_controller.dashCharged)
            {
                _controller.dashCharged = true;
                gameObject.GetComponent<Renderer>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().isTrigger = false;

                respawnTime = respawnTimeSet;
            }

        }
    }


}
