using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatDetection : MonoBehaviour
{
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Awake()
    {
        audioManager = AudioManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
