using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;



public class NetworkedInput : NetworkBehaviour
{
    Rigidbody2D _rbody;
    readonly float _speed = 6;
    public LevelManagerScript _managerScript;
    public NetManScript _netMan;
    public NetworkAnimatorScript _networkAnimatorScript;
    public int _chefIndex;
    Transform _transform;
    [SyncVar]
    public bool _isChopping;


    // movement sync stuff
    static NetworkedInput _localPlayer;
    [SyncVar]
    Vector2 _receivedPosn;


    float[] rttArr = { .1f, .1f, .1f, .1f, .1f };
    int nextRttPosn = 0;
    float avgRtt = 0.1f;

    [SyncVar]
    public int _playerNum;

    [SyncVar]
    public int _connectionID;

    [Command]
    void CmdSetConnID()
    {
        _connectionID = connectionToClient.connectionId;
        Debug.Log(" in server " + _connectionID);
        var numPlayers = NetworkManager.singleton.numPlayers;
        print(" in server number of players" + numPlayers);

        var playerCreator = PlayerCreatorScript._instance;
        var netMan = FindObjectOfType<NetManScript>();

        if (FindObjectOfType<NetPlayerConnectScript>())
        {
            Debug.LogError("writing to position " + (numPlayers - 1) + " in the playerID array");
            var ids = FindObjectOfType<NetManScript>()._playerIDs;
            ids[numPlayers - 1] = _connectionID;
            foreach (var item in ids)
            {
                print("id: " + item);
            }
        }
        else
        {
            Debug.LogError("checking player with connection " + _connectionID + " against the array");
            for (int i = 0; i < playerCreator._players.Length; i++)
            {
                if (netMan._playerIDs[i] == _connectionID)
                {
                    Debug.LogError("found a match at index " + i);

                    tag = TagList.CHEFS[i];

                    _playerNum = i;

                    var pcscript = PlayerCreatorScript._instance;

                    transform.position = pcscript._spawns[i].transform.position;

                    Animator anim = GetComponent<Animator>();
                    //load animation controller
                    var animator =  Resources.Load("Chef" + (i + 1) + "Animation") as RuntimeAnimatorController;

                    anim.runtimeAnimatorController = animator;

                    Debug.LogError("added animatpr " + animator);

                    //transform.GetChild(4).GetComponent<SpriteRenderer>().color = pcscript._highlighters[i];

                    RpcUpdatePlayers(i, TagList.CHEFS[i]);
                }
            }
        }
    }

    [ClientRpc]
    void RpcUpdatePlayers(int i, string newTag)
    {
        Debug.LogError("getting player " + i + " namme " + newTag);
        tag = newTag;
        transform.position = PlayerCreatorScript._instance._spawns[i].transform.position;
        Animator anim = GetComponent<Animator>();
        //load animation controller
        anim.runtimeAnimatorController = Resources.Load("Chef"+ (i + 1) + "Animation") as RuntimeAnimatorController;
    }

    private void Start() //HEY MAKE THE PLAYERS GIVE THEMSELVSES AN APPROPRIATE TAG MAYBE TARGETrpc MAYBE MAKE A HASHCODE
    {
        _transform = transform;
        _rbody = GetComponent<Rigidbody2D>();
        GetComponent<ChefMovementScript>()._isLocalPlayer = isLocalPlayer; //set the chef movement script to be local or not
        //print("starting the player" + connectionToClient.connectionId);
        _networkAnimatorScript = GetComponent<NetworkAnimatorScript>();
        _netMan = FindObjectOfType<NetManScript>();
        if (GameObject.FindGameObjectWithTag(TagList.MANAGER))
            _managerScript = GameObject.FindGameObjectWithTag(TagList.MANAGER).GetComponent<LevelManagerScript>();

        //transform.position = new(0, -2);

        if (isLocalPlayer)
        {
            _localPlayer = this;
            CmdSetConnID();
            
            if(PlayerCreatorScript._instance)
                transform.GetChild(4).GetComponent<SpriteRenderer>().color = PlayerCreatorScript._instance._highlighters[_playerNum];
            
            Debug.LogError("In localPlayer " + _connectionID);
            StartCoroutine(clock());
        }
        else
        {
            transform.GetChild(4).GetComponent<SpriteRenderer>().color = Color.clear;
        }
        AnimatorSetup();
    }

