using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HostileEntity : MonoBehaviour {
    protected Vector2 startPos;
    public void Activate(Vector3 pos) {
        startPos = new Vector2(pos.x, pos.y);
        SetStartPosition();
        this.gameObject.transform.position = new Vector3(startPos.x, startPos.y, 0);
        this.gameObject.SetActive(true);
    }

    public void Deactivate() {
        this.gameObject.SetActive(false);
    }

    public abstract void SetStartPosition();
}
