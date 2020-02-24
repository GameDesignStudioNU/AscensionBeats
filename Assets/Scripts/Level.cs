using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Level : MonoBehaviour
{
    public GameObject[] prefabs;
    public string[] stringTimeStamps;
    public int[] prefabPoolSizes;
    private AudioSource audio;
    [SerializeField]private ObjectPool[] _prefabPool;
    [SerializeField]private TimeStamp[] _timeStamps;
    [SerializeField]private float _startTime;
    [SerializeField]private int _numPrefabs;
    [SerializeField]private int[] _indices;
    void Start()
    {
        _numPrefabs = prefabs.Length;
        _indices = new int[_numPrefabs];
        _prefabPool = new ObjectPool[_numPrefabs];
        _timeStamps = new TimeStamp[_numPrefabs];
        audio = GetComponent<AudioSource>();
        for(int i = 0; i < _numPrefabs; i++) {
            _prefabPool[i] = new ObjectPool(prefabPoolSizes[i], prefabs[i]);
            _timeStamps[i] = new TimeStamp(stringTimeStamps[i]);
            _indices[i] = 0;
        }

        _startTime = Time.time;
        PlaySong();
    }

    void Update()
    {
        _startTime += 1000 * Time.deltaTime;
        for(int i = 0; i < _numPrefabs; i++) {
            if(isBeat(_timeStamps[i].getTimeStamp(_indices[i]))) {
                _prefabPool[i].CreateObject();
                _indices[i]++;
            }
        }
    }

    void PlaySong()
    {
        audio.Play();
    }

    bool isBeat(int timeStamp) {
        return(timeStamp > 0 && _startTime >= timeStamp);
    }
}
