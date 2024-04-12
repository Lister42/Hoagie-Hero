using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetManScript : NetworkManager
{
    public GameObject[] spawnPoints;
    public int _connectedPlayers;


    public int[] _playerIDs = { -1, -1, -1, -1 };

    public override void OnStartServer()
    {
        base.OnStartServer();
        print("server connected");
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        print("client Connected");
    }



    public override void OnStopClient()
    {
        base.OnStopClient();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        StopHost();
    }
}