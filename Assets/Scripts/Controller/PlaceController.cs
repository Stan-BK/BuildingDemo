using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceController : MonoBehaviour
{
    public GameObject? currentPlaceObject = null;
    DetectController detectController = null;
    Dictionary<GameObject, Stack<GameObject>> waitForPlaces = new();  // 备用建筑存储池，根据建筑原型prototype为键名进行存储
    public bool isPlacing;
    public Camera Camera;


    private void Awake()
    {
        isPlacing = false;

    }
    // Update is called once per frame
    public bool PlaceBuilding()
    {
        return PlaceCurrentBuilding();
    }

    public void PlaceBuilding(GameObject placeObject)
    {
        isPlacing = true;
        currentPlaceObject = placeObject;
        currentPlaceObject.transform.parent = Camera.gameObject.transform;
        detectController = currentPlaceObject.GetComponent<DetectController>();
        currentPlaceObject.GetComponent<TransformController>().enabled = true;
        detectController.EnableDetect();
    }

    // 从库存放置新建筑
    public void PlaceBuilding(Building building)
    {
        GameObject go;
        // 预先处理还原正在放置建筑
        if (isPlacing)
        {
            if (PlayerModelManager.Instance.BuildingsDic.TryGetValue(currentPlaceObject, out Building currentBuilding))
            {
                if (currentBuilding.isPlace)
                {
                    // 初始化到原始放置
                    currentPlaceObject.transform.position = currentBuilding.position;
                    currentPlaceObject.transform.rotation = currentBuilding.rotation;
                    currentPlaceObject.transform.parent = PlayerModelManager.Instance.transform;
                }
            }
            else
            {
                currentPlaceObject.SetActive(false);
                if (waitForPlaces.TryGetValue(detectController.prototype.building, out Stack<GameObject> gameObjects))
                {
                    gameObjects.Push(currentPlaceObject);
                }
                else
                {
                    Stack<GameObject> stack = new();
                    stack.Push(currentPlaceObject);
                    waitForPlaces.Add(detectController.prototype.building, stack);
                }
            }
            DisablePlaceMode();
        }

        if (waitForPlaces.TryGetValue(building.building, out Stack<GameObject> gos) &&
            (go = gos.Pop()) != null)
        {
            go.SetActive(true);
            PlaceBuilding(go);
        }
        else
        {
            CreatePlaceBuilding(building);
        }
    }

    public void CreatePlaceBuilding(Building Building)
    {
        isPlacing = true;
        currentPlaceObject = Instantiate(Building.building, Camera.gameObject.transform);
        currentPlaceObject.transform.localPosition = new Vector3(0, 0, 5f);

        currentPlaceObject.AddComponent<TransformController>();
        detectController = currentPlaceObject.AddComponent<DetectController>();
        detectController.prototype = Building;

        var bound = currentPlaceObject.GetComponent<MeshCollider>().bounds.max;
        var playerTrans = Camera.gameObject.transform;
        var forward = playerTrans.transform.forward;
        //currentPlaceObject.transform.localPosition += new Vector3(forward.x, 0, forward.z) * Mathf.Max(bound.x, bound.y);
    }

    // 初始化建筑放置
    public GameObject InitPlaceBuilding(Building Building)
    {
        var obj = Instantiate(Building.building);
        obj.transform.position = Building.position;
        obj.transform.rotation = Building.rotation;

        obj.AddComponent<TransformController>().enabled = false;
        obj.AddComponent<DetectController>().prototype = Building;
        return obj;
    }

    bool PlaceCurrentBuilding()
    {
        if (detectController.Valid())
        {
            if (!PlayerModelManager.Instance.BuildingsDic.TryGetValue(currentPlaceObject, out Building currentBuilding))
            {
                currentBuilding = detectController.prototype;
                PlayerModelManager.Instance.BuildingsDic.Add(currentPlaceObject, currentBuilding);
            }
            if (!PlayerModelManager.Instance.BuildingsList.Contains(currentBuilding))
            {
                PlayerModelManager.Instance.BuildingsList.Add(currentBuilding);
            }
            currentPlaceObject.transform.parent = PlayerModelManager.Instance.transform;
            currentBuilding.position = currentPlaceObject.transform.position;
            currentBuilding.rotation = currentPlaceObject.transform.rotation;
            currentBuilding.isPlace = true;

            isPlacing = false;
            detectController.DisableDetect();
            currentPlaceObject.GetComponent<TransformController>().enabled = false;
            currentPlaceObject = null;
            return true;
        }
        return false;
    }

    void DisablePlaceMode()
    {
        detectController.DisableDetect();
        currentPlaceObject.GetComponent<TransformController>().enabled = false;
        detectController = null;
        currentPlaceObject = null;
    }

    public void CancelPlacing(GameObject gameObject)
    {
        if (isPlacing)
        {
            if (PlayerModelManager.Instance.BuildingsDic.TryGetValue(currentPlaceObject, out Building currentBuilding))
            {
                if (currentBuilding.isPlace)
                {
                    // 初始化到原始放置
                    currentPlaceObject.transform.position = currentBuilding.position;
                    currentPlaceObject.transform.rotation = currentBuilding.rotation;
                    currentPlaceObject.transform.parent = PlayerModelManager.Instance.transform;
                }
                else
                {
                    if (waitForPlaces.TryGetValue(currentBuilding.building, out Stack<GameObject> gameObjects))
                    {
                        gameObjects.Push(currentPlaceObject);
                    }
                    else
                    {
                        currentPlaceObject.SetActive(false);
                        Stack<GameObject> stack = new();
                        stack.Push(currentPlaceObject);
                        waitForPlaces.Add(currentBuilding.building, stack);
                    }
                }
            }
        }
        else
        {
            var dc = gameObject.GetComponent<DetectController>();
            if (waitForPlaces.TryGetValue(dc.prototype.building, out Stack<GameObject> gameObjects))
            {
                gameObjects.Push(gameObject);
            }
            else
            {
                Stack<GameObject> stack = new();
                stack.Push(gameObject);
                waitForPlaces.Add(dc.prototype.building, stack);
            }
            gameObject.SetActive(false);
        }
        DisablePlaceMode();
        isPlacing = false;
    }
}
