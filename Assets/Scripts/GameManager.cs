using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject vertBar;
    public GameObject horizBar;
    private GameObject player;
    [Range(0,3)]
    public int[] onNthFullBeat;
    [Range(0,7)]
    public int[] onEigthBeat;
    private int nthFullBeat;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        CheckBeat();
    }

    void CheckBeat() {
        nthFullBeat = BPerM.fullBeatCount % 4;
        for(int i = 0; i < onEigthBeat.Length; i++) {
            if(BPerM.onEigthBeat && BPerM.EigthBeatCount % 8 == onEigthBeat[i]) {
                for(int j = 0; j < onNthFullBeat.Length; j++) {
                    if(nthFullBeat == onNthFullBeat[i]) {
                        SpawnOnBeat(vertBar);
                    }
                }
            }
        }
    }

    void SpawnOnBeat(GameObject prefab) {
        Instantiate(prefab, new Vector3(player.transform.position.x + Random.Range(-8f, 8f), player.transform.position.y - 20f, player.transform.position.z), Quaternion.identity);
    }
}