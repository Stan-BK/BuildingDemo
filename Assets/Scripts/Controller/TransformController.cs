using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TransformController : MonoBehaviour
{
    float x = 0;

    private void Start()
    { 
    }

    private void OnMouseDown()
    {
        if (!enabled) return;
        x = Input.mousePosition.x;
    }

    private void OnMouseDrag()
    {
        if (!enabled) return;
        transform.Rotate(Vector3.forward, Input.mousePosition.x - x);
        x = Input.mousePosition.x;
    }
}
