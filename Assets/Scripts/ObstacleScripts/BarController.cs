using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarController : MonoBehaviour
{
    private float t = 0f;
    SpriteRenderer renderer;
    Color newColor;

    void Awake()
    {
        renderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float alpha = Mathf.Lerp(0.0f, 1.0f, t);
        newColor = new Color(renderer.color.r, renderer.color.g, renderer.color.b, alpha);
        renderer.color = newColor;
        if(alpha == 1.0f) {
            gameObject.tag = "Obstacle";
            renderer.color = Color.red;
            StartCoroutine(Destroy());
        }
        t += Time.deltaTime*1f;
    }

    IEnumerator Destroy() 
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
