using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnBoundary : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Obstacle") {
            FindObjectOfType<AudioManager>().GetComponent<AudioBeatEventEmitter>().RemoveFallingCircle(col.gameObject);
        }
    }
}
