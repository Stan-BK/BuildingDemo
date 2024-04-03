using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerModelManager : MonoSingleton<PlayerModelManager>
{
    public BuildingsManifestSO manifest;
    public BuildingsManifestSO myManifest;
    public List<Building> BuildingsManifest = new ();
    public PlaceController placeController;
    public GameObject OptionBtn;
    public Camera Camera;
    public GameObject currentSelectObject;
    public Material PlaceMaterial;
    List<Building> BuildingsList = new ();
    Dictionary<GameObject, Building> BuildingsDic = new();
    public bool isPlacing
    {
        get => placeController.isPlacing;
    }

    protected override void Awake()
    {
        base.Awake();
        BuildingsManifest = manifest.Buildings;
        currentSelectObject = null;
    }

    private void Start()
    {
        InitAllBuildings();
    }

    public void PlaceBuilding()
    {
        if (isPlacing)
        {
            if (placeController.PlaceBuilding())
            {
                BuildingsDic.TryGetValue(currentSelectObject, out Building b);
                if (b == null)
                {
                    b = new Building(currentSelectObject.transform.position, currentSelectObject.transform.rotation);
                    b.building = currentSelectObject.GetComponent<IBuilding>().prototype.building;
                    BuildingsList.Add(b);
                    BuildingsDic.Add(currentSelectObject, b);
                } else
                {
                    b.position = currentSelectObject.transform.position;
                    b.rotation = currentSelectObject.transform.rotation;
                }
                ResetPlaceOption();
                SaveAllBuildings();
            }
        } else if (currentSelectObject != null)
            placeController.PlaceBuilding(currentSelectObject);
    }

    void InitAllBuildings()
    {
        foreach (Building b in myManifest.Buildings) {
            BuildingsDic.Add(placeController.CreatePlaceBuilding(b), b);
            BuildingsList.Add(b);
        }
    }

    void SaveAllBuildings()
    {
        myManifest.Buildings = BuildingsList;
    }

    public void PlaceOption(GameObject gameObject)
    {
        OptionBtn.SetActive(true);
        currentSelectObject = gameObject;
        OptionBtn.transform.position = Camera.WorldToScreenPoint(gameObject.transform.position);
    }

    public void ResetPlaceOption()
    {
        OptionBtn.SetActive(false);
        currentSelectObject = null;
    }
}
