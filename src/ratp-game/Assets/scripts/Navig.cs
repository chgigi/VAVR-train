using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Navig : Mirror.NetworkBehaviour
{

    // Use this for initialization

    public Camera camera;
    public float walkingSpeed = 0.2f;
    float rotationSpeed = 3.0f;

    public override void OnStartLocalPlayer()
    {
        camera.gameObject.active = true;
        gameObject.transform.Find("Canvas").gameObject.active = true;
        foreach (Transform child in GameObject.Find("game prefab package/Tram_joueur1/console").transform)
        {
            click g = (click)child.gameObject.GetComponent(typeof(click));
            g.set_manager(transform.gameObject);
        }
    }

    void Start () {

	}
	
	// Update is called once per frame
	void Update ()
    {
        if(isLocalPlayer)
        {
            if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0, 0, 1) * walkingSpeed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0, 0, -1) * walkingSpeed);
            }
            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector3(-1, 0, 0) * walkingSpeed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector3(1, 0, 0) * walkingSpeed);
            }
            float h = rotationSpeed * Input.GetAxis("Mouse X");
            float v = rotationSpeed * Input.GetAxis("Mouse Y");

            transform.RotateAround(transform.position, transform.TransformVector(Vector3.up), h);
            camera.transform.RotateAround(transform.position, camera.transform.TransformVector(Vector3.left), v);
        }
    }
}
