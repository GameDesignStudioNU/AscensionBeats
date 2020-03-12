using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes2D;

public class Donut : MonoBehaviour
{
    public int donutProjectilePoolSize = 200;
    private Shapes2D.Shape shape;
    private Vector3 initialScale;
    private Vector3 finalScale;
    private float scaleFactor = 3f;
    private float timeScale = 0.5f;
    [SerializeField] private int projectileAmt;
    [SerializeField] private float startAngle;
    [SerializeField] private float endAngle;
    [SerializeField] private ObjectPool donutProjectiles;

    void Start()
    {
        projectileAmt = 5;
        startAngle = 90f;
        endAngle = 270f;

        donutProjectiles = new ObjectPool(donutProjectilePoolSize, Resources.Load("Prefabs/DonutProjectile") as GameObject);
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
        Fire();
    }

    void Fire() {
        float angleStep = (endAngle - startAngle) / projectileAmt;
        float angle = startAngle;

        for(int i = 0; i <= projectileAmt; i++) {
            float xDir = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float yDir = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 moveVector = new Vector3(xDir, yDir, 0f);
            Vector2 dir = (moveVector - transform.position).normalized;

            GameObject donutProjectile = donutProjectiles.CreateObject(transform.position);
            // if(donutProjectile == null) {return;}
            donutProjectile.GetComponent<DonutProjectile>().SetMoveDir(dir);

            angle += angleStep;
        }
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
