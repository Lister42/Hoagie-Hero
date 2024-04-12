using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorScript : MonoBehaviour
{
    private bool _isColliding;
    
    // Start is called before the first frame update
    void Start()
    {
        _isColliding = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        _isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isColliding = false;
    }


    public bool IsColliding()
    {
        return _isColliding;
    }
}
