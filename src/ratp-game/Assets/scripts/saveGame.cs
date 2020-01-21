using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class saveGame : Mirror.NetworkBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab;
    public GameObject indice;
    public Material[] mat;
    public Material[] color;
    private GameObject currentPanneau;
    [Mirror.SyncVar(hook = "curcolorcallback")]
    private int currentcolor;
    private List<Tuple<int, int>> paring;
    [Mirror.SyncVar(hook = "curcallback")]
    private int current;
    [Mirror.SyncVar(hook = "seccallback")]
    private float seconds;
    public Text m_text;
    public Text textspeed;
    public Text infoText;
    private GameObject train;
    private movetrain traininfo;
    [Mirror.SyncVar(hook = "bonuscallback")]
    private bool bonus = false;
    private float bonustime;
    [Mirror.SyncVar(hook = "startcallback")]
    private bool started = false;
    [Mirror.SyncVar(hook = "speedcallback")]
    private float speed = 0;
    [Mirror.SyncVar(hook = "seedcallback")]
    private int seed;
    [Mirror.SyncVar(hook = "panneaucallback")]
    private bool newpanneau;


    void panneaucallback(bool pn)
    {
        //Debug.LogWarning("panneau callback on" + isClientOnly.ToString());
        newpanneau = pn;
        Debug.LogWarning("panneau:" + newpanneau.ToString());

    }

    void seedcallback(int s)
    {
        seed = s;
        Random.InitState(s);
    }

    void curcolorcallback(int col)
    {
        currentcolor = col;
    }

    void curcallback(int col)
    {
        current = col;
    }

    void seccallback(float sec)
    {
        seconds = sec;
    }

    void bonuscallback(bool bon)
    {
        bonus = bon;
    }

    void startcallback(bool st)
    {
        started = st;
    }

    void speedcallback(float sp)
    {
        speed = sp;
        //traininfo.set_speed(speed);
    }

    void generateCube(int i, int j)
    {
        float x, y, z;
        x = train.transform.position.x;
        z = train.transform.position.z - 10;
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

    public void clickButton(int color)
    {
        if (started)
        {
            if (currentcolor == color)
            {
                //seconds -= 10;
                seccallback(seconds - 10);
                Destroy(currentPanneau);
                CmdgeneratePanneau();
                speedcallback(speed + 0.1f);
                //traininfo.add_speed(0.1f);
                //bonus = true;
                bonuscallback(true);
                bonustime = Time.time;
            }
            else
            {
                //seconds += 20;
                seccallback(seconds + 20);
                if (speed >= 0.05)
                {
                    speedcallback(speed - 0.05f);
                    //traininfo.add_speed(-0.05f);
                }
            }
        }
    }


    void generateParing()
    {
        if (isServer)
        {
            seedcallback(Random.seed);
            //started = true;
            startcallback(true);
            for (int i = 0; i < mat.Length; i++)
            {
                paring.Add(new Tuple<int, int>(i, Random.Range(0, color.Length)));
                generateCube(i, paring[i].Item2);
            }
            CmdgeneratePanneau();
            //traininfo.set_speed(0.11f);
            speedcallback(0.11f);
        }
    }

    [Mirror.Command]
    void CmdgeneratePanneau()
    {
        current = Random.Range(0, 20);
        curcallback(current);
        currentcolor = paring[current].Item2;
        curcolorcallback(currentcolor);
        Destroy(currentPanneau);
        currentPanneau = Instantiate(prefab, train.transform.position + new Vector3(2.8f, 0.5f, 50), Quaternion.identity);
        GameObject face = currentPanneau.transform.Find("sign/Front").gameObject;
        face.GetComponent<Renderer>().material = mat[current];
        Mirror.NetworkServer.Spawn(currentPanneau);
        newpanneau = true;
        panneaucallback(true);
    }

    void Start()
    {
        if (isServer)
            seedcallback(Random.seed);
        else
            seed = -1;
        train = GameObject.Find("Tram_joueur1");
        transform.parent = train.transform;
        paring = new List<Tuple<int, int>>();
        traininfo = (movetrain)train.GetComponent(typeof(movetrain));
        speedcallback(0);
        newpanneau = false;
        //traininfo.set_speed(0);

    }

    // Update is called once per frame
    void Update()
    {
        traininfo.set_speed(speed);
        //Debug.LogWarning(seed);
        if (started)
        {
            if (isClientOnly)
            {
                Debug.LogError("im the player2");
                if (paring.Count == 0)
                {
                    Debug.LogWarning("paring is empty");
                    Random.InitState(seed);
                    for (int i = 0; i < mat.Length; i++)
                    {
                        paring.Add(new Tuple<int, int>(i, Random.Range(0, color.Length)));
                    }
                    GameObject[] indices = GameObject.FindGameObjectsWithTag("indice");

                    for (int i = 0; i < 20; i++)
                    {
                        GameObject ind = indices[i];
                        int j = paring[i].Item2;
                        ind.transform.parent = train.transform;
                        ind.transform.Find("Front").gameObject.GetComponent<Renderer>().material = mat[i];
                        ind.transform.Find("Back").gameObject.GetComponent<Renderer>().material = color[j];
                        ind.transform.Find("Bottom").gameObject.GetComponent<Renderer>().material = color[j];
                        ind.transform.Find("Top").gameObject.GetComponent<Renderer>().material = color[j];
                        ind.transform.Find("Left").gameObject.GetComponent<Renderer>().material = color[j];
                        ind.transform.Find("Right").gameObject.GetComponent<Renderer>().material = color[j];
                        ind.transform.position = transform.position + new Vector3(0, 0, 10);
                    }
                    //Debug.LogWarning(paring.Count);
                    GameObject[] pan = GameObject.FindGameObjectsWithTag("panneau");
                    currentPanneau = pan[pan.Length - 1];
                    GameObject face = currentPanneau.transform.Find("sign/Front").gameObject;
                    face.GetComponent<Renderer>().material = mat[current];
                }
                else if (newpanneau == true)
                {
                    Debug.LogWarning("trueeee");
                    GameObject[] pan = GameObject.FindGameObjectsWithTag("panneau");
                    //Debug.LogWarning(pan.Length);
                    foreach (GameObject p in pan)
                    {
                        GameObject face = p.transform.Find("sign/Front").gameObject;
                        face.GetComponent<Renderer>().material = mat[current];
                    }
                    newpanneau = false;
                }
                else
                {
                    Debug.LogWarning("Nothing");
                }

            }
            if (isServer)
            {
                seconds += Time.deltaTime;
                seccallback(seconds);
                if (bonus && Time.time - bonustime > 2)
                    //bonus = false;
                    bonuscallback(false);
                if (!bonus)
                    if (speed > 0.02)
                        speed -= 0.0001f;
            }
            speedcallback(speed);
            m_text.text = "Time : " + ((int)seconds / 60).ToString() + ":" + ((int)seconds % 60).ToString("00");
            textspeed.text = "Speed : " + (1000 * speed).ToString("00");
            traininfo.set_speed(speed);
        }
        else
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            infoText.text = "Waiting for player 2";
            if (players.Length == 2)
            {
                generateParing();
                infoText.text = "";
            }

        }
    }
}
