using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class click : MonoBehaviour
{
    public int color;
    private GameObject gamemanager;
    private bool pushed = false;
    private float t = 10; 

    void OnMouseDown()
    {
        game g = (game) gamemanager.GetComponent(typeof(game));
       
        if(!pushed)
        {
            transform.position = transform.position - new Vector3(0, 0.02f, 0);
            pushed = true;
        }
        g.clickButton(color);
    }

    public void set_manager(GameObject player)
    {
        gamemanager = player;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(gamemanager);
        if(pushed)
        {
            t = t - 0.1f;
            if(t < 0)
            {
                t = 10;
                pushed = false;
                transform.position = transform.position + new Vector3(0, 0.02f, 0);
            }
        }
    }
}
