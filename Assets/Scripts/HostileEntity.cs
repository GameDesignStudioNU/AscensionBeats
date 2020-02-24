using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HostileEntity : MonoBehaviour {

    public void Activate() {
        Vector2 startPos = GetStartPosition();
        this.gameObject.transform.position = new Vector3(startPos.x, startPos.y, 10);
        this.gameObject.SetActive(true);
    }

    public void Deactivate() {
        this.gameObject.SetActive(false);
    }

    public abstract Vector2 GetStartPosition();
}
