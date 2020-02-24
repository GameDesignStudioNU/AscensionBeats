using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool: MonoBehaviour
{
    private List<GameObject> _pool;
    private int _size;

    public ObjectPool(int size, GameObject prefab) {
        _pool = new List<GameObject>();
        _size = size;
        for(int i = 0; i < size; i++) {
            GameObject obj = (GameObject)Instantiate(prefab);
            obj.SetActive(false);
            _pool.Add(obj);
        }
    }

    private GameObject GetObject() {
        for(int i = 0; i < _size; i++) {
            if(!_pool[i].activeInHierarchy) {
                return _pool[i];
            }
        }
        return null;
    }

    private void RemoveObject(GameObject obj) {
        for(int i = 0; i < _size; i++) {
            if(_pool[i] == obj) {
                obj.SetActive(false);
            }
        }
    }

    public void CreateObject() {
        GameObject obj = GetObject();
        if(obj != null) {
            HostileEntity e = obj.GetComponent<HostileEntity>();
            e.Activate();
        }
    }
}
