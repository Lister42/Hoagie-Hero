using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour
{
    Rigidbody2D[] _rbodies;
    //Transform _transform1;
    //Transform _transform2;
    readonly float _speed = 6;
    //public bool pickup;

    public bool _singlePlayer;
    public int _numPlayers;
    bool _networked;
    //Vector2 _direction1;
    //Vector2 _direction2;


    public ChefSwitchManagerScript _switchManager;
    public LevelManagerScript _managerScript;
    // Start is called before the first frame update
    void Start()
    {
        PlayerCreatorScript creatorScript = PlayerCreatorScript._instance;

        _singlePlayer = creatorScript._singlePlayer;
        _numPlayers = creatorScript._playerCount;
        _networked = creatorScript._networked;
        if (_networked)
        {
            return;
        }


        var players = creatorScript._players;
        _rbodies = new Rigidbody2D[_numPlayers];

        for (int i = 0; i < _rbodies.Length; i++)
        {
            _rbodies[i] = players[i].GetComponent<Rigidbody2D>();
        }

        if (_singlePlayer)
        {
            _switchManager = FindObjectOfType<ChefSwitchManagerScript>();
        }
        _managerScript = FindObjectOfType<LevelManagerScript>();
    }



    private void Update()
    {
        if (_networked) return;
        
        if (_singlePlayer)
        {
            var h = Input.GetAxis("Player1H");
            var v = Input.GetAxis("Player1V");

            OnMove(new(h, v), _switchManager.CurrentChef().Equals(TagList.CHEF_ONE) ? 0 : 1);
            
            if (Input.GetButtonDown("Switch"))
                OnSwitch();
            if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Space))
                OnPickup(_switchManager.CurrentChef().Equals(TagList.CHEF_ONE) ? 0 : 1);
            if (Input.GetButtonDown("Pause"))
                _managerScript.Pause();
        } else
        {
            for (int i = 0; i < _numPlayers; i++)
            {
                var h = Input.GetAxis("Player"+(i + 1)+"H");
                var v = Input.GetAxis("Player"+(i + 1)+"V");
                OnMove(new Vector2(h, v), i);
            }


            if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Space))//player1
            {
                print("Player1 pickup");
                OnPickup(0);
            }else if (Input.GetKeyDown(KeyCode.Joystick2Button2) || Input.GetKeyDown(KeyCode.LeftShift))//player2
            {
                print("Player2 pickup");
                OnPickup(1);
            }
            else if (Input.GetKeyDown(KeyCode.Joystick3Button2))//player3
            {
                print("Player3 pickup");
                OnPickup(2);
            }
            else if (Input.GetKeyDown(KeyCode.Joystick4Button2))//player4
            {
                print("Player4 pickup");
                OnPickup(3);
            }
            if (Input.GetButtonDown("Pause"))
                _managerScript.Pause();
        }
    }

    void OnMove(Vector2 val, int player)
    {
        //print("doing input " + val + " for player " + player);
        //print(_rbodies[player]);
        _rbodies[player].velocity = _speed * val;
    }

    void OnSwitch()
    {
        if (_switchManager._currentChef.Equals(TagList.CHEF_ONE))
        {
            _switchManager._currentChef = TagList.CHEF_TWO;
            _switchManager._chefOneSelector.color = _switchManager._deselectColor;
            _switchManager._chefTwoSelector.color = _switchManager._selectorColor;

        }
        else if (_switchManager._currentChef.Equals(TagList.CHEF_TWO))
        {
            _switchManager._currentChef = TagList.CHEF_ONE;
            _switchManager._chefOneSelector.color = _switchManager._selectorColor;
            _switchManager._chefTwoSelector.color = _switchManager._deselectColor;
        }
    }

    private void OnPickup(int player)
    {
        CounterScript counter;
        BoxScript box;
        StoveScript stove;

        if (!_managerScript._chefsSelected[player]) return; //check if there is a counter selected

        counter = _managerScript._chefsSelected[player].GetComponent<CounterScript>();
        box = _managerScript._chefsSelected[player].GetComponent<BoxScript>();
        stove = _managerScript._chefsSelected[player].GetComponent<StoveScript>();

        if (counter._item == null && box)
        {
            box.Pickup(player);
        }
        else if (stove)
        {
            stove.Pickup(player);
        }
        else
        {
            counter.Pickup(player);
        }
    }
}
