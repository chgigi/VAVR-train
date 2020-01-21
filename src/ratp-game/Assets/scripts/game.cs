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
    public Text m_text;
    public Text textspeed;
    [SerializeField] public Text infoText;
    private GameObject train;
    private movetrain traininfo;
    public bool player1;
    private game other;
    [Mirror.SyncVar(hook = "OnActiveChange")]
    public bool hasstarted = false;
    private List<Tuple<int, int>> paring;
    [Mirror.SyncVar(hook = "OnActiveChangeSeed")]
    public int seed;
    [Mirror.SyncVar(hook = "OnActiveChangeCurrent")]
    public int current;
    [Mirror.SyncVar(hook = "OnActiveChangeCurrentColor")]
    public int currentColor;
    private GameObject currentPanneau;
    [Mirror.SyncVar(hook = "OnActiveChangePanneau")]
    public bool newpanneau = false;
    [Mirror.SyncVar(hook = "OnActiveChangeString")]
    public string rd;
    private List<GameObject> players;
    [Mirror.SyncVar(hook = "OnActiveChangeSpeed")]
    public float speed;
    private float seconds = 0f;
    [Mirror.SyncVar(hook = "OnActiveChangeTime")]
    public string tm;
    private float bonusTime;

    [Mirror.ClientRpc]
    public void RpcupdateSpeed(float s)
    {
        speed = s;
        Debug.LogWarning("Speed = " + speed.ToString());
    }


    public void clickButton(int color)
    {
        if (hasstarted)
        {
            Cmdinvertpan();
            if (currentColor == color)
            {
                //Destroy(currentPanneau);
                CmdgeneratePanneau();
                Cmdchangespeed(speed + 0.1f);
                seconds -= 10f;
                CmdchangeTime();
                bonusTime = Time.time;

            }
            else
            {
                seconds += 20f;
                CmdchangeTime();
                if (speed >= 0.05)
                {
                    Cmdchangespeed(speed - 0.05f);
                }
            }
        }
    }

    private void OnActiveChangeTime(string t)
    {
        tm = t;
    }
    private void OnActiveChangeSpeed(float s)
    {
        speed = s;
        Debug.LogWarning("hook speed on client!");
    }
    private void OnActiveChangeString(String s)
    {
        rd = s;
    }

    private void OnActiveChangeCurrent(int c)
    {
        current = c;
    }

    private void OnActiveChangeCurrentColor(int c)
    {
        currentColor = c;
    }
    private void OnActiveChangeSeed(int updatedActive)
    {
        seed = updatedActive;
    }

    private void OnActiveChange(bool updatedActive)
    {
        hasstarted = updatedActive;
    }

    private void OnActiveChangePanneau(bool p)
    {
        newpanneau = p;
        Debug.LogWarning("update pannal state");

    }

    [Mirror.Command]
    private void CmdDisable()
    {
        
    }

    [Mirror.Command]
    void CmdgenerateParing()
    {
        string s = "";
        seed = Random.seed;
        for (int i = 0; i < mat.Length; i++)
        {
            paring.Add(new Tuple<int, int>(i, Random.Range(0, color.Length)));
            s += paring[i].Item2.ToString() + "-";
        }
        rd = s;
    }

    private void regenerateParing()
    {
        seed = other.seed;
        rd = other.rd;
        string[] st = rd.Split('-');
        for (int i = 0; i < 20; i++)
        {
            paring.Add(new Tuple<int, int>(i, Int32.Parse(st[i])));
        }
    }

    [Mirror.Command]
    void Cmdinvertpan()
    {
        newpanneau = false;
    }

    [Mirror.Command]
    void CmdchangeTime()
    {
        int mn = (int)seconds / 60;
        int sec = (int)seconds % 60;
        tm = "Time : " + mn.ToString() + ":" + sec.ToString("00"); 
    }

    [Mirror.Command]
    void CmdgeneratePanneau()
    {
        if (currentPanneau != null)
            Destroy(currentPanneau);
        current = Random.Range(0, 20);
        currentColor = paring[current].Item2;
        currentPanneau = Instantiate(prefab, train.transform.position + new Vector3(2.8f, 0.5f, 50), Quaternion.identity);
        GameObject face = currentPanneau.transform.Find("sign/Front").gameObject;
        face.GetComponent<Renderer>().material = mat[current];
        Mirror.NetworkServer.Spawn(currentPanneau);
        currentPanneau = GameObject.FindGameObjectWithTag("panneau");
        newpanneau = true;
    }

    void regeneratePanneau()
    {
        other.newpanneau = false;
        if(currentPanneau != null)
            Destroy(currentPanneau);
        current = other.current;
        currentColor = other.currentColor;
        currentPanneau = GameObject.FindGameObjectWithTag("panneau");
        GameObject face = currentPanneau.transform.Find("sign/Front").gameObject;
        face.GetComponent<Renderer>().material = mat[current];
    }

    [Mirror.Command]
    void CmdgenerateCube()
    {
        float x, y, z;
        x = train.transform.position.x;
        z = train.transform.position.z - 10;
        y = 2.5f;
        for (int i = 0; i < 20; i++)
        {
            int j = paring[i].Item2;
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
        tm = "Time: 0:00";
        speed = 0.11f;
        hasstarted = true;
    }

    void regenerateCube()
    {
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
        hasstarted = true;
    }

    void Start()
    {
        bonusTime = Time.time;
        paring = new List<Tuple<int, int>>();
        players = new List<GameObject>();
        if (!isClientOnly)
        {
            player1 = true;
            players.Add(this.gameObject);
        }
        else
        {
            player1 = false;
            GameObject[] p = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < 2; i++)
            {
                if (p[i] != this.gameObject)
                    players.Add(p[i]);
            }
            players.Add(this.gameObject);
            other = ((game)players[0].GetComponent(typeof(game)));
        }

        train = GameObject.Find("Tram_joueur1");
        transform.parent = train.transform;
        traininfo = (movetrain)train.GetComponent(typeof(movetrain));
        traininfo.speed = 0f;

    }

    [Mirror.Command]
    void Cmdchangespeed(float newspeed)
    {
        speed = newspeed;
        Debug.LogWarning("Speed changed on client!");
        //RpcupdateSpeed(newspeed);
    }

    void checkpan()
    {
        newpanneau = other.newpanneau;
        if (newpanneau)
        {
            Debug.LogWarning("regeneraaate panneau");
            regeneratePanneau();
        }
    }

    void updateSpeedTM()
    {
        speed = other.speed;
        tm = other.tm;
        traininfo.speed = speed;
        m_text.text = tm;
        textspeed.text = "Speed : " + (1000 * speed).ToString("00");
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;
        if(hasstarted)
        {
            if (isServer)
            {
                seconds = seconds + Time.deltaTime;
                CmdchangeTime();
                if (Time.time - bonusTime <= 2)
                {
                    Debug.LogWarning("la c'est le bonus");
                }
                else
                {
                    if (speed > 0.02)
                        Cmdchangespeed(speed - 0.0001f);
                }
                traininfo.speed = speed;
                m_text.text = tm;
                textspeed.text = "Speed : " + (1000 * speed).ToString("00");
            }
            else if(isClientOnly)
            {
                Debug.LogWarning("on passe ici");
                checkpan();
                updateSpeedTM();
            }
            
        }
        else
        {
            infoText.text = "Waiting for player 2";
            GameObject[] p = GameObject.FindGameObjectsWithTag("Player");
            if (p.Length == 2)
            {
                
                if (isServer)
                {
                    players.Add(p[1]);
                    other = ((game)players[1].GetComponent(typeof(game)));
                    CmdgenerateParing();
                    CmdgenerateCube();
                    CmdgeneratePanneau();
                    infoText.text = "";
                }
                else if(isClientOnly)
                {
                    if(other.hasstarted)
                    {
                        regenerateParing();
                        regenerateCube();
                        regeneratePanneau();
                        speed = other.speed;
                        tm = other.tm;
                        infoText.text = "";
                    }
                }
            }
        }
    }











}