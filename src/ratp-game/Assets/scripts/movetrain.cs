using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movetrain : MonoBehaviour
{
    public float speed = 0.11f;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float get_speed()
    { 
        return speed;
    }

    public void set_speed(float f)
    {
        speed = f;
    }

    public void add_speed(float f)
    {
        speed += f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0, 1) * speed;
    }
}
