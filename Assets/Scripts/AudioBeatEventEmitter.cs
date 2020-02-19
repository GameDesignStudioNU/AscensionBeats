using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBeatEventEmitter : MonoBehaviour
{
    public string songData;
    public GameObject projectilePrefab;
    public string[] stringTimeStamps;
    public int[] timeStamps;
    private int index = 0;
    private float startTime;
    private int numTimeStamps;

    void Awake()
    {
        stringTimeStamps = songData.Split(',');
        timeStamps = new int[stringTimeStamps.Length];
        for(int j = 0; j < stringTimeStamps.Length; j++) {
            timeStamps[j] = int.Parse(stringTimeStamps[j]);
        }
    }

    void Start()
    {
        startTime = Time.time;
        numTimeStamps = timeStamps.Length;
        Play();
    }

    // Update is called once per frame
    void Update()
    {
        startTime += 1000*Time.deltaTime;
        if(index < numTimeStamps && startTime >= timeStamps[index]) {
            SpawnSphere();
            index++;
        }
    }

    void Play() {
        GetComponent<AudioSource>().Play();
    }

    void SpawnSphere() {
        float spawnX = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
        float spawnY = (Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y) - 1;
        Instantiate(projectilePrefab, new Vector3(spawnX, spawnY, 10), Quaternion.identity);
    }

}
