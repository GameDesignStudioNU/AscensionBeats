using UnityEngine;
using TMPro;

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
    private float lastVertSpawnPoint;
    private bool can_respawn_vert_bar;

    private Vector2 leftScreen;
    private Vector2 rightScreen;

    // PAUSE
    public bool NOT_PAUSED;
    public GameObject pause_menu;

    // Spawn Obstacles
    public bool spawnObstacles = true;

    public int deathCount;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        can_respawn_vert_bar = true;

        deathCount = 0;

        leftScreen = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        rightScreen = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, 0));

        NOT_PAUSED = true;

        GameStats.InitialHeight = Mathf.RoundToInt(player.transform.position.y);
    }

    void Update()
    {
        if (FindObjectOfType<CharacterController2D>().State.PauseButtonPress)
        {
            NOT_PAUSED = !NOT_PAUSED;
        }
        
        if (NOT_PAUSED)
        {
            pause_menu.SetActive(false);

            if (!FindObjectOfType<AudioManager>().audioSource.isPlaying)
                FindObjectOfType<AudioManager>().StartSong();
            if (spawnObstacles)
                CheckBeat();
        }
        else
        {
            if (FindObjectOfType<AudioManager>().audioSource.isPlaying)
                FindObjectOfType<AudioManager>().PauseSong();
            pause_menu.SetActive(true);
        }

        // Update score
        UpdateScore();
        
    }

    private void UpdateScore()
    {
        var current_height = Mathf.RoundToInt(player.transform.position.y) - GameStats.InitialHeight;
        if (current_height > GameStats.MaxHeight)
            GameStats.MaxHeight = current_height;
        if (current_height < 0)
            current_height = 0;
        
        if (FindObjectOfType<Canvas>().transform.Find("Score"))
        {
            TextMeshProUGUI score_text = FindObjectOfType<Canvas>().transform.Find("Score").gameObject.GetComponent<TextMeshProUGUI>();
            score_text.text = string.Format("Top height: {0}\nCurrent height: {1}", GameStats.MaxHeight, current_height);
        }

        if (FindObjectOfType<Canvas>().transform.Find("Deaths"))
        {
            TextMeshProUGUI deathCount_text = FindObjectOfType<Canvas>().transform.Find("Deaths").gameObject.GetComponent<TextMeshProUGUI>();
            deathCount_text.text = string.Format("Deaths: {0}", deathCount);
        }
    }

    void CheckBeat() {
        nthFullBeat = BPerM.fullBeatCount % 4;
        TextMeshProUGUI beat_text = FindObjectOfType<Canvas>().transform.Find("Beat").gameObject.GetComponent<TextMeshProUGUI>();
        beat_text.text = (nthFullBeat + 1).ToString();

        if (nthFullBeat == 0 && can_respawn_vert_bar)
        {
            SpawnOnBeat(vertBar);
            can_respawn_vert_bar = false;
        }
        else if (nthFullBeat == 3 && !can_respawn_vert_bar) {
            can_respawn_vert_bar = true;
        }

        if (0 == nthFullBeat || 2 == nthFullBeat)
            FindObjectOfType<StrobeLights>().Strobe();

    }

    void SpawnOnBeat(GameObject prefab) {
        float newVertSpawnPoint = player.transform.position.x + Random.Range(-16f, 16f);

        while ((Mathf.Abs(newVertSpawnPoint - lastVertSpawnPoint) < 2f) || (newVertSpawnPoint < leftScreen.x) || (newVertSpawnPoint > rightScreen.x - .5f))
        {
            newVertSpawnPoint = player.transform.position.x + Random.Range(-16, 16f);
        }

        Instantiate(prefab, new Vector3(newVertSpawnPoint, player.transform.position.y - 200f, -2.5f), Quaternion.identity);
        lastVertSpawnPoint = newVertSpawnPoint;
    }

    
    public void Save()
    {
        GameStats.RespawnPlayerPos = player.transform.position;
        GameStats.RespawnEqualizerPos = FindObjectOfType<CubeVisualizer>().startScale;
        GameStats.RespawnSongTime = FindObjectOfType<AudioManager>().audioSource.time;
        GameStats.RespawnBeatTimer = FindObjectOfType<BPerM>().beatTimer;
    }

    public void Reload()
    {
        CubeVisualizer[] equalizerBars = FindObjectsOfType<CubeVisualizer>();
        foreach (CubeVisualizer bar in equalizerBars)
        {
            bar.startScale = GameStats.RespawnEqualizerPos;
        }

        BarController[] vertBars = FindObjectsOfType<BarController>();
        foreach (BarController bar in vertBars)
        {
            Destroy(bar.gameObject);
        }

        DashGem[] dashGems = FindObjectsOfType<DashGem>();
        foreach (DashGem gem in dashGems)
        {
            gem.respawnTime = 0;
        }

        player.transform.position = GameStats.RespawnPlayerPos;
        FindObjectOfType<AudioManager>().audioSource.Stop();
        FindObjectOfType<AudioManager>().audioSource.time = GameStats.RespawnSongTime;
        FindObjectOfType<BPerM>().beatTimer = GameStats.RespawnBeatTimer;
        FindObjectOfType<Player>().reloading = false;
    }

}