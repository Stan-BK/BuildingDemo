using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove: MonoSingleton<PlayerMove>
{
    CharacterController controller;
    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();

    }
    private void Start()
    {
        //PlayerModelManager.Instance.PlaceBuilding();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward, Color.green);
        controller.Move(new Vector3(Input.GetAxis("Vertical") * Time.deltaTime * 10, 0, -Input.GetAxis("Horizontal") * Time.deltaTime * 10));
    }
}
