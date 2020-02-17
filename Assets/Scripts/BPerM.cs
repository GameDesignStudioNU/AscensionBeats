using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPerM : MonoBehaviour
{
    private static BPerM instance;
    public float bpm;
    private float beatInterval, beatTimer, beatIntervalD8, beatTimerD8;
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

        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         BeatDetection();
    }

    void BeatDetection()
    {
        onFullBeat = false;
        beatInterval = 60/bpm;
        beatTimer += Time.deltaTime;

        if(beatTimer >= beatInterval) {
            beatTimer -= beatInterval;
            onFullBeat = true;
            fullBeatCount++;
            // Debug.Log("Full");
        }

        onEigthBeat = false;
        beatIntervalD8 = beatInterval/8;
        beatTimerD8 += Time.deltaTime;
        if(beatTimerD8 >= beatIntervalD8) {
            beatTimerD8 -= beatIntervalD8;
            onEigthBeat = true;
            EigthBeatCount++;
            // Debug.Log("8th");
        }
    }
}
