using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera camera;
    Player player;
    [Range(0f, 2f)]
    public float shiftFactor;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindObjectOfType<Camera>();
        player = GameObject.FindObjectOfType<Player>();
        camera.transform.position = new Vector3(camera.transform.position.x, player.transform.position.y, camera.transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y + shiftFactor * Time.deltaTime, camera.transform.position.z);
    }
}