    private void Update()
    {
        //update player info
        if (!CompareTag(TagList.CHEFS[_playerNum])){
            tag = TagList.CHEFS[_playerNum];
            transform.position = PlayerCreatorScript._instance._spawns[_playerNum].transform.position;
            Animator anim = GetComponent<Animator>();
            //load animation controller
            anim.runtimeAnimatorController = Resources.Load("Chef" + (_playerNum + 1) + "Animation") as RuntimeAnimatorController;
        }

        if(_transform.position.x < -7.5 && _transform.position.y < .5 && _transform.position.y > -.5)
        {
            _transform.position = PlayerCreatorScript._instance._spawns[_playerNum].transform.position;
        }
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Space))
            OnPickup();
        if (Input.GetButtonDown("Pause"))
            _managerScript.Pause();
    }

    private void OnMove(Vector2 move)
    {
        _rbody.velocity = _speed * move;
    }

    private void OnPickup()
    {
        if (!_managerScript._chefsSelected[_playerNum]) return;

        var hash = _managerScript._chefsSelected[_playerNum].GetComponent<CounterScript>()._hash;
        //CLIENT AUTHROURATIATIVE DECISION OF WHICH COUNTER TO PICKUP

        CmdPickup(_playerNum, hash);
    }

    [Command]
    void CmdPickup(int player, int hash)
    {
        print("resolving pickup on the server for player " + player);
        RpcPickup(player, hash);
    }

    [ClientRpc]
    void RpcPickup(int player, int hash)
    {
        print("resolving pickup on the client for player " + player);

        GameObject surface = null;

        var counters = FindObjectsOfType<CounterScript>();

        foreach (var c in counters)
        {
            if (c._hash == hash)
            {
                surface = c.gameObject;
                break;
            }
        }

        if (surface == null)
            return;

        CounterScript counter = surface.GetComponent<CounterScript>();
        BoxScript box = surface.GetComponent<BoxScript>();
        StoveScript stove = surface.GetComponent<StoveScript>();


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


    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            var h = Input.GetAxis("Player1H");
            var v = Input.GetAxis("Player1V");

            OnMove(new(h, v));
        }
        else
        {
            LerpRemote();
        }

        if (isLocalPlayer)
        {
            CmdSetRttTimestamp(Time.time);
        }
    }

    void LerpRemote()
    {
        if (Vector2.Distance(_rbody.position, _receivedPosn) < .5)
        {
            return;
        }

        var delta = ((_receivedPosn - _rbody.position) / 0.1f) * _localPlayer.avgRtt / 2;

        _rbody.MovePosition(Vector2.Lerp(_rbody.position, _receivedPosn + delta, .2f));
        _rbody.MovePosition(Vector2.Lerp(_rbody.position, _receivedPosn, .2f));
    }

    [Command]
    void CmdUpdatePosn(Vector2 posn)
    {
        _receivedPosn = posn;
        RpcUpdatePosn(posn);
    }

    [ClientRpc]
    void RpcUpdatePosn(Vector2 posn)
    {
        if (isServer) return;
        _receivedPosn = posn;
    }

    [Command]
    void CmdSetRttTimestamp(float time)
    {
        TargetSendRttTimeStamp(connectionToClient, time);
    }

    [TargetRpc]
    void TargetSendRttTimeStamp(NetworkConnection conn, float time)
    {
        if (isLocalPlayer)
        {
            if (rttArr.Length == 0) return;
            float dif = Time.time - time;
            rttArr[nextRttPosn] = dif;
            nextRttPosn = (nextRttPosn + 1) % rttArr.Length;
            float sum = 0;
            foreach (var item in rttArr)
            {
                sum += item;
            }
            avgRtt = sum / rttArr.Length;
            //print(avgRtt);
        }
    }

    IEnumerator clock()
    {
        for (; ; )
        {
            CmdUpdatePosn(_rbody.position);
            yield return new WaitForSeconds(0.1f);
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdSetSelected(int id, int chefIndex)
    {
        RpcSetSelected(id, chefIndex);
    }

    [Command(requiresAuthority = false)]
    public void CmdDeselect(int id, int chefIndex)
    {
        RpcDeselect(id, chefIndex);
    }

    [ClientRpc]
    void RpcSetSelected(int id, int chefIndex)
    {
        if (isLocalPlayer) return;

        //Debug.LogError("*** RECIEVED MESSAGE *** counter " + id + " SELECTED by chef " + chefIndex);

        var counters = FindObjectsOfType<CounterScript>();

        foreach (var counter in counters)
        {
            if (counter._hash == id)
            {
                if(_managerScript)
                    _managerScript._chefsSelected[chefIndex] = counter.gameObject;
                counter.ChangeSelectedCounterColor(TagList.CHEFS[chefIndex]);
                return;
            }
        }
    }

    [ClientRpc]
    void RpcDeselect(int id, int chefIndex)
    {
        if (isLocalPlayer) return;


        //Debug.LogError("*** RECIEVED MESSAGE *** counter " + id + " DESELECTED by chef " + chefIndex);

        var counters = FindObjectsOfType<CounterScript>();

        foreach (var counter in counters)
        {
            if (counter._hash == id)
            {
                _managerScript._chefsSelected[chefIndex] = null;
                counter._srend.color = counter.DESELECT_COLOR;
                return;
            }
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdUpdateAnimation(bool is_standing, bool is_moving, bool should_flip)
    {
        RpcUpdateAnimation(is_standing, is_moving, should_flip);
    }

    [ClientRpc]
    public void RpcUpdateAnimation(bool is_standing, bool is_moving, bool should_flip)
    {
        if (isLocalPlayer) return;
        Animator animator = GetComponent<ChefMovementScript>()._animator;
        SpriteRenderer srend = GetComponent<ChefMovementScript>()._srenderer;

        animator.SetBool(AnimStateList.IS_STANDING, is_standing);
        animator.SetBool(AnimStateList.IS_MOVING, is_moving);

        if (should_flip)
        {
            srend.flipX = true;
        }
        else
        {
            srend.flipX = false;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdUpdateChoppingAnimation(bool is_chopping)
    {
        RpcUpdateChoppingAnimation(is_chopping);
    }

    [ClientRpc]
    public void RpcUpdateChoppingAnimation(bool is_chopping)
    {
        if (isLocalPlayer) return;
        Animator animator = GetComponent<ChefMovementScript>()._animator;
        SpriteRenderer srend = GetComponent<ChefMovementScript>()._srenderer;

        animator.SetBool(AnimStateList.IS_CHOPPING, is_chopping);
    }

    public void AnimatorSetup()
    {
        _chefIndex = _networkAnimatorScript.AddChefToAnimator(gameObject);
    }
}