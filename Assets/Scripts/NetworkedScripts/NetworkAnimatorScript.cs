using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkAnimatorScript : NetworkBehaviour
{
    public readonly SyncList<GameObject> chefs = new SyncList<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int AddChefToAnimator(GameObject chef)
    {
        chefs.Add(chef);
        print("Added chef to network animator script " + chefs[chefs.Count - 1]);
        return chefs.Count - 1;
    }
}
