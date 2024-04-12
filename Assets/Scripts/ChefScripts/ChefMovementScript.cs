using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Implements Player movement and animation
 */

[RequireComponent(typeof(Rigidbody2D))]
public class ChefMovementScript : MonoBehaviour
{
    #region Components
    private Rigidbody2D _rbody;
    public Animator _animator;
    public SpriteRenderer _srenderer;
    public Transform _transform;
    #endregion

    public float speed = 6;
    public DirectionStateList.Direction _direction;
    public GameObject _item;
    private Vector2 oldPosn;
    bool _isTitle;

    bool singlePlayer;

    #region Audio Variables
    private bool _playChopSound;
    private bool _isChopping;
    //---
    private bool _playRunningSound;
    private bool _isRunning;
    #endregion
    private AudioList _audio;

    public bool _isLocalPlayer;
    public bool _networked;

    #region Animator Booleans
    bool is_moving = false;
    bool is_standing = false;
    bool is_chopping = false;
    bool should_flip = false;
    #endregion

    private ChefSwitchManagerScript _chefSwitchManager;
    private NetworkedInput _networkedInput;
    private PlayerCreatorScript _playerCreator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _srenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        if (FindObjectOfType<TitleSceneScript>())
        {
            _rbody = _rbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _srenderer = GetComponent<SpriteRenderer>();
            _transform = transform;
            _direction = DirectionStateList.Direction.NONE;
            _isTitle = true;
        }
        else
        {
            _networked = PlayerPrefs.GetInt(TagList.NETWORKED) == 1;
            _playerCreator = PlayerCreatorScript._instance;
            if (_playerCreator != null && _playerCreator._singlePlayer)
                _chefSwitchManager = GameObject.FindGameObjectWithTag(TagList.MANAGER).GetComponent<ChefSwitchManagerScript>();
            if (_networked) _networkedInput = GetComponent<NetworkedInput>();
            _rbody = GetComponent<Rigidbody2D>();
            //_animator = GetComponent<Animator>();
            _transform = transform;
            _direction = DirectionStateList.Direction.NONE;
            _playChopSound = false;
            _isChopping = false;
            _playRunningSound = false;
            _isRunning = false;
            _isTitle = false;
            oldPosn = new Vector2(_transform.position.x, _transform.position.y);
            if(GameObject.FindGameObjectWithTag(TagList.MANAGER))
                _audio = GameObject.FindGameObjectWithTag(TagList.MANAGER).GetComponent<AudioList>();
        }
    }

    private void Update()
    {
        if (_playerCreator == null && !_isTitle && _networked) //keep looking for it if you get instantiated in later
        {
            _playerCreator = PlayerCreatorScript._instance;
        }
        if (_networked && !_isLocalPlayer) return; //client authoratative direction

        var vel = _rbody.velocity;
        if (vel.x > 0) //Right
        {
            _direction = DirectionStateList.Direction.RIGHT;
        }
        if (vel.x < 0) //Left
        {
            _direction = DirectionStateList.Direction.LEFT;
        }
        if (vel.y > 0) //Up
        {
            _direction = DirectionStateList.Direction.UP;
        }
        if (vel.y < 0) //Down
        {
            _direction = DirectionStateList.Direction.DOWN;
        }
        if (!_isTitle && !_networked && _playerCreator._singlePlayer && !gameObject.CompareTag(_chefSwitchManager._currentChef))//stop chef if not selected
            _rbody.velocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (_isTitle)
        {
            CheckAnimation(_rbody.velocity);
            return;
        }
        if(_audio != null)
            CheckAudio();

        if (_networked && !_isLocalPlayer) return; //client authoratative animation

        //Check if networked as localplayer, then update everyone else as to wait the heck you're doing
        if (_networked)
        {
            NetworkedCheckAnimation(_rbody.velocity);
        }
        else
        {
            CheckAnimation(_rbody.velocity);
        }
        AdvancedChoppingAnim();
    }

    private void CheckAnimation(Vector2 direction)
    {
        if (direction.y != 0) //Chef is moving vertically, but could be moving horizontally too
        {
            _animator.SetBool(AnimStateList.IS_MOVING, true);
            _animator.SetBool(AnimStateList.IS_STANDING, false);
            _animator.SetBool(AnimStateList.IS_CHOPPING, false);
            if (direction.x > 0)
            {
                _srenderer.flipX = false;
            }
            else if (direction.x < 0)
            {
                _srenderer.flipX = true;
            }

            if (!_isRunning)
            {
                _isRunning = true;
                _playRunningSound = true;
            }
            _isChopping = false;
        }
        else if (direction.x != 0) //Chef is moving ONLY horizontally
        {
            _animator.SetBool(AnimStateList.IS_MOVING, true);
            _animator.SetBool(AnimStateList.IS_STANDING, false);
            _animator.SetBool(AnimStateList.IS_CHOPPING, false);
            if (direction.x > 0)
            {
                _srenderer.flipX = false;
            }
            else if (direction.x < 0)
            {
                _srenderer.flipX = true;
            }

            if (!_isRunning)
            {
                _isRunning = true;
                _playRunningSound = true;
            }
            _isChopping = false;
        }
        else //Chef is standing or chopping if not moving
        {
            _animator.SetBool(AnimStateList.IS_MOVING, false);
            _animator.SetBool(AnimStateList.IS_STANDING, true);
            _isRunning = false;
        }
    }

    public void IsChoppingAnim()
    {
        _animator.SetBool(AnimStateList.IS_CHOPPING, true);
        if (_networked)
        {
            if (_networkedInput.isServer)
            {
                if (!_networkedInput._isChopping) //Only run if the player isn't already chopping
                {
                    _networkedInput._isChopping = true;
                    _playChopSound = true;
                }
                _networkedInput.RpcUpdateChoppingAnimation(true);
            }
            else
            {
                if (!_networkedInput._isChopping) //Only run if the player isn't already chopping
                {
                    _networkedInput._isChopping = true;
                    _playChopSound = true;
                }
                _networkedInput.CmdUpdateChoppingAnimation(true);
            }
        }
        else
        {
            _isChopping = true;
            _playChopSound = true;
        }
    }


    public void IsNotChoppingAnim()
    {
        _animator.SetBool(AnimStateList.IS_CHOPPING, false);
        if (_networked)
        {
            if (_networkedInput.isServer)
            {
                _networkedInput._isChopping = false;
                _networkedInput.RpcUpdateChoppingAnimation(false);
            }
            else
            {
                _networkedInput._isChopping = false;
                _networkedInput.CmdUpdateChoppingAnimation(false);
            }
        }
        else
        {
            _isChopping = false;
        }
    }


    private void CheckAudio()
    {
        if (_networked && _playChopSound && _networkedInput._isChopping && !_audio._isChopping)
        {
            StartCoroutine(ChopSoundBuffer());
        }
        if (_playChopSound && _isChopping && !_audio._isChopping)
        {
            StartCoroutine(ChopSoundBuffer());
        }
        if (_playRunningSound && _isRunning && !_audio._isRunning)
        {
            StartCoroutine(RunningSoundBuffer());
        }
    }

    private IEnumerator ChopSoundBuffer()
    {
        _playChopSound = false;

        _audio.PlayChopAudio();
        _audio._isChopping = true;
        yield return new WaitForSeconds(0.4f);
        _audio._isChopping = false;

        _playChopSound = true;
    }

    private IEnumerator RunningSoundBuffer()
    {
        _playRunningSound = false;

        _audio.PlayRunningAudio();
        _audio._isRunning = true;
        yield return new WaitForSeconds(0.4f);
        _audio._isRunning = false;

        _playRunningSound = true;
    }

    private void AdvancedChoppingAnim()
    {
        if (_item)
        {
            IsNotChoppingAnim();
        }

        Vector2 newPosn = new Vector2(_transform.position.x, _transform.position.y);

        if (oldPosn != (new Vector2(_transform.position.x, _transform.position.y)))
        {
            IsNotChoppingAnim();
        }

        oldPosn = newPosn;
    }

    private void NetworkedCheckAnimation(Vector2 direction)
    {
        if (direction.y != 0) //Chef is moving vertically, but could be moving horizontally too
        {
            _animator.SetBool(AnimStateList.IS_MOVING, true);
            _animator.SetBool(AnimStateList.IS_STANDING, false);
            _animator.SetBool(AnimStateList.IS_CHOPPING, false);
            is_moving = true;
            is_standing = false;
            is_chopping = false;
            if (direction.x > 0)
            {
                _srenderer.flipX = false;
                should_flip = false;
            }
            else if (direction.x < 0)
            {
                _srenderer.flipX = true;
                should_flip = true;
            }

            if (!_isRunning)
            {
                _isRunning = true;
                _playRunningSound = true;
            }
            _networkedInput._isChopping = false;
        }
        else if (direction.x != 0) //Chef is moving ONLY horizontally
        {
            _animator.SetBool(AnimStateList.IS_MOVING, true);
            _animator.SetBool(AnimStateList.IS_STANDING, false);
            _animator.SetBool(AnimStateList.IS_CHOPPING, false);
            is_moving = true;
            is_standing = false;
            is_chopping = false;
            if (direction.x > 0)
            {
                _srenderer.flipX = false;
                should_flip = false;
            }
            else if (direction.x < 0)
            {
                _srenderer.flipX = true;
                should_flip = true;
            }

            if (!_isRunning)
            {
                _isRunning = true;
                _playRunningSound = true;
            }
            _networkedInput._isChopping = false;
        }
        else //Chef is standing or chopping if not moving
        {
            _animator.SetBool(AnimStateList.IS_MOVING, false);
            _animator.SetBool(AnimStateList.IS_STANDING, true);
            _isRunning = false;
            is_moving = false;
            is_standing = true;
        }
        if (_networkedInput.isServer)
        {
            _networkedInput.RpcUpdateAnimation(is_standing, is_moving, should_flip);
        }
        else
        {
            _networkedInput.CmdUpdateAnimation(is_standing, is_moving, should_flip);
        }
    }
}
