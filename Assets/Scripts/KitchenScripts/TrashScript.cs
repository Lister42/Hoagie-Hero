using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashScript : MonoBehaviour
{
    CounterScript _counterScript;

    // Start is called before the first frame update
    void Start()
    {
        _counterScript = GetComponent<CounterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_counterScript._item && !_counterScript._item.CompareTag(TagList.PAN) && !_counterScript._item.CompareTag(TagList.POT)) Destroy(_counterScript._item);
    }
}
