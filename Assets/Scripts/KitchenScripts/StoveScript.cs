using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveScript : MonoBehaviour
{
    CounterScript _counterScript;
    public GameObject _cookwareItem; //Pan or pot
    public GameObject _foodItem; //Food in pan or pot
    CookableScript _foodCookScript;
    public Chef _curChef;
    private AudioList _audioList;

    // Start is called before the first frame update
    void Start()
    {
        _counterScript = GetComponent<CounterScript>();
        _audioList = GameObject.FindGameObjectWithTag(TagList.MANAGER).GetComponent<AudioList>();

        _cookwareItem = transform.GetChild(0).gameObject;
        _foodItem = null;

        //Checks if they have these components
    }

    
    void FixedUpdate()
    {

        if (_foodItem && (_foodItem.CompareTag(TagList.HAM) || _foodItem.CompareTag(TagList.BACON) || _foodItem.CompareTag(TagList.MEATBALLS) || _foodItem.CompareTag(TagList.TOMATO) || _foodItem.CompareTag(TagList.TOMATO_SAUCE)) && _foodCookScript._cut)
        {
            if (_foodCookScript._cookTime > GameBalanceList.BURN_TIME)
            {
                _foodCookScript._cookTime--;
                _audioList.PlayCookingAudio();
            }
        }
    }

    public void Pickup(int player)
    {
        _curChef = _counterScript.chefs[player]; //Update Chef to current chef based on ChefSwitchManager. Since everything is based off the current chef, it auto updates.


        if (_curChef.GetChefMovementScript()._item && !_cookwareItem && (_curChef.GetChefMovementScript()._item.CompareTag(TagList.PAN) || _curChef.GetChefMovementScript()._item.CompareTag(TagList.POT))) //player puts down pan/pot item
        {
            _audioList.PlayPlaceAudio();
            _cookwareItem = _curChef.GetChefMovementScript()._item; //sets counter item to chef's held item (which is a pot or pan)
            if (_curChef.GetChefMovementScript()._item.transform.childCount != 0) //the pan that is being placed down has food on it
            {
                _foodItem = _curChef.GetChefMovementScript()._item.transform.GetChild(0).gameObject;
                _curChef.GetChefMovementScript()._item = null; //will only set down if the held item is a pan so this should always run
                _cookwareItem.transform.SetParent(transform);
                _cookwareItem.GetComponent<SpriteRenderer>().sortingOrder = 0;
                _foodItem.GetComponent<SpriteRenderer>().sortingOrder = 1;
                _cookwareItem.transform.position = transform.position;
                if (_cookwareItem.CompareTag(TagList.POT)) _cookwareItem.transform.position = new Vector2(_cookwareItem.transform.position.x, _cookwareItem.transform.position.y + 0.1f);
                if (_cookwareItem.CompareTag(TagList.PAN)) _cookwareItem.transform.position = new Vector2(_cookwareItem.transform.position.x + 0.07f, _cookwareItem.transform.position.y - 0.09f);
                _foodCookScript = _foodItem.GetComponent<CookableScript>();
                if (_foodCookScript)
                    _foodCookScript._cooking = true;
            }
            else //the pan that is being placed down does not have food on it
            {
                _foodItem = null;
                _curChef.GetChefMovementScript()._item = null;
                _cookwareItem.transform.SetParent(transform);
                _cookwareItem.GetComponent<SpriteRenderer>().sortingOrder = 0;
                _cookwareItem.transform.position = transform.position;
                if (_cookwareItem.CompareTag(TagList.POT)) _cookwareItem.transform.position = new Vector2(_cookwareItem.transform.position.x, _cookwareItem.transform.position.y + 0.1f);
                if (_cookwareItem.CompareTag(TagList.PAN)) _cookwareItem.transform.position = new Vector2(_cookwareItem.transform.position.x + 0.07f, _cookwareItem.transform.position.y - 0.09f);
            }
        }
        else if (_curChef.GetChefMovementScript()._item && _cookwareItem && !_foodItem && !_curChef.GetChefMovementScript()._item.CompareTag(TagList.PAN) && !_curChef.GetChefMovementScript()._item.CompareTag(TagList.POT) && _curChef.GetChefMovementScript()._item.GetComponent<CookableScript>()) //player puts down item on pan/pot already sitting on the stove
        {
            _audioList.PlayPlaceAudio();
            _foodItem = _curChef.GetChefMovementScript()._item;
            _curChef.GetChefMovementScript()._item = null;
            _foodItem.transform.SetParent(_cookwareItem.transform);
            _foodItem.transform.position = transform.position;
            _cookwareItem.GetComponent<SpriteRenderer>().sortingOrder = 0;
            _foodItem.GetComponent<SpriteRenderer>().sortingOrder = 1;
            _foodCookScript = _foodItem.GetComponent<CookableScript>();
            if (_foodCookScript)
            {
                _foodCookScript._cooking = true;
                _foodCookScript._cookware = _cookwareItem.tag;
            }
        }
        else if (_cookwareItem && !_curChef.GetChefMovementScript()._item && _foodItem) // player picks up pan with food
        {
            _audioList.PlayPickupAudio();
            if (_foodCookScript)
                _foodCookScript._cooking = false;
            _curChef.GetChefMovementScript()._item = _cookwareItem;
            _cookwareItem = null;
            _foodItem = null;
            _foodCookScript = null;
            _curChef.GetChefMovementScript()._item.transform.SetParent(_curChef.GetChefMovementScript()._transform);
            _curChef.GetChefMovementScript()._item.GetComponent<SpriteRenderer>().sortingOrder = 2;
            _curChef.GetChefMovementScript()._item.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 3;
            _curChef.GetChefMovementScript()._item.transform.position = new Vector2(_curChef.GetChefMovementScript()._transform.position.x, _curChef.GetChefMovementScript()._transform.position.y + 0.8f);
        }
        else if (_cookwareItem && !_curChef.GetChefMovementScript()._item && !_foodItem) // player picks up pan without food
        {
            _audioList.PlayPickupAudio();
            _curChef.GetChefMovementScript()._item = _cookwareItem;
            _cookwareItem = null;
            _curChef.GetChefMovementScript()._item.transform.SetParent(_curChef.GetChefMovementScript()._transform);
            _curChef.GetChefMovementScript()._item.GetComponent<SpriteRenderer>().sortingOrder = 2;
            _curChef.GetChefMovementScript()._item.transform.position = new Vector2(_curChef.GetChefMovementScript()._transform.position.x, _curChef.GetChefMovementScript()._transform.position.y + 0.8f);
        }
        else  //Otherwise, change the color back to normal
        {
            foreach (var color in _counterScript.CHEF_COLORS)
            {
                if (_counterScript._srend.color == color)
                    _counterScript._srend.color = _counterScript.DESELECT_COLOR;
            }
        }
    }
}
