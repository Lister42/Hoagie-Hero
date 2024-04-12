using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterScript : MonoBehaviour
{
    public Chef _curChef; //My idea behind using classes is for optimization. We don't have to "GetComponent" every time, and instead they're saved, and we can add more chefs easily this way in the future.
    public GameObject _item;
    public ItemScript _itemScript;
    public CookableScript _cookScript;
    public BreadScript _breadScript;

    #region Scripts
    private LevelManagerScript _managerScript;
    public BoxScript _boxScript;
    InputScript _inputScript;
    PlayerCreatorScript _creatorScript;
    #endregion

    public SpriteRenderer _srend;
    private Transform _transform;

    public Chef[] chefs;

    public bool singlePlayer;

    ChefSwitchManagerScript _chefSwitchManager;
    AudioList _audioList;

    bool cook, cut, grill;

    bool ready;

    public bool _puttingDown;

    bool _networked;

    #region Colors
    public Color[] CHEF_COLORS;
    public Color DESELECT_COLOR = Color.white;
    #endregion
    //hehe

    public int _hash;


    IEnumerator PutDownWait()
    {
        yield return new WaitForSeconds(.1f);
        _puttingDown = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        _creatorScript = PlayerCreatorScript._instance;
        CHEF_COLORS = _creatorScript._highlighters;
        _networked = PlayerPrefs.GetInt(TagList.NETWORKED) == 1;

        singlePlayer = _creatorScript._singlePlayer;

        #region Retrieve Components
        _srend = GetComponent<SpriteRenderer>();
        cut = GetComponent<CuttingScript>();
        cook = GetComponent<StoveScript>();
        grill = GetComponent<GrillScript>();
        _transform = transform;
        #endregion

        #region Assign Scripts
        _managerScript = GameObject.FindGameObjectWithTag(TagList.MANAGER).GetComponent<LevelManagerScript>();
        _inputScript = GameObject.FindGameObjectWithTag(TagList.INPUT).GetComponent<InputScript>();
        _boxScript = gameObject.GetComponent<BoxScript>();
        _audioList = GameObject.FindGameObjectWithTag(TagList.MANAGER).GetComponent<AudioList>();
        #endregion

        chefs = _creatorScript._chefs;

        if (singlePlayer)
        {
            _chefSwitchManager = GameObject.FindGameObjectWithTag(TagList.MANAGER).GetComponent<ChefSwitchManagerScript>();

            _curChef = chefs[_chefSwitchManager.GetChefIndex()];
        }

        var x = transform.position.x + 30;
        var y = transform.position.y + 30;


        _hash = (int)((.5 * (x + y) * (x + y + 1)) + y);
    }

    // Update is called once per frame
    void Update()
    {
        if (_creatorScript._networked)
        {
            //print(_creatorScript._chefs);
            for (int i = 0; i < chefs.Length; i++)
            {
                if (chefs[i] == null)
                {
                    //if (_creatorScript._chefs[i] == null)
                    //    Debug.LogError("chef " + i + " is null in creatorscript");
                    chefs[i] = _creatorScript._chefs[i];
                }
            }
        }

        if (singlePlayer)
        {
            int chef = _chefSwitchManager.GetChefIndex();

            if (_managerScript._chefsSelected[chef] && _managerScript._chefsSelected[chef] == gameObject)
            {
                return;
            }

            if (_srend.color != DESELECT_COLOR) //Otherwise, change the color back to normal
            {
                _srend.color = DESELECT_COLOR;
            }
        }
        else
        {
            for (int i = 0; i < _managerScript._chefsSelected.Length; i++)
            {
                if (_managerScript._chefsSelected[i] && _managerScript._chefsSelected[i] == gameObject)
                {
                    return;
                }
            }

            if (_srend.color != DESELECT_COLOR) //Otherwise, change the color back to normal
            {
                _srend.color = DESELECT_COLOR;
            }
        }
    }

    public void Pickup(int player) //Need to add check if pan is currently being held in hand
    {
        _curChef = chefs[player]; //Update Chef to current chef based on ChefSwitchManager. Since everything is based off the current chef, it auto updates.

        if (_curChef.GetChefMovementScript()._item && !_item) //player puts down item
        {
            if (_puttingDown) return;
            if (_curChef.GetChefMovementScript()._item.CompareTag(TagList.PAN) || _curChef.GetChefMovementScript()._item.CompareTag(TagList.POT)) //That item is a pan or pot
            {
                if (_curChef.GetChefMovementScript()._item.transform.childCount != 0 && !_item && _curChef.GetChefMovementScript()._item.CompareTag(TagList.POT)) //placing pot on counter
                {
                    if (GetComponent<TrashScript>())
                    {
                        _audioList.PlayPlaceAudio();
                        Destroy(_curChef.GetChefMovementScript()._item.transform.GetChild(0).gameObject);
                        _curChef.GetChefMovementScript()._item.transform.DetachChildren();
                    }
                    else
                    {
                        _audioList.PlayPlaceAudio();
                        _item = _curChef.GetChefMovementScript()._item;
                        _curChef.GetChefMovementScript()._item = null;
                        _item.transform.SetParent(_transform);
                        _item.GetComponent<SpriteRenderer>().sortingOrder = 0;
                        _item.transform.position = _transform.position;
                        _itemScript = _item.GetComponent<ItemScript>();
                        _cookScript = _item.GetComponent<CookableScript>();
                        _breadScript = _item.GetComponent<BreadScript>();
                        if (cut && _itemScript)
                            _itemScript._cutting = true;
                        if (cut && _cookScript)
                            _cookScript._cutting = true;
                        if (cook && _cookScript)
                            _cookScript._cooking = true;
                        if (grill && _breadScript)
                            _breadScript._cooking = true;
                    }
                }
                else if (_curChef.GetChefMovementScript()._item.transform.childCount != 0 && !_item && _curChef.GetChefMovementScript()._item.CompareTag(TagList.PAN))
                {
                    _audioList.PlayPlaceAudio();
                    _item = _curChef.GetChefMovementScript()._item.transform.GetChild(0).gameObject;
                    _curChef.GetChefMovementScript()._item.transform.DetachChildren();
                    _item.transform.SetParent(_transform);
                    _item.GetComponent<SpriteRenderer>().sortingOrder = 0;
                    _item.transform.position = _transform.position;
                    _itemScript = _item.GetComponent<ItemScript>();

                }
                else
                {
                    _audioList.PlayPlaceAudio();
                    _item = _curChef.GetChefMovementScript()._item;
                    _curChef.GetChefMovementScript()._item = null;
                    _item.transform.SetParent(_transform);
                    _item.GetComponent<SpriteRenderer>().sortingOrder = 0;
                    _item.transform.position = _transform.position;
                    _itemScript = _item.GetComponent<ItemScript>();
                }
            }
            else
            {
                _audioList.PlayPlaceAudio();
                _item = _curChef.GetChefMovementScript()._item;
                _curChef.GetChefMovementScript()._item = null;
                _item.transform.SetParent(_transform);
                _item.GetComponent<SpriteRenderer>().sortingOrder = 0;
                _item.transform.position = _transform.position;
                _itemScript = _item.GetComponent<ItemScript>();
                _cookScript = _item.GetComponent<CookableScript>();
                _breadScript = _item.GetComponent<BreadScript>();
                if (cut && _itemScript)
                    _itemScript._cutting = true;
                if (cut && _cookScript)
                    _cookScript._cutting = true;
                if (cook && _cookScript)
                    _cookScript._cooking = true;
                if (grill && _breadScript)
                    _breadScript._cooking = true;
            }
        }
        else if (_item && !_curChef.GetChefMovementScript()._item) // player picks up item
        {
            _audioList.PlayPickupAudio();
            if (cut && _itemScript)
                _itemScript._cutting = false;
            if (cut && _cookScript)
                _cookScript._cutting = false;
            if (cook && _cookScript)
                _cookScript._cooking = false;
            if (grill && _breadScript)
                _breadScript._cooking = false;
            _curChef.GetChefMovementScript()._item = _item;
            _item = null;
            _itemScript = null;
            _cookScript = null;
            _breadScript = null;
            _curChef.GetChefMovementScript()._item.transform.SetParent(_curChef.GetChefMovementScript()._transform);
            _curChef.GetChefMovementScript()._item.GetComponent<SpriteRenderer>().sortingOrder = 2;
            _curChef.GetChefMovementScript()._item.transform.position = new Vector2(_curChef.GetChefMovementScript()._transform.position.x, _curChef.GetChefMovementScript()._transform.position.y + 0.8f);

            _puttingDown = true;
            StartCoroutine(PutDownWait());
        }

        else if (_curChef.GetChefMovementScript()._item && !_curChef.GetChefMovementScript()._item.CompareTag(TagList.BREAD) && _item.CompareTag(TagList.BREAD) && !_curChef.GetChefMovementScript()._item.CompareTag(TagList.PAN) && !_curChef.GetChefMovementScript()._item.CompareTag(TagList.POT)
            && _curChef.GetChefMovementScript()._item.GetComponent<ItemScript>()._cut) //put item on bread
        {
            _audioList.PlayPlaceAudio();
            _item.GetComponent<BreadScript>().AddIngredient(_curChef.GetChefMovementScript()._item);
            Destroy(_curChef.GetChefMovementScript()._item);
            _curChef.GetChefMovementScript()._item = null;
        }
        else if (_curChef.GetChefMovementScript()._item && _item.CompareTag(TagList.BREAD) && (_curChef.GetChefMovementScript()._item.CompareTag(TagList.PAN) || _curChef.GetChefMovementScript()._item.CompareTag(TagList.POT))) //place item on bread from pot/pan
        {
            _audioList.PlayPlaceAudio();
            if (_curChef.GetChefMovementScript()._item.transform.childCount > 0 && _curChef.GetChefMovementScript()._item.transform.GetChild(0).GetComponent<CookableScript>()._cooked)
            {
                _item.GetComponent<BreadScript>().AddIngredient(_curChef.GetChefMovementScript()._item.transform.GetChild(0).gameObject);
                Destroy(_curChef.GetChefMovementScript()._item.transform.GetChild(0).gameObject);
            }
        }
        else if (_curChef.GetChefMovementScript()._item && _curChef.GetChefMovementScript()._item && _curChef.GetChefMovementScript()._item.CompareTag(TagList.BREAD) && !_item.CompareTag(TagList.BREAD)) //bread scooping
        {
            if (_item.CompareTag(TagList.POT) || _item.CompareTag(TagList.PAN)) return;
            if (!_item.GetComponent<ItemScript>()._cut) return;
            _audioList.PlayPickupAudio();
            _curChef.GetChefMovementScript()._item.GetComponent<BreadScript>().AddIngredient(_item);
            Destroy(_item);
            _item = null;
        }
        else  //Otherwise, change the color back to normal
        {
            foreach (var color in CHEF_COLORS)
            {
                if (_srend.color == color)
                    _srend.color = DESELECT_COLOR;
            }
        }
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        var input = collision.gameObject.transform.parent.GetComponent<NetworkedInput>();
        if (input != null)
        { //if other player on network
            if (!input.isLocalPlayer)
                return;
            else
            {
                if (ChefSelect(collision.gameObject))
                {
                    ChangeSelectedCounterColor(collision.gameObject.transform.parent.tag);
                    string chefTag = collision.gameObject.transform.parent.gameObject.tag;//find the chef that hit it
                    int chefIndex = -1;
                    for (int i = 0; i < chefs.Length; i++)
                    {
                        if (chefs[i] != null)
                        {
                            if (chefs[i].GetChef().CompareTag(chefTag))
                            {
                                chefIndex = i;
                                break;
                            }
                        }
                    }

                    //Debug.LogError("counter " + _hash + " SELECTED by chef " + chefIndex);

                    input.CmdSetSelected(_hash, chefIndex);
                }
            }
        }
        else
        {
            if(ChefSelect(collision.gameObject))
                ChangeSelectedCounterColor(collision.gameObject.transform.parent.tag);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.LogError("counter " + _hash + " trigger Exit");
        var input = collision.gameObject.transform.parent.gameObject.GetComponent<NetworkedInput>();
        //Debug.LogError("input script is " + input);
        if (input != null && !input.isLocalPlayer) //client authority baby
            return;

        string colTag = collision.gameObject.tag;
        if (colTag.Equals(TagList.CHEF_BOTTOM_DETECTOR) || colTag.Equals(TagList.CHEF_RIGHT_DETECTOR) || colTag.Equals(TagList.CHEF_LEFT_DETECTOR) || colTag.Equals(TagList.CHEF_ABOVE_DETECTOR))
        {
            for (int i = 0; i < _managerScript._chefsSelected.Length; i++)
            {
                if (_managerScript._chefsSelected[i] == gameObject)
                {
                    _managerScript._chefsSelected[i] = null;
                    _srend.color = DESELECT_COLOR;

                    //Debug.LogError("counter " + _hash + " DESELECTED by chef " + i);
                    if (input != null)
                    {
                        input.CmdDeselect(_hash, i); //update other clients
                    }
                }
            }

        }
    }

    public bool ChefSelect(GameObject detector)
    {
        string tag = detector.tag;

        string chefTag = detector.transform.parent.gameObject.tag;

        int chefIndex = -1;
        for (int i = 0; i < chefs.Length; i++)
        {
            if (chefs[i] != null)
            {
                if (chefs[i].GetChef().CompareTag(chefTag))
                {
                    chefIndex = i;
                    break;
                }
            }
            //else Debug.LogError("Chefs[" + i + "] is null. FIX IT");
        }

        if (chefIndex < 0) { print("Error in ChefSelect, collision detection is busted"); return false; } //Something bad happened

        var direction = chefs[chefIndex].GetChefMovementScript()._direction; ;


        switch (tag)
        {
            case TagList.CHEF_ABOVE_DETECTOR:
                switch (direction)
                {
                    case DirectionStateList.Direction.UP:
                        _managerScript._chefsSelected[chefIndex] = gameObject;
                        return true;

                    case DirectionStateList.Direction.RIGHT:
                        if (!chefs[chefIndex].GetLeftDetector().IsColliding() && !chefs[chefIndex].GetRightDetector().IsColliding() && !chefs[chefIndex].GetBottomDetector().IsColliding())
                        {
                            _managerScript._chefsSelected[chefIndex] = gameObject;
                            return true;
                        }
                        break;

                    case DirectionStateList.Direction.LEFT:
                        if (!chefs[chefIndex].GetLeftDetector().IsColliding() && !chefs[chefIndex].GetRightDetector().IsColliding() && !chefs[chefIndex].GetBottomDetector().IsColliding())
                        {
                            _managerScript._chefsSelected[chefIndex] = gameObject;
                            return true;
                        }
                        break;

                    case DirectionStateList.Direction.DOWN:
                        if (!chefs[chefIndex].GetLeftDetector().IsColliding() && !chefs[chefIndex].GetRightDetector().IsColliding() && !chefs[chefIndex].GetBottomDetector().IsColliding())
                        {
                            _managerScript._chefsSelected[chefIndex] = gameObject;
                            return true;
                        }
                        break;
                }
                break;

            case TagList.CHEF_RIGHT_DETECTOR:
                switch (direction)
                {
                    case DirectionStateList.Direction.RIGHT:
                        _managerScript._chefsSelected[chefIndex] = gameObject;
                        return true;

                    case DirectionStateList.Direction.DOWN:
                        if (!chefs[chefIndex].GetBottomDetector().IsColliding() && !chefs[chefIndex].GetLeftDetector().IsColliding() && !chefs[chefIndex].GetAboveDetector().IsColliding())
                        {
                            _managerScript._chefsSelected[chefIndex] = gameObject;
                            return true;
                        }
                        break;

                    case DirectionStateList.Direction.LEFT:
                        if (!chefs[chefIndex].GetBottomDetector().IsColliding() && !chefs[chefIndex].GetLeftDetector().IsColliding() && !chefs[chefIndex].GetAboveDetector().IsColliding())
                        {
                            _managerScript._chefsSelected[chefIndex] = gameObject;
                            return true;
                        }
                        break;

                    case DirectionStateList.Direction.UP:
                        if (!chefs[chefIndex].GetBottomDetector().IsColliding() && !chefs[chefIndex].GetLeftDetector().IsColliding() && !chefs[chefIndex].GetAboveDetector().IsColliding())
                        {
                            _managerScript._chefsSelected[chefIndex] = gameObject;
                            return true;
                        }
                        break;
                }
                break;

            case TagList.CHEF_BOTTOM_DETECTOR:
                switch (direction)
                {
                    case DirectionStateList.Direction.DOWN:
                        _managerScript._chefsSelected[chefIndex] = gameObject;
                        return true;

                    case DirectionStateList.Direction.LEFT:
                        if (!chefs[chefIndex].GetLeftDetector().IsColliding() && !chefs[chefIndex].GetAboveDetector().IsColliding() && !chefs[chefIndex].GetRightDetector().IsColliding())
                        {
                            _managerScript._chefsSelected[chefIndex] = gameObject;
                            return true;
                        }
                        break;

                    case DirectionStateList.Direction.UP:
                        if (!chefs[chefIndex].GetLeftDetector().IsColliding() && !chefs[chefIndex].GetAboveDetector().IsColliding() && !chefs[chefIndex].GetRightDetector().IsColliding())
                        {
                            _managerScript._chefsSelected[chefIndex] = gameObject;
                            return true;
                        }
                        break;

                    case DirectionStateList.Direction.RIGHT:
                        if (!chefs[chefIndex].GetLeftDetector().IsColliding() && !chefs[chefIndex].GetAboveDetector().IsColliding() && !chefs[chefIndex].GetRightDetector().IsColliding())
                        {
                            _managerScript._chefsSelected[chefIndex] = gameObject;
                            return true;
                        }
                        break;
                }
                break;

            case TagList.CHEF_LEFT_DETECTOR:
                switch (direction)
                {
                    case DirectionStateList.Direction.LEFT:
                        _managerScript._chefsSelected[chefIndex] = gameObject;
                        return true;

                    case DirectionStateList.Direction.UP:
                        if (!chefs[chefIndex].GetAboveDetector().IsColliding() && !chefs[chefIndex].GetRightDetector().IsColliding() && !chefs[chefIndex].GetBottomDetector().IsColliding())
                        {
                            _managerScript._chefsSelected[chefIndex] = gameObject;
                            return true;
                        }
                        break;

                    case DirectionStateList.Direction.RIGHT:
                        if (!chefs[chefIndex].GetAboveDetector().IsColliding() && !chefs[chefIndex].GetRightDetector().IsColliding() && !chefs[chefIndex].GetBottomDetector().IsColliding())
                        {
                            _managerScript._chefsSelected[chefIndex] = gameObject;
                            return true;
                        }
                        break;

                    case DirectionStateList.Direction.DOWN:
                        if (!chefs[chefIndex].GetAboveDetector().IsColliding() && !chefs[chefIndex].GetRightDetector().IsColliding() && !chefs[chefIndex].GetBottomDetector().IsColliding())
                        {
                            _managerScript._chefsSelected[chefIndex] = gameObject;
                            return true;
                        }
                        break;
                }
                break;

            default:
                print("Error in ChefSelect, collision detection is busted");
                break;
        }
        return false;
    }
    public void ChangeSelectedCounterColor(string tag)
    {
        for (int i = 0; i < TagList.CHEFS.Length; i++)
        {
            if (tag.Equals(TagList.CHEFS[i]))
                _srend.color = CHEF_COLORS[i];
        }
    }
}