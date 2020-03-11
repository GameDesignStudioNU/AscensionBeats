using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class StrobeLights : MonoBehaviour
{
    GameObject leftStrobeLight, rightStrobeLight;
    Light2D LS_Comp, RS_Comp;

    public float max_radius;
    public float decay_speed;

    private Vector2 topLeft;
    private Vector2 topRight;

    void Start()
    {
        leftStrobeLight = transform.Find("Strobe Light Left").gameObject;
        rightStrobeLight = transform.Find("Strobe Light Right").gameObject;

        LS_Comp = leftStrobeLight.GetComponent<Light2D>();
        RS_Comp = rightStrobeLight.GetComponent<Light2D>();

    }

    void Update()
    {
        leftStrobeLight.transform.position = topLeft;
        rightStrobeLight.transform.position = topRight;

        LS_Comp.pointLightOuterRadius = Mathf.Lerp(LS_Comp.pointLightOuterRadius, 0, decay_speed * Time.deltaTime);
        RS_Comp.pointLightOuterRadius = Mathf.Lerp(RS_Comp.pointLightOuterRadius, 0, decay_speed * Time.deltaTime);

        topLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight));
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight));
    }

    public void Strobe()
    {
        LS_Comp.pointLightOuterRadius = Mathf.Lerp(LS_Comp.pointLightOuterRadius, max_radius, 20f * Time.deltaTime);
        RS_Comp.pointLightOuterRadius = Mathf.Lerp(RS_Comp.pointLightOuterRadius, max_radius, 20f * Time.deltaTime);
    }
}
