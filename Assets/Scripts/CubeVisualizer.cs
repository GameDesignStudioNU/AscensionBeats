using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeVisualizer : MonoBehaviour
{
    public int band;
    public float startScale, scaleMultiplier;
    [Range(0f,2f)]
    public float shiftFactor;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        int not_paused = FindObjectOfType<GameManager>().NOT_PAUSED ? 1 : 0;

        startScale += shiftFactor * Time.deltaTime * not_paused;

        //transform.position = new Vector3(transform.position.x, transform.position.y + shiftFactor * Time.deltaTime, transform.position.z);
        transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, AudioManager.bandBuffer[band] * scaleMultiplier + startScale, 10f * Time.deltaTime), transform.localScale.z);
    }
}
