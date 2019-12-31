using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject NetworkObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickJoin()
    {
        Mirror.NetworkManager NetManage = NetworkObject.GetComponent<Mirror.NetworkManager>();
        NetManage.StartClient();
        Debug.Log(NetManage.clientLoadedScene);
    }

    public void ClickHost()
    {
        Mirror.NetworkManager NetManage = NetworkObject.GetComponent<Mirror.NetworkManager>();
        NetManage.StartHost();
    }
}
