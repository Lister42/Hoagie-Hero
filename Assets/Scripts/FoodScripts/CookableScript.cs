using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookableScript : ItemScript
{
    public float _cookTime = GameBalanceList.COOK_TIME;

    public bool _cooked = false, _burnt = false, _cooking = false;

    public Image _cookBar;
    public Sprite _panCookedSprite;
    public Sprite _potCookedSprite;

    public string _cookware;

    LevelManagerScript _levelMgr;

    private void Start()
    {
        _levelMgr = GameObject.FindGameObjectWithTag(TagList.MANAGER).GetComponent<LevelManagerScript>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_cookware.Equals(TagList.PAN) && tag.Equals(TagList.TOMATO))
        {
            return;
        }
        if (_cutTime <= 0 && !_cut)
        {
            Cut();
        }
        if (_cookTime <= 0 && !_cooked)
        {
            _cooked = true;
            if(CompareTag(TagList.HAM))
                if (_cookware.Equals(TagList.PAN))
                {
                    GetComponent<SpriteRenderer>().sprite = _panCookedSprite;
                    tag = TagList.BACON;
                }
                else if (_cookware.Equals(TagList.POT))
                {
                    GetComponent<SpriteRenderer>().sprite = _potCookedSprite;
                    transform.position = new Vector2(transform.position.x, transform.position.y + .1f);
                    tag = TagList.MEATBALLS;
                }
            if(CompareTag(TagList.TOMATO))
                if (_cookware.Equals(TagList.POT))
                {
                    GetComponent<SpriteRenderer>().sprite = _potCookedSprite;
                    transform.position = new Vector2(transform.position.x, transform.position.y + .1f);
                    tag = TagList.TOMATO_SAUCE;
                }
            _cookBar.color = Color.red;
        }
        if (_cooked && _cookTime <= GameBalanceList.BURN_TIME)
        {
            _burnt = true;
            GetComponent<SpriteRenderer>().color = Color.black;
        }
        if (_cutting && _cutTime < GameBalanceList.CUT_TIME && _cutTime > 0)
        {
            if (!_background.gameObject.activeSelf)
            {
                _background.gameObject.SetActive(true);
            }
            if (!_cutBar.gameObject.activeSelf)
            {
                _cutBar.gameObject.SetActive(true);
            }
            if (_cookBar.gameObject.activeSelf)
            {
                _cookBar.gameObject.SetActive(false);
            }
            _cutBar.rectTransform.localScale = new Vector2(1 + (-_cutTime / GameBalanceList.CUT_TIME), 1);
        }
        else if (_cooking && _cookTime < GameBalanceList.COOK_TIME && _cookTime > 0)
        {
            if (!_background.gameObject.activeSelf)
            {
                _background.gameObject.SetActive(true);
            }
            if (!_cookBar.gameObject.activeSelf)
            {
                _cookBar.gameObject.SetActive(true);
            }
            if (_cutBar.gameObject.activeSelf)
            {
                _cutBar.gameObject.SetActive(false);
            }
            _cookBar.rectTransform.localScale = new Vector2(1 + (-_cookTime / GameBalanceList.COOK_TIME), 1);
        }
        else if (_cooking && _cooked && _cookTime < GameBalanceList.COOK_TIME && _cookTime > GameBalanceList.BURN_TIME)
        {
            if (!_background.gameObject.activeSelf)
            {
                _background.gameObject.SetActive(true);
            }
            if (!_cookBar.gameObject.activeSelf)
            {
                _cookBar.gameObject.SetActive(true);
            }
            if (_cutBar.gameObject.activeSelf)
            {
                _cutBar.gameObject.SetActive(false);
            }
            _cookBar.rectTransform.localScale = new Vector2(_cookTime / GameBalanceList.BURN_TIME, 1);
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
            if (_cookBar.gameObject.activeSelf)
            {
                _cookBar.gameObject.SetActive(false);
            }
        }
    }
}
