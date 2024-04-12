using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;
using UnityEngine.UI;

public class MyDiscoveryHUD : MonoBehaviour
{
    ServerResponse _info;

    public NetworkDiscovery networkDiscovery;



    public GameObject _hostButton;
    public GameObject _findButton;
    public GameObject _joinButton;

    public TMPro.TextMeshProUGUI _ip;

    public GameObject _canvas;

    public void HostServer()
    {
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();
    }

    public void FindServer()
    {
        networkDiscovery.StartDiscovery();
    }

    public void JoinServer()
    {
        Connect(_info);
    }

    void Connect(ServerResponse info)
    {
        networkDiscovery.StopDiscovery();
        NetworkManager.singleton.StartClient(info.uri);
    }

    public void OnDiscoveredServer(ServerResponse info)
    {
        // Note that you can check the versioning to decide if you can connect to the server or not using this method
        _info = info;
        _joinButton.GetComponent<Button>().interactable = true;
        _ip.text = info.EndPoint.Address.ToString();
        Debug.LogError(info.EndPoint.Address.ToString());
    }    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
