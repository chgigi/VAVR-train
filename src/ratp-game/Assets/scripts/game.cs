using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class game : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab;
    public GameObject indice;
    public Material[] mat;
    public Material[] color;
    private GameObject currentPanneau;
    private List<Tuple<int, int>> paring;
    private int current;
    private float seconds;
    public Text m_text;
    public Text textspeed;
    public GameObject train;
    private movetrain traininfo;
    private bool bonus = false;
    private float bonustime;


    void generateCube(int i, int j)
    {
        float x, y, z;
        x = train.transform.position.x;
        z = train.transform.position.z - 15;
        y = 2.5f;
        GameObject c = Instantiate(indice, new Vector3(x, y, z), Quaternion.identity);
        c.transform.parent = train.transform;
        c.transform.Find("Front").gameObject.GetComponent<Renderer>().material = mat[i];
        c.transform.Find("Back").gameObject.GetComponent<Renderer>().material = color[j];
        c.transform.Find("Bottom").gameObject.GetComponent<Renderer>().material = color[j];
        c.transform.Find("Top").gameObject.GetComponent<Renderer>().material = color[j];
        c.transform.Find("Left").gameObject.GetComponent<Renderer>().material = color[j];
        c.transform.Find("Right").gameObject.GetComponent<Renderer>().material = color[j];
    }

    public void clickButton(int color)
    {
        if(paring[current].Item2 == color)
        {
            seconds -= 10;
            Destroy(currentPanneau);
            generatePanneau(Random.Range(0, mat.Length));
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

    void generatePanneau(int i)
    {
        Destroy(currentPanneau);
       currentPanneau = Instantiate(prefab, train.transform.position + new Vector3(2.8f, 0.5f, 50), Quaternion.identity);
        GameObject face = currentPanneau.transform.Find("sign/Front").gameObject;
        current = i;
        face.GetComponent<Renderer>().material = mat[i];
    }

    void Start()
    {
        paring = new List<Tuple<int, int>>();
        traininfo = (movetrain)train.GetComponent(typeof(movetrain));
        for (int i = 0; i < mat.Length; i++)
        {
            paring.Add(new Tuple<int, int>(i, Random.Range(0, color.Length)));
            generateCube(i, paring[i].Item2);
        }
       
        currentPanneau = Instantiate(prefab, train.transform.position + new Vector3(2.8f, 0.5f, 50), Quaternion.identity);
        GameObject face = currentPanneau.transform.Find("sign/Front").gameObject;
        current = Random.Range(0, 20);
        face.GetComponent<Renderer>().material = mat[current];
    }

    // Update is called once per frame
    void Update()
    {
        seconds += Time.deltaTime;
        m_text.text = "Time : " + ((int)seconds/60).ToString() + ":" + ((int)seconds%60).ToString("00");
        textspeed.text = "Speed : " + (1000 * traininfo.get_speed()).ToString("00");
        if (bonus && Time.time - bonustime > 2)
            bonus = false;
        if(!bonus)
            if(traininfo.get_speed() > 0.02)
                traininfo.add_speed(-0.0001f);
    }
}
