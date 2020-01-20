using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class game : Mirror.NetworkBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab;
    public GameObject indice;
    public Material[] mat;
    public Material[] color;
    [Mirror.SyncVar]
    private GameObject currentPanneau;
    [Mirror.SyncVar]
    private int currentcolor;
    [Mirror.SyncVar]
    private List<Tuple<int, int>> paring;
    [Mirror.SyncVar]
    private int current;
    [Mirror.SyncVar]
    private float seconds;
    public Text m_text;
    public Text textspeed;
    public Text infoText;
    [Mirror.SyncVar]
    public GameObject train;
    [Mirror.SyncVar]
    private movetrain traininfo;
    [Mirror.SyncVar]
    private bool bonus = false;
    [Mirror.SyncVar]
    private float bonustime;
    [Mirror.SyncVar]
    private bool started = false;
    [Mirror.SyncVar]
    private float speed = 0;


    [Mirror.Command]
    void generateCube(int i, int j)
    {
        float x, y, z;
        x = train.transform.position.x;
        z = train.transform.position.z - 20;
        y = 2.5f;
        GameObject c = Instantiate(indice, new Vector3(x, y, z), Quaternion.identity);
        c.transform.parent = train.transform;
        c.transform.Find("Front").gameObject.GetComponent<Renderer>().material = mat[i];
        c.transform.Find("Back").gameObject.GetComponent<Renderer>().material = color[j];
        c.transform.Find("Bottom").gameObject.GetComponent<Renderer>().material = color[j];
        c.transform.Find("Top").gameObject.GetComponent<Renderer>().material = color[j];
        c.transform.Find("Left").gameObject.GetComponent<Renderer>().material = color[j];
        c.transform.Find("Right").gameObject.GetComponent<Renderer>().material = color[j];
        Mirror.NetworkServer.Spawn(c);
    }

    [Mirror.Command]
    public void clickButton(int color)
    {
        if (started)
        {
            if (currentcolor == color)
            {
                seconds -= 10;
                Destroy(currentPanneau);
                generatePanneau();
                traininfo.add_speed(0.1f);
                bonus = true;
                bonustime = Time.time;
            }
            else
            {
                seconds += 20;
                if (traininfo.get_speed() >= 0.05)
                    traininfo.add_speed(-0.05f);
            }
        }
    }


    void generateParing()
    {
        if (isServer)
        {
            started = true;
            for (int i = 0; i < mat.Length; i++)
            {
                paring.Add(new Tuple<int, int>(i, Random.Range(0, color.Length)));
                generateCube(i, paring[i].Item2);
            }
            generatePanneau();
            traininfo.set_speed(0.11f);
        }
    }

    [Mirror.Command]
    void generatePanneau()
    {
        current = Random.Range(0, 20);
        currentcolor = paring[current].Item2;
        Destroy(currentPanneau);
        currentPanneau = Instantiate(prefab, train.transform.position + new Vector3(2.8f, 0.5f, 50), Quaternion.identity);
        GameObject face = currentPanneau.transform.Find("sign/Front").gameObject;
        face.GetComponent<Renderer>().material = mat[current];
        Mirror.NetworkServer.Spawn(currentPanneau);
    }

    void Start()
    {
        paring = new List<Tuple<int, int>>();
        traininfo = (movetrain)train.GetComponent(typeof(movetrain));
        traininfo.set_speed(0);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(started)
        {
            if(isServer)
            {
                seconds += Time.deltaTime;
                if (bonus && Time.time - bonustime > 2)
                    bonus = false;
                if (!bonus)
                    if (traininfo.get_speed() > 0.02)
                        traininfo.add_speed(-0.0001f);

                speed = traininfo.get_speed();
            }
            m_text.text = "Time : " + ((int)seconds / 60).ToString() + ":" + ((int)seconds % 60).ToString("00");
            textspeed.text = "Speed : " + (1000 * traininfo.get_speed()).ToString("00");
            if (!bonus)
                if (traininfo.get_speed() > 0.02)
                    traininfo.add_speed(-0.0001f);
            traininfo.set_speed(speed);
        }
        else
        {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                infoText.text = "Waiting for player 2";
                if (players.Length == 2)
                {
                    players[0].transform.parent = train.transform;
                    players[1].transform.parent = train.transform;
                    

                    generateParing();
                    infoText.text = "";
                }
            
        }
    }
}
