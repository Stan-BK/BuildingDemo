using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerModelManager : MonoSingleton<PlayerModelManager>
{
    public BuildingsManifestSO manifest;
    public BuildingsManifestSO myManifest;
    public PlaceController placeController;
    public GameObject OptionBtn;
    public TMP_Text CancelBtnText;
    GameObject currentSelectObject;
    public Material PlaceMaterial;
    public List<Building> BuildingsList = new(); // 存储玩家拥有建筑集合
    public Dictionary<GameObject, Building> BuildingsDic = new(); // 存储场景内已放置建筑集合
    public Camera Camera;
    public bool isPlacing
    {
        get => placeController.isPlacing;
    }

    protected override void Awake()
    {
        base.Awake();
        currentSelectObject = null;
    }

    private void Start()
    {
        InitAllBuildings();
    }

    public void PlaceBuilding(Building building)
    {
        placeController.PlaceBuilding(building);
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
                }
                else
                {
                    b.position = currentSelectObject.transform.position;
                    b.rotation = currentSelectObject.transform.rotation;
                }
                b.isPlace = true;
                ResetPlaceOption();
                SaveAllBuildings();
            }
        }
        else if (currentSelectObject != null)
            // 激活当前选中物体的放置状态
            placeController.PlaceBuilding(currentSelectObject);
    }

    void InitAllBuildings()
    {
        foreach (Building b in myManifest.Buildings)
        {
            if (b.isPlace)
            {
                BuildingsDic.Add(placeController.InitPlaceBuilding(b), b);
            }
            BuildingsList.Add(b);
        }
    }

    void SaveAllBuildings()
    {
        myManifest.Buildings = BuildingsList;
    }

    public void CancelPlacing()
    {
        placeController.CancelPlacing(currentSelectObject);
    }

    public void PlaceOption(GameObject gameObject)
    {
        OptionBtn.SetActive(true);
        if (isPlacing)
        {
            CancelBtnText.text = "Cancel";
        }
        else
        {
            CancelBtnText.text = "Remove";
        }
        currentSelectObject = gameObject;
        OptionBtn.transform.position = Camera.WorldToScreenPoint(gameObject.transform.position);
    }

    public void ResetPlaceOption()
    {
        OptionBtn.SetActive(false);
        currentSelectObject = null;
    }
}
