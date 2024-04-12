using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillScript : MonoBehaviour
{
    CounterScript _counterScript;
    AudioList _audioList;
    // Start is called before the first frame update
    void Start()
    {
        _counterScript = GetComponent<CounterScript>();
        _audioList = GameObject.FindGameObjectWithTag(TagList.MANAGER).GetComponent<AudioList>();
    }


    void FixedUpdate()
    {
        if (_counterScript._item && _counterScript._item.CompareTag(TagList.BREAD))
        {
            var breadScript = _counterScript._item.GetComponent<BreadScript>();
            if(breadScript._cookTime > GameBalanceList.BURN_TIME)
                breadScript._cookTime--;
            _audioList.PlayToastingAudio();
        }
    }
}
