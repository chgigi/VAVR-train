using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navig : MonoBehaviour {

    // Use this for initialization

    public Camera camera;
    public float walkingSpeed = 0.2f;
    float rotationSpeed = 3.0f;

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
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
