using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingScript : MonoBehaviour
{
    CounterScript _counterScript;
    LevelManagerScript _managerScript;

    // Start is called before the first frame update
    void Start()
    {
        _counterScript = GetComponent<CounterScript>();
        _managerScript = GameObject.FindGameObjectWithTag(TagList.MANAGER).GetComponent<LevelManagerScript>();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _managerScript._chefsSelected.Length; i++)
        {
            if (_managerScript._chefsSelected[i] == gameObject && _counterScript._item)
            {
                if (_counterScript._item.CompareTag(TagList.BREAD)) return;


                if ((_counterScript._itemScript && _counterScript._itemScript._cutTime >= 0) || (_counterScript._cookScript && _counterScript._cookScript._cutTime >= 0))
                {
                    if (_managerScript._chefsSelected[i] == gameObject && _counterScript._item) //Chef 1 Chopping Animation
                    {
                        if ((_counterScript._itemScript && _counterScript._itemScript._cut) || (_counterScript._cookScript && _counterScript._cookScript._cut))
                        {
                            _counterScript.chefs[i].GetChefMovementScript().IsNotChoppingAnim();
                        }
                        else
                        {
                            _counterScript.chefs[i].GetChefMovementScript().IsChoppingAnim();
                        }
                    }

                    //if (_managerScript._chefTwoSelected == gameObject && _counterScript._item) //Chef 2 Chopping Animation
                    //{
                    //    if ((_counterScript._itemScript && _counterScript._itemScript._cut) || (_counterScript._cookScript && _counterScript._cookScript._cut))
                    //    {
                    //        _counterScript.chefs[1].GetChefMovementScript().IsNotChoppingAnim();
                    //    }
                    //    else
                    //    {
                    //        _counterScript.chefs[1].GetChefMovementScript().IsChoppingAnim();
                    //    }

                    //}
                    if (_counterScript._itemScript)
                    {
                        _counterScript._itemScript._cutTime--;
                    }
                    else if (_counterScript._cookScript)
                    {
                        _counterScript._cookScript._cutTime--;
                    }
                }
            }
        }
    }
}
