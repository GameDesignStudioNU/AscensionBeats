using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Level : MonoBehaviour
{
    public List<ObstacleData> obstacleData;
    public string name;
    public string[] jsonFileNames;
    public string[] stringTimeStamps;
    public int[] prefabPoolSizes;
    private string path;
    private string jsonString;
    private int _numPrefabs;
    private float _time;
    [SerializeField]private GameObject[] donuts;
    [SerializeField]private AudioSource audio;
    [SerializeField]private ObjectPool[] _prefabPool;
    
    void Start()
    {
        for(int i = 0; i < 2; i++) {
            donuts[i].SetActive(false);
        }
        
        for(int i = 0; i < jsonFileNames.Length; i++) {
            path = Application.streamingAssetsPath + "/" + name + "/" + jsonFileNames[i];
            jsonString = File.ReadAllText(path);
            obstacleData.Add(JsonUtility.FromJson<ObstacleData>(jsonString));
            obstacleData[i].numTimeStamps = obstacleData[i].timeStamps.Length;
        } 

        _numPrefabs = obstacleData.Count;
        _prefabPool = new ObjectPool[_numPrefabs];
        for(int i = 0; i < _numPrefabs; i++) {
            if(obstacleData[i].isStatic) break;
            _prefabPool[i] = new ObjectPool(prefabPoolSizes[i], Resources.Load("Prefabs/" + obstacleData[i].name) as GameObject);
        }

        audio = GetComponent<AudioSource>();
        PlaySong();
        _time = Time.time;
    }

    void Update()
    {
        _time += 1000 * Time.deltaTime;
        for(int i = 0; i < _numPrefabs; i++) {
            if(obstacleData[i].currentIndex < obstacleData[i].numTimeStamps && isBeat(obstacleData[i].timeStamps[obstacleData[i].currentIndex])) {
                if(obstacleData[i].name == "Donut") {
                    if(obstacleData[i].currentIndex == 0) {
                        for(int j = 0; j < 2; j++) {
                            donuts[j].SetActive(true);
                        }
                    }
                    else {
                        for(int j = 0; j < 2; j++) {
                            donuts[j].GetComponent<Donut>().OnBeatEvent();
                        }
                    }
                    obstacleData[i].currentIndex++;
                    if(obstacleData[i].currentIndex == obstacleData[i].numTimeStamps) {
                        for(int j = 0; j < 2; j++) {
                            donuts[j].SetActive(false);
                        }
                    }
                }
                else {
                    _prefabPool[i].CreateObject();
                    obstacleData[i].currentIndex++;
                }
            }
        }
    }

    void PlaySong()
    {
        audio.Play();
    }

    bool isBeat(int timeStamp) {
        return(timeStamp > 0 && _time >= timeStamp);
    }
}
