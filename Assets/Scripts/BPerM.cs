using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPerM : MonoBehaviour
{
    private static BPerM instance;
    public float bpm, beatTimer;
    public float beatInterval, beatIntervalD8, beatTimerD8;
    public static bool onFullBeat, onEigthBeat;
    public static int fullBeatCount, EigthBeatCount;

    void Awake()
    {
        if(instance == null){
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<GameManager>().NOT_PAUSED)
            BeatDetection();
    }

    void BeatDetection()
    {
        onFullBeat = false;
        beatInterval = 60/bpm;
        beatTimer += Time.deltaTime;

        if(beatTimer >= beatInterval)
        {
            beatTimer -= beatInterval;
            onFullBeat = true;
            fullBeatCount++;
            // Debug.Log("Full");
        }

        onEigthBeat = false;
        beatIntervalD8 = beatInterval/8;
        beatTimerD8 += Time.deltaTime;
        if(beatTimerD8 >= beatIntervalD8)
        {
            beatTimerD8 -= beatIntervalD8;
            onEigthBeat = true;
            EigthBeatCount++;
            // Debug.Log("8th");
        }
    }
}
