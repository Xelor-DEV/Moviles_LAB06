using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolStatic : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectPool = new List<GameObject>();
    [SerializeField] private GameObject objPrefab;
    [SerializeField] private int maxQuantity;


    void Start() => InstantiateObjects();

    public void InstantiateObjects()
    {
        for (int i = 0; i < maxQuantity; i++)
        {
            GameObject obj = Instantiate(objPrefab, transform);
            obj.GetComponent<DestructibleEntity>().SetPool(this);
            obj.SetActive(false);
            objectPool.Add(obj);
        }
    }

    public void GetObject(Vector3 position)
    {
        if (objectPool.Count == 0) return;

        GameObject obj = objectPool[0];
        objectPool.RemoveAt(0);
        obj.transform.position = position;
        obj.SetActive(true);
        obj.GetComponent<DestructibleEntity>().Init();
    }

    public void SetObject(GameObject obj)
    {
        obj.SetActive(false);
        objectPool.Add(obj);
    }
}