using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private SongData[] allSongData;
    private SongData currentSongData;
    private int numSongs;
    private AudioBeatEventEmitter eventEmitter;
    public static GameManager instance;
    void Awake() {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start() {
        // //initialize dictionary of SongData with the scene name as the key
        // allSongData = GetComponents<SongData>();
        // numSongs = allSongData.Length;

        // //get current scene build index
        // Scene currentScene = SceneManager.GetActiveScene();
        // sceneIndex = currentScene.buildIndex;
        // if(sceneIndex < numSongs) {
        //     currentSongData = allSongData[sceneIndex];
        // }
        // else {
        //     Debug.Warn("There are more scenes than there are songs!");
        // }

        // //initialize eventEmitter
        // eventEmitter = GetComponent<AudioBeatEventEmitter>();
    }

    // Update is called once per frame
    void Update() {
        
    }
}
