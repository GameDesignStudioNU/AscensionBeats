using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private SpriteRenderer Back_Layer;
    private SpriteRenderer Middle_Layer;
    private SpriteRenderer Front_Layer;
    private SpriteRenderer Shade_Layer;

    private Player player;

    private float initial_height;
    private float initial_player_height;
    private float max_height;
    private float delta_height;

    void Start()
    {
        Back_Layer = transform.Find("Back Layer").gameObject.GetComponent<SpriteRenderer>();
        Middle_Layer = transform.Find("Middle Layer").gameObject.GetComponent<SpriteRenderer>();
        Front_Layer = transform.Find("Front Layer").gameObject.GetComponent<SpriteRenderer>();
        Shade_Layer = transform.Find("Shade Layer").gameObject.GetComponent<SpriteRenderer>();

        player = FindObjectOfType<Player>();

        initial_height = Shade_Layer.transform.position.y;
        initial_player_height = player.transform.position.y;
        max_height = initial_height + 500;
        
    }

    // Update is called once per frame
    void Update()
    {
        delta_height = player.transform.position.y - initial_player_height;

        Back_Layer.transform.position = new Vector3(Back_Layer.transform.position.x, initial_height + .95f * delta_height, Back_Layer.transform.position.z);
        Middle_Layer.transform.position = new Vector3(Middle_Layer.transform.position.x, initial_height + .90f * delta_height, Middle_Layer.transform.position.z);
        Front_Layer.transform.position = new Vector3(Front_Layer.transform.position.x, initial_height + .75f * delta_height, Front_Layer.transform.position.z);
        Shade_Layer.transform.position = new Vector3(Shade_Layer.transform.position.x, initial_height + delta_height, Shade_Layer.transform.position.z);
    }
}
