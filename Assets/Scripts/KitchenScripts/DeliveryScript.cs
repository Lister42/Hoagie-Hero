using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryScript : MonoBehaviour
{
    CounterScript _counterScript;
    OrderManagerScript _orderManagerScript;
    LevelManagerScript _levelManagerScript;
    AudioList _audioList;

    // Start is called before the first frame update
    void Start()
    {
        _counterScript = GetComponent<CounterScript>();
        _orderManagerScript = GameObject.FindGameObjectWithTag(TagList.MANAGER).GetComponent<OrderManagerScript>();
        _levelManagerScript = GameObject.FindGameObjectWithTag(TagList.MANAGER).GetComponent<LevelManagerScript>();
        _audioList = GameObject.FindGameObjectWithTag(TagList.MANAGER).GetComponent<AudioList>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_counterScript._item && _counterScript._item.CompareTag(TagList.BREAD))
        {
            var breadScript = _counterScript._item.GetComponent<BreadScript>();

            if (_orderManagerScript.CorrectOrder(breadScript))
            {
                _audioList.PlayCorrectOrderAudio();
                Destroy(_counterScript._item);
                _counterScript._item = null;
            }
            else
            {
                _audioList.PlayWrongOrderAudio();
                Destroy(_counterScript._item);
                _counterScript._item = null;
                _levelManagerScript._startTime -= GameBalanceList.INCORRECT_ORDER_PENALTY;
                StartCoroutine(_levelManagerScript.TimingPenalty());
            }
        }
    }
}
