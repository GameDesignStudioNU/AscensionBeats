using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;


public class CheckPoint : MonoBehaviour
{
    public bool activated;

    private void Awake()
    {
        activated = false;
        if (GetComponentInParent<TextMeshPro>())
        {
            GetComponentInParent<TextMeshPro>().enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponentInParent<Player>() && !activated)
        {
            FindObjectOfType<GameManager>().Save();

            GetComponent<SpriteRenderer>().color = Color.green;
            GetComponentInChildren<Light2D>().color = Color.green;
            activated = true;
        }

        if (GetComponentInParent<TextMeshPro>())
        {
            GetComponentInParent<TextMeshPro>().enabled = true;
        }
    }
}
