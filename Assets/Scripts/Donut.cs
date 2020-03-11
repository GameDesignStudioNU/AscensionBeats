using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes2D;

public class Donut : MonoBehaviour
{
    Shapes2D.Shape shape;

    Vector3 initialScale;
    Vector3 finalScale;
    float scaleFactor = 3f;
    float timeScale = 0.5f;
    void Start()
    {
        shape = GetComponent<Shapes2D.Shape>();
        initialScale = transform.localScale;
        finalScale = new Vector3(initialScale.x + scaleFactor, initialScale.y + scaleFactor, initialScale.z);
    }

    void Update()
    {
        shape.settings.fillRotation += 2;

        if(shape.settings.fillRotation >= 360) shape.settings.fillRotation = 0;
    }

    public void OnBeatEvent() {
        StartCoroutine("Scale");
    }

    IEnumerator LerpUp() {
        float progress = 0;

        while(progress <= 1) {
            transform.localScale = Vector3.Lerp(initialScale, finalScale, progress);
            progress += Time.deltaTime * 2 * timeScale;
            yield return null;
        }

        transform.localScale = finalScale;
    }

    IEnumerator LerpDown() {
        float progress = 0;

        while(progress <= 1) {
            transform.localScale = Vector3.Lerp(finalScale, initialScale, progress);
            progress += Time.deltaTime * timeScale;
            yield return null;
        }

        transform.localScale = initialScale;
    }

    IEnumerator Scale() {
        yield return StartCoroutine("LerpUp");
        yield return StartCoroutine("LerpDown");
    }
}
