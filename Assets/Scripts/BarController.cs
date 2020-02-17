using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarController : MonoBehaviour
{
    private float t = 0f;
    SpriteRenderer renderer;
    Color originalColor;

    void Awake()
    {
        renderer = this.gameObject.GetComponent<SpriteRenderer>();
        originalColor = renderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        renderer.color = Color.Lerp(Color.clear, originalColor, t);
        if(renderer.color == originalColor) {
            Destroy(this.gameObject);
        }
        t += Time.deltaTime*1f;
    }
}
