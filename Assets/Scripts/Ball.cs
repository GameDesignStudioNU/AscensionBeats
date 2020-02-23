using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{ 
    public SpringJoint2D spring;

    void Awake()
    {
        spring = gameObject.GetComponent<SpringJoint2D>();
        spring.connectedAnchor = gameObject.transform.position;
        spring.enabled = false;
    }

    void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2)) {
            spring.enabled = true;
        }
    }

    void Update()
    {
        if (spring.enabled == true)
        {
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spring.connectedAnchor = cursorPosition;
        }

        if (Input.GetKeyUp(KeyCode.Mouse2))
        {
            spring.enabled = false;
        }
    }
}


