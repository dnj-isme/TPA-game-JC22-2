using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool instance;
    public static ObjectPool Instance { get => instance; }
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField, Range(1, 100)] private int amountToPool = 5;
    [SerializeField] private List<GameObject> pooledObjects;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(prefabs[Random.Range(0, prefabs.Capacity)]);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
