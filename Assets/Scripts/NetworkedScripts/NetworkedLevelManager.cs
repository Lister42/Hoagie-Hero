using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;

public class NetworkedLevelManager : NetworkBehaviour
{
    [SyncVar]
    double _serverStartTime;

    [SyncVar]
    public int _itemCount;

    
    readonly SyncList<int> _orderNumbers = new SyncList<int>();

    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer() {
        _serverStartTime = NetworkTime.time;
        FindObjectOfType<LevelManagerScript>()._startTime = _serverStartTime;
        _itemCount = 0;
        var list = new int[] {0, 1, 2, 3, 4, 5, 6 }.OrderBy(item => Random.value).ToArray();
        foreach (var item in list)
        {
            _orderNumbers.Add(item);
        }
    }

    /// <summary>
    /// Called on every NetworkBehaviour when it is activated on a client.
    /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
    /// </summary>
    public override void OnStartClient()
    {
        FindObjectOfType<LevelManagerScript>()._startTime = _serverStartTime;
        OrderSync();
    }

    //public override void OnStartLocalPlayer() {
    //    FindObjectOfType<LevelManagerScript>()._startTime = _serverStartTime;
    //}

    void OrderSync()
    {
        print("please");
        var man = FindObjectOfType<OrderManagerScript>();

        man.CreateOrders();

        var orders = man._orders;

        man._orders = new List<Order>(); 

        for (int i = 0; i < orders.Count; i++)
        {
            man._orders.Add(orders[_orderNumbers[i]]);
        }

        man._order1.SetOrder(man._orders[0]);
        man._order2.SetOrder(man._orders[1]);
        man._order3.SetOrder(man._orders[2]);
        man._orders.RemoveRange(0, 3);
    }

    [Command (requiresAuthority = false)]
    public void CmdCutItem(int hash)
    {
        RpcCutItem(hash);
    }

    [ClientRpc]
    void RpcCutItem(int hash)
    {
        if (isLocalPlayer) return;

        var items = FindObjectsOfType<ItemScript>();

        foreach (var item in items)
        {
            if(item._hash == hash)
            {
                item._cutTime = -1;
            }
        }
    }
}
