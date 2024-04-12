using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class NetPlayerConnectScript : NetworkBehaviour
{

    public Text _titleText;
    public NetManScript _networkManager;
    
    int _playersConnected;
    [SyncVar]
    int _numPlayers;
    int _level;
    // Start is called before the first frame update
    void Start()
    {
        _networkManager = GameObject.FindGameObjectWithTag(TagList.NETWORK_MANAGER).GetComponent<NetManScript>();
        _level = PlayerPrefs.GetInt(TagList.LEVEL, 1);
        if (isClientOnly)
        {
            print("Ran is client only");
            
            CmdJoinLobby();

        }
        if (isServer)
        {
            var ip = GetLocalIPAddress();
            print("Host: " + ip);
            print("tt " + _titleText.text);
            _titleText.text = "Host IP - " + ip;
            print("now " + _titleText.text);
            print("ran is server");
            _playersConnected = 1;
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        _numPlayers = PlayerPrefs.GetInt(TagList.NUM_PLAYERS, 2);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        PlayerPrefs.SetInt(TagList.NUM_PLAYERS, _numPlayers);
    }


    [Command (requiresAuthority = false)]
    private void CmdJoinLobby()
    {
        print("called the commane");
        _playersConnected++;
        if (_playersConnected == _numPlayers)
        {
            _titleText.text = "Starting Game";

            RpcSetText();
            
            StartGame();
        }
    }

    [ClientRpc]
    private void RpcSetText()
    {
        _titleText.text = "Starting Game";
    }

    private void StartGame()
    {
        print("time to start game");
        var scene = "";
        switch (_level)
        {
            case 1:
                switch (_numPlayers)
                {
                    case 2:
                        scene = TagList.LEVEL_1_2P;
                        break;
                    case 3:
                        scene = TagList.LEVEL_1_3P;
                        break;
                    case 4:
                        scene = TagList.LEVEL_1_4P;
                        break;
                    default:
                        scene = TagList.LEVEL_1_2P;
                        break;
                }
                break;
            case 2:
                switch (_numPlayers)
                {
                    case 2:
                        scene = TagList.LEVEL_2_2P;
                        break;
                    case 3:
                        scene = TagList.LEVEL_2_3P;
                        break;
                    case 4:
                        scene = TagList.LEVEL_2_4P;
                        break;
                    default:
                        scene = TagList.LEVEL_2_2P;
                        break;
                }
                break;
            case 3:
                switch (_numPlayers)
                {
                    case 2:
                        scene = TagList.LEVEL_3_2P;
                        break;
                    case 3:
                        scene = TagList.LEVEL_3_3P;
                        break;
                    case 4:
                        scene = TagList.LEVEL_3_4P;
                        break;
                    default:
                        scene = TagList.LEVEL_3_2P;
                        break;
                }
                break;
            default:
                break;
        }
        print("loading " + scene);
        NetworkManager.singleton.ServerChangeScene(scene);
        print(Time.time);
    }

    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
}
