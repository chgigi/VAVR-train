using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour {

    public float walkingSpeed = 0.5f;
    public float rotationSpeed = 0.5f;
    public Camera camera;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // Mouvement
		if (Input.GetKey(KeyCode.Z))
        {
            Debug.Log("Up");
            transform.Translate(new Vector3(0, 0, 1) * walkingSpeed);
        }

        else if (Input.GetKey(KeyCode.S))
        {
            Debug.Log("down");
            transform.Translate(new Vector3(0, 0, -1) * walkingSpeed);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("right");
            transform.Translate(new Vector3(1, 0, 0) * walkingSpeed);
        }

        else if (Input.GetKey(KeyCode.Q))
        {
            Debug.Log("left");
            transform.Translate(new Vector3(-1, 0, 0) * walkingSpeed);
        }

        // Rotation
        float h = rotationSpeed * Input.GetAxis("Mouse X");
        float v = rotationSpeed * Input.GetAxis("Mouse Y");


        transform.RotateAround(transform.position, Vector3.up, h);
        camera.transform.RotateAround(camera.transform.position,camera.transform.TransformVector(Vector3.right), v);

    }
}
