using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectController : MonoBehaviour, IBuilding
{
    Collider Collider;
    public bool isPlacing = true;
    public bool isCollide = false;
    Color originColor;
    Material PlaceMaterial;

    public Building prototype { get; set; }
    public System.Numerics.Vector3 position {  get; set; }
    public System.Numerics.Quaternion rotation { get; set; }

    void Awake()
    {
        isPlacing = true;
        if (!(Collider = GetComponent<Collider>()))
        {
            Collider = gameObject.AddComponent<MeshCollider>();
            (Collider as MeshCollider).convex = true;
        }

        Collider.isTrigger = true;

        Rigidbody rgb = gameObject.AddComponent<Rigidbody>();
        rgb.useGravity = false;
        InitBuildingMaterail();
    }

    public void InitBuildingMaterail()
    {
        PlaceMaterial = PlayerModelManager.Instance.PlaceMaterial;
        originColor = PlaceMaterial.color;

        Material[] materials = gameObject.GetComponent<MeshRenderer>().materials;
        Material[] p = new Material[materials.Length + 1];
        Array.Copy(materials, p, materials.Length);
        p.SetValue(PlaceMaterial, p.Length - 1);
        gameObject.GetComponent<MeshRenderer>().materials = p;
    }

    public void EnableDetect()
    {
        isPlacing = true;
        PlaceMaterial.color = originColor;
    }

    public void DisableDetect()
    {
        isPlacing = false;
        PlaceMaterial.color = Color.clear;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isPlacing) return;
        isCollide = true;
        CollideMaterial();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isPlacing) return;
        isCollide = false;
        CollideMaterial();
    }

    private void OnMouseEnter()
    {
        if (PlayerModelManager.Instance.isPlacing == true && !isPlacing) return;
        PlayerModelManager.Instance.PlaceOption(gameObject);
    }

    private void OnMouseExit()
    {
        if (PlayerModelManager.Instance.isPlacing == true && !isPlacing) return;
        PlayerModelManager.Instance.ResetPlaceOption();
    }

    private void OnDestroy()
    {
        isCollide = false;
        CollideMaterial();
    }

    void CollideMaterial()
    {
        if (isCollide)
        {
            PlaceMaterial.color = Color.red;
        } else
        {
            PlaceMaterial.color = originColor;
        }
    }

    public bool Valid()
    {
        return !isCollide;
    }
}
