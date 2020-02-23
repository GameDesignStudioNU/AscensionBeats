using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Player player;
    [Range(0f, 2f)]
    public float shiftFactor;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);
    }
}
