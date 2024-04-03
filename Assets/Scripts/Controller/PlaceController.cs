using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceController : MonoBehaviour
{
    public GameObject? currentPlaceObject = null;
    DetectController detectController = null;
    Dictionary<GameObject, List<GameObject>> waitForPlaces;  
    public Material placeMaterial;
    public bool isPlacing;

    private void Start()
    {
        //InitPlaceBuilding(PlayerModelManager.Instance.BuildingsManifest[0]);

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
        currentPlaceObject.transform.parent = PlayerMove.Instance.gameObject.transform;
        detectController = currentPlaceObject.GetComponent<DetectController>();
        currentPlaceObject.GetComponent<TransformController>().enabled = true;
        detectController.EnableDetect();
    }

    public void InitPlaceBuilding(Building Building)
    {
        isPlacing = true;
        currentPlaceObject = Instantiate(Building.building, PlayerMove.Instance.gameObject.transform);
        currentPlaceObject.transform.localPosition = new Vector3(0, 0, 1f);

        currentPlaceObject.AddComponent<TransformController>();
        detectController = currentPlaceObject.AddComponent<DetectController>();
        detectController.prototype = Building;

        var bound = currentPlaceObject.GetComponent<MeshCollider>().bounds.max;
        var playerTrans = PlayerMove.Instance.transform;
        var forward = playerTrans.transform.forward;
        //currentPlaceObject.transform.localPosition += new Vector3(forward.x, 0, forward.z) * Mathf.Max(bound.x, bound.y);
    }

    public GameObject CreatePlaceBuilding(Building Building)
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
            isPlacing = false;
            currentPlaceObject.transform.parent = PlayerModelManager.Instance.transform;
            detectController.DisableDetect();
            currentPlaceObject.GetComponent<TransformController>().enabled = false;
            return true;
        }
        return false;
    }

}
