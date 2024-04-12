using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour
{
    public float _cutTime = GameBalanceList.CUT_TIME;

    public bool _cut = false, _cutting = false;

    public Image _background;
    public Image _cutBar;
    public Sprite _cutSprite;

    public int _hash;


    // Update is called once per frame
    void Update()
    {
        if (_cutTime <= 0 && !_cut)
        {
            Cut();
        }

        if (_cutting && _cutTime < GameBalanceList.CUT_TIME && _cutTime > 0) //done cutting
        {
            if (!_background.gameObject.activeSelf)
            {
                _background.gameObject.SetActive(true);
            }
            if (!_cutBar.gameObject.activeSelf)
            {
                _cutBar.gameObject.SetActive(true);
            }

            _cutBar.rectTransform.localScale = new Vector2(1 + (-_cutTime / GameBalanceList.CUT_TIME), 1);
        }
        else
        {
            if (_background.gameObject.activeSelf)
            {
                _background.gameObject.SetActive(false);
            }
            if (_cutBar.gameObject.activeSelf)
            {
                _cutBar.gameObject.SetActive(false);
            }
        }
    }

    protected void Cut()
    {
        _cut = true;
        GetComponent<SpriteRenderer>().sprite = _cutSprite;
        //Debug.LogError("ITEM FINISHED CUTTING");
        if (PlayerCreatorScript._instance._networked)
        {
            var netLevelMan = FindObjectOfType<NetworkedLevelManager>();
            if (netLevelMan != null)
            {
                //Debug.LogError("sending cut CMD");
                netLevelMan.CmdCutItem(_hash);
            }
        }
    }
}
