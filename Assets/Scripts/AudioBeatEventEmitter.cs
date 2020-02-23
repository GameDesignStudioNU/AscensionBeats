using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBeatEventEmitter : MonoBehaviour
{
    public string songData;
    public GameObject fallingCircle;
    public string[] stringTimeStamps;
    public int[] timeStamps;
    private int index = 0;
    private float startTime;
    private int numTimeStamps;
    private ObjectPool fallingCirclePool;

    void Awake()
    {
        //format timestamps
        stringTimeStamps = songData.Split(',');
        timeStamps = new int[stringTimeStamps.Length];
        for(int j = 0; j < stringTimeStamps.Length; j++) {
            timeStamps[j] = int.Parse(stringTimeStamps[j]);
        }

        //create falling circles pool
        fallingCirclePool = new ObjectPool(40, fallingCircle);
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
            SpawnFallingCircle();
            index++;
        }
    }

    void Play() {
        GetComponent<AudioSource>().Play();
    }

    void SpawnFallingCircle() {
        float spawnX = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
        float spawnY = (Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y) - 1;
        GameObject circle = fallingCirclePool.GetObject();
        if(circle != null) {
            circle.transform.position = new Vector3(spawnX, spawnY, 10);
            circle.SetActive(true);
        }
        // Instantiate(projectilePrefab, new Vector3(spawnX, spawnY, 10), Quaternion.identity);
    }

    public void RemoveFallingCircle(GameObject obj) {
        fallingCirclePool.RemoveObject(obj);
    }
}
