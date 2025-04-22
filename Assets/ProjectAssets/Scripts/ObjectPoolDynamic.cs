using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolDynamic : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectPool = new List<GameObject>();
    [SerializeField] private GameObject objPrefab;

    public GameObject ObjPrefab
    {
        get
        {
            return objPrefab;
        }
    }
    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        GameObject obj;

        if (objectPool.Count > 0)
        {
            obj = objectPool[0];
            objectPool.RemoveAt(0);
        }
        else
        {
            obj = Instantiate(objPrefab, transform);
            obj.GetComponent<Projectile>().SetPool(this);
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    public void SetObject(GameObject obj)
    {
        obj.SetActive(false);
        objectPool.Add(obj);
    }
}