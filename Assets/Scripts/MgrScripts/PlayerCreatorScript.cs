using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCreatorScript : MonoBehaviour
{
    //index 0 is player 1, index 1 is player 2, etc
    public GameObject[] _playerPrefabs;

    public GameObject[] _players;

    public Chef[] _chefs;

    public GameObject[] _spawns;

    public Color[] _highlighters = {new Color(204/255f, 121 / 255f, 47 / 255f), new Color(27/255f, 171 / 255f, 204 / 255f),
       new Color(153/255f, 199 / 255f, 80 / 255f), new Color(169/255f, 37 / 255f, 204 / 255f)};

    public int _playerCount;
    public bool _singlePlayer;
    public bool _networked;

    NetManScript _netMan;

    public static PlayerCreatorScript _instance { get; private set; } = null;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
        var sp = PlayerPrefs.GetInt(TagList.SINGLE_PLAYER, 1);
        _singlePlayer = sp == 1;
        print("is singleplayer: " + _singlePlayer);

        var n = PlayerPrefs.GetInt(TagList.NETWORKED, 0);
        _networked = n == 1;
        print("is networked: " + _networked);

        _playerCount = PlayerPrefs.GetInt(TagList.NUM_PLAYERS, 2);
        print("player count: " + _playerCount);


        //DONT FORGET YOU NEED TO ADD A WAY FOR THE PLAYER TO HAVE THE APROPRIATE TAG

        _chefs = new Chef[_playerCount];
        print(_playerCount + " total number of players");
        if (_networked)
        {

            _players = new GameObject[_playerCount];

            _netMan = FindObjectOfType<NetManScript>();


            /*
             * 
             * 
             * GET THE ID FROM NETMAN AND THEN WHEN THEY JOIN HAVE THEM GET PROCESSED BY THE SERVER AND THEN SYNCVAR THAT ISH
             * 
             * 
             */
        }
        else
        {
            _players = _singlePlayer ? new GameObject[2] : new GameObject[_playerCount]; //limit to 2 players no matter what if singleplayer
            for (int i = 0; i < _players.Length; i++)
            {
                AddPlayer(i, _singlePlayer);
            }
        }

    }
    
    public void AddPlayer(int i, bool singlePlayer)
    {
        _players[i] = Instantiate(_playerPrefabs[i], _spawns[i].transform.position, Quaternion.identity);
        Animator anim = _players[i].GetComponent<Animator>();
        //load animation controller


        if (singlePlayer)
            _players[i].transform.GetChild(4).GetComponent<SpriteRenderer>().color = _highlighters[0];
        else
        {
            _players[i].transform.GetChild(4).GetComponent<SpriteRenderer>().color = _highlighters[i];
            anim.runtimeAnimatorController = Resources.Load("Chef" + (i + 1) + "Animation") as RuntimeAnimatorController;
        }
        _players[i].tag = TagList.CHEFS[i];
        _chefs[i] = new Chef(_players[i]);
    }

    public void AddPlayerNetworked(int i, GameObject player)
    {
        _players[i] = player;
        
        
        //ONLY HAVE THE CIRCLE GUY IF YOU ARE LOCALPLAYER, MAYBE IN NETINPUT

        _players[i].transform.position = _spawns[i].transform.position;

        ////HATS

        _players[i].tag = TagList.CHEFS[i];
        _chefs[i] = new Chef(_players[i]);

        //Debug.LogError("Adding player " + player.tag + "at index " + i);
    }

    private void Update()
    {
        if(_networked)  //check if there is a plauer that needs to get added to the thing
            for (int i = 0; i < _players.Length; i++)
            {
                if(_players[i] == null || _chefs[i] == null) //might NoT dO aNyThInG
                {
                    //Debug.LogError("HEY THE PLAYER IS MISSING OVER HERE " + i);
                    var player = GameObject.FindGameObjectWithTag(TagList.CHEFS[i]);
                    if(player != null)
                    {
                        AddPlayerNetworked(i, player);
                    }
                } if(_chefs[i] == null && _players[i] != null)
                {
                    Debug.LogError("HEY THE CHEF IS MISSING OVER HERE " + i);
                    _chefs[i] = new Chef(_players[i]);
                }
            }
    }

    public int GetChefIndex(GameObject chef)
    {
        if (_networked)
        {
            for (int i = 0; i < _players.Length; i++)
            {
                if (_players[i].tag.Equals(chef.tag))
                {
                    print("Found the chef you were looking for at index " + i);
                    return i;
                }
            }
        }
        return -1;
    }
}
