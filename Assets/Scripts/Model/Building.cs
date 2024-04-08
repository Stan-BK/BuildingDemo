using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building
{
    int ID;
    public int count = 1;
    public string name;
    [HideInInspector] public bool isPlace;
    public GameObject building; // 预制体对象
    [HideInInspector]public Vector3 position;
    [HideInInspector]public Quaternion rotation;

    public Building(int count, GameObject gameObject)
    {
        this.count = count;
        building = gameObject;
    }
    public Building(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}
