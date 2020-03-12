using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool: MonoBehaviour
{
    private List<GameObject> _pool;
    private int _size;
    private GameObject _prefab;

    public ObjectPool(int size, GameObject prefab) {
        _prefab = prefab;
        _pool = new List<GameObject>();
        _size = size;
        for(int i = 0; i < size; i++) {
            AddObject();
        }
    }

    private GameObject GetObject() {
        for(int i = 0; i < _size; i++) {
            if(!_pool[i].activeInHierarchy) {
                return _pool[i];
            }
        }
        GameObject obj = AddObject();
        _size++;
        Grow();
        return obj;
    }

    private void Grow() {
        for(int i = 0; i < _size; i++) {
            GameObject obj = AddObject();
        }
        _size = 2 * _size;
    }

    private GameObject AddObject() {
        GameObject obj = (GameObject)Instantiate(_prefab);
        obj.SetActive(false);
        _pool.Add(obj);
        return obj;
    }

    private void RemoveObject(GameObject obj) {
        for(int i = 0; i < _size; i++) {
            if(_pool[i] == obj) {
                obj.SetActive(false);
            }
        }
    }

    public GameObject CreateObject(Vector3 pos = default(Vector3)) {
        GameObject obj = GetObject();
        if(obj != null) {
            HostileEntity e = obj.GetComponent<HostileEntity>();
            e.Activate(pos);
        }
        return obj;
    }
}
