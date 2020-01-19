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


    void generateCube(int i, int j)
    {
        float x, y, z;
        x = Random.Range(-3, 3);
        z = Random.Range(-3, 3);
        y = 0.5f;
        GameObject c = Instantiate(indice, new Vector3(x, y, z), Quaternion.identity);
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
        }
        else
        {
            seconds += 20;
        }
    }

    void generatePanneau(int i)
    {
        currentPanneau = Instantiate(prefab, new Vector3(0, 0.5f, 0), Quaternion.identity);
        GameObject face = currentPanneau.transform.Find("sign/Front").gameObject;
        current = i;
        face.GetComponent<Renderer>().material = mat[i];
    }

    void Start()
    {
        paring = new List<Tuple<int, int>>();

        for(int i = 0; i < mat.Length; i++)
        {
            paring.Add(new Tuple<int, int>(i, Random.Range(0, color.Length)));
            generateCube(i, paring[i].Item2);
        }
        currentPanneau = Instantiate(prefab, new Vector3(0, 0.5f, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Destroy(currentPanneau);
            currentPanneau = Instantiate(prefab, new Vector3(0, 0.5f, 0), Quaternion.identity);
            GameObject face = currentPanneau.transform.Find("sign/Front").gameObject;
            current = Random.Range(0, 20);
            face.GetComponent<Renderer>().material = mat[current];
        }
        seconds += Time.deltaTime;
        m_text.text = "Time : " + ((int)seconds/60).ToString() + ":" + ((int)seconds%60).ToString("00");
    }
}
