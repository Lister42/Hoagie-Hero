using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreadScript : MonoBehaviour
{
    SpriteRenderer _srend;
    Color _dark = new(.7f, .7f, .7f);

    public float _cookTime = GameBalanceList.COOK_TIME;
    private List<string> _ingredients;
    private GameObject _oneIngredientIcon;
    private GameObject _twoIngredientsIcon;
    private GameObject _threeIngredientsIcon;

    public Image _cookBar;
    public Image _background;

    public Sprite L;
    public Sprite T;
    public Sprite C;
    public Sprite LT;
    public Sprite LC;
    public Sprite TC;
    public Sprite TLC;
    public Sprite H;
    public Sprite HL;
    public Sprite HT;
    public Sprite HC;
    public Sprite HLT;
    public Sprite HLC;
    public Sprite HTC;
    public Sprite HLTC;
    public Sprite B;
    public Sprite BL;
    public Sprite BT;
    public Sprite BC;
    public Sprite BLT;
    public Sprite BLC;
    public Sprite BTC;
    public Sprite BLTC;
    public Sprite MH;


    public bool ham, bacon, lettuce, tomato, cheese, meatballs, tomato_sauce, grilled, burnt, _cooking;
    private void Start()
    {
        _srend = GetComponent<SpriteRenderer>();
        _ingredients = new List<string>();
        _oneIngredientIcon = transform.GetChild(1).gameObject;
        _twoIngredientsIcon = transform.GetChild(2).gameObject;
        _threeIngredientsIcon = transform.GetChild(3).gameObject;
    }

    private void Update()
    {
        if (_cookTime <= 0 && !grilled)
        {
            grilled = true;
            _srend.color = _dark;
            _cookBar.color = Color.red;
        }
        if(grilled && _cookTime <= GameBalanceList.BURN_TIME)
        {
            burnt = true;
            _srend.color = Color.black;
        }

        if (_cooking && _cookTime < GameBalanceList.COOK_TIME && _cookTime > 0)
        {
            if (!_background.gameObject.activeSelf)
            {
                _background.gameObject.SetActive(true);
            }
            if (!_cookBar.gameObject.activeSelf)
            {
                _cookBar.gameObject.SetActive(true);
            }
            _cookBar.rectTransform.localScale = new Vector2(1 + (-_cookTime / GameBalanceList.COOK_TIME), 1);
        }
        else if (_cooking && _cookTime < GameBalanceList.COOK_TIME && _cookTime > GameBalanceList.BURN_TIME)
        {
            if (!_background.gameObject.activeSelf)
            {
                _background.gameObject.SetActive(true);
            }
            if (!_cookBar.gameObject.activeSelf)
            {
                _cookBar.gameObject.SetActive(true);
            }
            _cookBar.rectTransform.localScale = new Vector2(_cookTime / GameBalanceList.BURN_TIME, 1);
        }
        else
        {
            if (_background.gameObject.activeSelf)
            {
                _background.gameObject.SetActive(false);
            }
            if (_cookBar.gameObject.activeSelf)
            {
                _cookBar.gameObject.SetActive(false);
            }
        }
    }

    public void AddIngredient(GameObject g)
    {
        var tag = g.tag;
        switch (tag)
        {
            case TagList.HAM:
                ham = true;
                break;
            case TagList.BACON:
                bacon = true;
                break;
            case TagList.LETTUCE:
                lettuce = true;
                break;
            case TagList.TOMATO:
                tomato = true;
                break;
            case TagList.CHEESE:
                cheese = true;
                break;
            case TagList.MEATBALLS:
                meatballs = true;
                break;
            case TagList.TOMATO_SAUCE:
                tomato_sauce = true;
                break;

        }
        _ingredients.Add(g.transform.tag);
        UpdateSprite();
        UpdateIcons();
    }

    void UpdateSprite()
    {
        if (ham)
        {
            Set(H);
            if (cheese)
            {
                Set(HC);
                if (lettuce)
                {
                    Set(HLC);
                    if (tomato)
                    {
                        Set(HLTC);
                    }
                }
                else if (tomato)
                {
                    Set(HTC);
                }
            }
            else if (tomato)
            {
                Set(HT);
                if (lettuce)
                {
                    Set(HLT);
                }
            }
            else if (lettuce)
            {
                Set(HL);
            }
        }
        else if (bacon)
        {
            Set(B);
            if (cheese)
            {
                Set(BC);
                if (lettuce)
                {
                    Set(BLC);
                    if (tomato)
                    {
                        Set(BLTC);
                    }
                }
                else if (tomato)
                {
                    Set(BTC);
                }
            }
            else if (tomato)
            {
                Set(BT);
                if (lettuce)
                {
                    Set(BLT);
                }
            }
            else if (lettuce)
            {
                Set(BL);
            }
        }
        else if (cheese)
        {
            Set(C);
            if (lettuce)
            {
                Set(LC);
                if (tomato)
                {
                    Set(TLC);
                }
            }
            else if (tomato)
            {
                Set(TC);
            }
        }
        else if (lettuce)
        {
            Set(L);
            if (tomato)
            {
                Set(LT);
            }
        }
        else if (tomato) Set(T);

        if (meatballs && tomato_sauce)
        {
            Set(MH);
        }
    }


    void Set(Sprite s)
    {
        _srend.sprite = s;
    }

    private void UpdateIcons()
    {
        if (_ingredients.Count == 0)
        {
            return;
        }

        else if (_ingredients.Count == 1)
        {
            _oneIngredientIcon.SetActive(true);
            _oneIngredientIcon.transform.GetChild(0).GetComponent<SpriteRenderer>().color = GetIngredientColor(_ingredients[0]);
        }
        else if (_ingredients.Count == 2)
        {
            _oneIngredientIcon.SetActive(false);
            _twoIngredientsIcon.SetActive(true);
            _twoIngredientsIcon.transform.GetChild(0).GetComponent<SpriteRenderer>().color = GetIngredientColor(_ingredients[0]);
            _twoIngredientsIcon.transform.GetChild(1).GetComponent<SpriteRenderer>().color = GetIngredientColor(_ingredients[1]);
        }
        else if (_ingredients.Count == 3)
        {
            _twoIngredientsIcon.SetActive(false);
            _threeIngredientsIcon.SetActive(true);
            _threeIngredientsIcon.transform.GetChild(0).GetComponent<SpriteRenderer>().color = GetIngredientColor(_ingredients[0]);
            _threeIngredientsIcon.transform.GetChild(1).GetComponent<SpriteRenderer>().color = GetIngredientColor(_ingredients[1]);
            _threeIngredientsIcon.transform.GetChild(2).GetComponent<SpriteRenderer>().color = GetIngredientColor(_ingredients[2]);



        }
    }

    public Color GetIngredientColor(string g)
    {
        switch (g)
        {
            case TagList.HAM:
                return new Color(1, 138 / 255f, 166 / 255f, 1); //Pink
            case TagList.CHEESE:
                return new Color(1, 225 / 255f, 0, 1); //Yellow
            case TagList.LETTUCE:
                return new Color(49 / 255f, 170 / 255f, 41 / 255f, 1); //Green
            case TagList.TOMATO:
                return new Color(242 / 255f, 34 / 255f, 40 / 255f, 1); //Red
            case TagList.BACON:
                return new Color(176 / 255f, 104 / 255f, 84 / 255f, 1); //Brown
            case TagList.MEATBALLS:
                return new Color(176 / 255f, 104 / 255f, 84 / 255f, 1); //Brown
            case TagList.TOMATO_SAUCE:
                return new Color(247 / 255f, 72 / 255f, 34 / 255, 1); //Red-Orange
            default:
                return new Color(1, 1, 1, 1);
        }
    }
}
