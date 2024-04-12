using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Mirror;

[RequireComponent(typeof(AudioList))]
public class LevelManagerScript : MonoBehaviour
{
    //public GameObject _chefOneSelected;
    //public GameObject _chefTwoSelected;

    public GameObject[] _chefsSelected;

    public GameObject _replayButton;
    public GameObject _resumeButton;
    public GameObject _scoring;
    public Image _bar;
    public int numChefs;
    public double _startTime;
    double _endTime;
    public Text _timerText;
    public Text _finalTimeText;
    public Text _bestTimeText;
    Color timerColor = new Color(244 / 244f, 160 / 255f, 57 / 255f, 1);
    public bool _paused = false;
    public GameObject _pausePanel;
    public Text _readyText;
    public Text _setText;
    public Text _heroText;
    public GameObject _readyTileMap;
    public GameObject _setTileMap;
    private bool _begin;
    private AudioList _audioList;
    private bool _isOver;

    bool _networked;

    #region Current Level Booleans
    public bool _isLevelOne_1P = false;
    public bool _isLevelTwo_1P = false;
    public bool _isLevelThree_1P = false;
    public bool _isLevelOne_2P = false;
    public bool _isLevelTwo_2P = false;
    public bool _isLevelThree_2P = false;
    public bool _isLevelOne_3P = false;
    public bool _isLevelTwo_3P = false;
    public bool _isLevelThree_3P = false;
    public bool _isLevelOne_4P = false;
    public bool _isLevelTwo_4P = false;
    public bool _isLevelThree_4P = false;
    #endregion

    int level;

    // Start is called before the first frame update
    void Start()
    {
        _networked = PlayerPrefs.GetInt(TagList.NETWORKED, 0) == 1;
        if (!_networked)
        {
            _startTime = Time.time;
        }

        _begin = false;
        _isOver = false;
        StartCoroutine(EpicIntro());

        _audioList = GetComponent<AudioList>();
        print(_audioList + " this is the audio list");
        _chefsSelected = new GameObject[PlayerCreatorScript._instance._playerCount];
        
        DetermineLevel();
        PlayLevelMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (_begin)
        {
        var time = (_networked ? NetworkTime.time : Time.time) - _startTime; // need to subtract from _startTime
        int m = (int)(time / 60);
        int s = (int)time % 60;
        int ms = (int)((time % 60) * 100) % 100;
        _timerText.text = s < 10 ? m + ":0" + s + (ms < 10 ? ".0" + ms: "." + ms) : m + ":" + s + (ms < 10 ? ".0" + ms : "." + ms);
        } else
        {
            if(!_networked)
                _startTime = Time.time;
        }
    }

    public void Over()
    {
        _audioList.Stop();
        _audioList.PlayEndSceneBackgroundMusic();
        _isOver = true;
        _endTime = _networked ? NetworkTime.time : Time.time;
        var time = _endTime - _startTime;


        print(time);

        var record = PlayerPrefs.GetFloat(_networked.ToString() + "Level" + level + "Score" + numChefs + PlayerCreatorScript._instance._singlePlayer, float.MaxValue);
        if (time < record)
        {
            PlayerPrefs.SetFloat(_networked.ToString() + "Level" + level + "Score" + numChefs + PlayerCreatorScript._instance._singlePlayer, (float)time);
            record = (float)time;
        }

        int m = (int)(record / 60);
        int s = (int)record % 60;
        _bestTimeText.text = s < 10 ? m + ":0" + s : m + ":" + s;

        _finalTimeText.text = _timerText.text;
        _scoring.gameObject.SetActive(true);

        DetermineScore((float)time);

        EventSystem.current.SetSelectedGameObject(_replayButton);

        Time.timeScale = 0;
    }
    public void NextLevel()
    {
        _audioList.PlayPlaceAudio();
        Time.timeScale = 1;
        if (_isLevelOne_1P || _isLevelOne_2P) LoadScene("Level2Scene");
        else if (_isLevelOne_3P) LoadScene("Level2_3P_Scene");
        else if (_isLevelOne_4P) LoadScene("Level2_4P_Scene");
        else if (_isLevelTwo_1P || _isLevelTwo_2P) LoadScene("Level3Scene");
        else if (_isLevelTwo_3P) LoadScene("Level3_3P_Scene");
        else if (_isLevelTwo_4P) LoadScene("Level3_4P_Scene");
        else return;
    }

    public void RestartLevel1()
    {
        _audioList.PlayPlaceAudio();
        Time.timeScale = 1;
        if (_isLevelOne_1P || _isLevelOne_2P) LoadScene("Level1Scene"); //this is because level 1 is the same for both single player and two player multi player
        else if (_isLevelOne_3P) LoadScene("Level1_3P_Scene");
        else if (_isLevelOne_4P) LoadScene("Level1_4P_Scene");
    }


    public void RestartLevel2()
    {
        _audioList.PlayPlaceAudio();
        Time.timeScale = 1;
        if (_isLevelTwo_1P || _isLevelTwo_2P) LoadScene("Level2Scene");
        else if (_isLevelTwo_3P) LoadScene("Level2_3P_Scene");
        else if (_isLevelTwo_4P) LoadScene("Level2_4P_Scene");
    }

    public void RestartLevel3()
    {
        _audioList.PlayPlaceAudio();
        Time.timeScale = 1;
        if (_isLevelThree_1P || _isLevelThree_2P) LoadScene("Level3Scene");
        else if (_isLevelThree_3P) LoadScene("Level3_3P_Scene");
        else if (_isLevelThree_4P) LoadScene("Level3_4P_Scene");
    }

    void LoadScene(string scene)
    {
        //Debug.LogError("Loading scene: " + scene);
        if (_networked)
        {
            //Debug.LogError("Loading networked scene scene: " + scene);
            NetworkManager.singleton.ServerChangeScene(scene);
        }
        else
        {
            SceneManager.LoadScene(scene);
        }
    }

    public void Menu()
    {
        _audioList.PlayPlaceAudio();
        Time.timeScale = 1;
        if (_networked)
        {
            var man = FindObjectOfType<NetManScript>();
            if(FindObjectOfType<NetworkedLevelManager>().isServer)
                man.StopHost();
            else 
                man.StopClient();
        }
        SceneManager.LoadScene("TitleScene");
    }

    public void Pause()
    {
        if (!_isOver) {
            if(_paused)
            {
                _audioList.PlayPlaceAudio();
                Time.timeScale = 1;
                _pausePanel.SetActive(false);
            }
            else
            {
                _audioList.PlayPlaceAudio();
                EventSystem.current.SetSelectedGameObject(_resumeButton);
                Time.timeScale = 0;
                _pausePanel.SetActive(true);
            }

            _paused = !_paused;
        }
    }

    public IEnumerator TimingPenalty()
    {
        _timerText.color = Color.red;
        yield return new WaitForSeconds(2);
        _timerText.color = timerColor;
    }

    //Do differently
    public void DetermineLevel()
    {
        int numPlayers = PlayerCreatorScript._instance._playerCount;
        switch (name)
        {
            case TagList.LEVEL_ONE:
                switch (numPlayers)
                {
                    case 1:
                        _isLevelOne_1P = true;
                        break;
                    case 2:
                        _isLevelOne_2P = true;
                        break;
                    case 3:
                        _isLevelOne_3P = true;
                        break;
                    case 4:
                        _isLevelOne_4P = true;
                        break;
                }
                level = 1;
                break;
            case TagList.LEVEL_TWO:
                switch (numPlayers)
                {
                    case 1:
                        _isLevelTwo_1P = true;
                        break;
                    case 2:
                        _isLevelTwo_2P = true;
                        break;
                    case 3:
                        _isLevelTwo_3P = true;
                        break;
                    case 4:
                        _isLevelTwo_4P = true;
                        break;
                }
                level = 2;
                break;
            case TagList.LEVEL_THREE:
                switch (numPlayers)
                {
                    case 1:
                        _isLevelThree_1P = true;
                        break;
                    case 2:
                        _isLevelThree_2P = true;
                        break;
                    case 3:
                        _isLevelThree_3P = true;
                        break;
                    case 4:
                        _isLevelThree_4P = true;
                        break;
                }
                level = 3;
                break;
            default:
                print("Invalid level manager");
                level = 0;
                break;
        }
    }

    public void DetermineScore(float time)
    {
        // === SINGLE PLAYER === //
        if (_isLevelOne_1P)
        {
            if
                (GameBalanceList.LEVEL1_1P_BEST_TIME / time >= 1) _bar.transform.localScale = new Vector2(0, 0);
            else
                _bar.transform.localScale = new Vector2(1 - GameBalanceList.LEVEL1_1P_BEST_TIME / time, 1);
        }

        else if (_isLevelTwo_1P)
        {
            if
                (GameBalanceList.LEVEL2_1P_BEST_TIME / time >= 1) _bar.transform.localScale = new Vector2(0, 0);
            else
                _bar.transform.localScale = new Vector2(1 - GameBalanceList.LEVEL2_1P_BEST_TIME / time, 1);
        }

        else if (_isLevelThree_1P)
        {
            if
                (GameBalanceList.LEVEL3_1P_BEST_TIME / time >= 1) _bar.transform.localScale = new Vector2(0, 0);
            else
                _bar.transform.localScale = new Vector2(1 - GameBalanceList.LEVEL3_1P_BEST_TIME / time, 1);
        }
        // === END === //

        // === 2 PLAYERS ===/
        else if (_isLevelOne_2P)
        {
            if
                (GameBalanceList.LEVEL1_2P_BEST_TIME / time >= 1) _bar.transform.localScale = new Vector2(0, 0);
            else
                _bar.transform.localScale = new Vector2(1 - GameBalanceList.LEVEL1_2P_BEST_TIME / time, 1);
        }

        else if (_isLevelTwo_2P)
        {
            if
                (GameBalanceList.LEVEL2_2P_BEST_TIME / time >= 1) _bar.transform.localScale = new Vector2(0, 0);
            else
                _bar.transform.localScale = new Vector2(1 - GameBalanceList.LEVEL2_2P_BEST_TIME / time, 1);
        }

        else if (_isLevelThree_2P)
        {
            if
                (GameBalanceList.LEVEL3_2P_BEST_TIME / time >= 1) _bar.transform.localScale = new Vector2(0, 0);
            else
                _bar.transform.localScale = new Vector2(1 - GameBalanceList.LEVEL3_2P_BEST_TIME / time, 1);
        }
        // === END === //

        // === 3 PLAYERS ===//
        else if (_isLevelOne_3P)
        {
            if
                (GameBalanceList.LEVEL1_3P_BEST_TIME / time >= 1) _bar.transform.localScale = new Vector2(0, 0);
            else
                _bar.transform.localScale = new Vector2(1 - GameBalanceList.LEVEL1_3P_BEST_TIME / time, 1);
        }

        else if (_isLevelTwo_3P)
        {
            if
                (GameBalanceList.LEVEL2_3P_BEST_TIME / time >= 1) _bar.transform.localScale = new Vector2(0, 0);
            else
                _bar.transform.localScale = new Vector2(1 - GameBalanceList.LEVEL2_3P_BEST_TIME / time, 1);
        }

        else if (_isLevelThree_3P)
        {
            if
                (GameBalanceList.LEVEL3_3P_BEST_TIME / time >= 1) _bar.transform.localScale = new Vector2(0, 0);
            else
                _bar.transform.localScale = new Vector2(1 - GameBalanceList.LEVEL3_3P_BEST_TIME / time, 1);
        }
        // === END === //

        // === 4 PLAYERS === //
        else if (_isLevelOne_4P)
        {
            if
                (GameBalanceList.LEVEL1_4P_BEST_TIME / time >= 1) _bar.transform.localScale = new Vector2(0, 0);
            else
                _bar.transform.localScale = new Vector2(1 - GameBalanceList.LEVEL1_4P_BEST_TIME / time, 1);
        }

        else if (_isLevelTwo_4P)
        {
            if
                (GameBalanceList.LEVEL2_4P_BEST_TIME / time >= 1) _bar.transform.localScale = new Vector2(0, 0);
            else
                _bar.transform.localScale = new Vector2(1 - GameBalanceList.LEVEL2_4P_BEST_TIME / time, 1);
        }

        else if (_isLevelThree_4P)
        {
            if
                (GameBalanceList.LEVEL3_4P_BEST_TIME / time >= 1) _bar.transform.localScale = new Vector2(0, 0);
            else
                _bar.transform.localScale = new Vector2(1 - GameBalanceList.LEVEL3_4P_BEST_TIME / time, 1);
        }
        // === END === //
    }

    private IEnumerator EpicIntro()
    {
        print("in coroutine");
        _readyText.gameObject.SetActive(true);
        _readyTileMap.SetActive(true);
        yield return new WaitForSeconds(.7f);
        _readyText.gameObject.SetActive(false);
        _readyTileMap.SetActive(false);
        _setText.gameObject.SetActive(true);
        _setTileMap.SetActive(true);
        yield return new WaitForSeconds(.9f);
        _setText.gameObject.SetActive(false);
        _setTileMap.SetActive(false);
        _heroText.gameObject.SetActive(true);
        _begin = true;
        yield return new WaitForSeconds(2f);
        _heroText.gameObject.SetActive(false);
        print("done coroutine");
    }

    private void PlayLevelMusic()
    {
        if (_isLevelOne_1P || _isLevelOne_2P || _isLevelOne_3P || _isLevelOne_4P)
        {
            _audioList.PlayLevelOneBackgroundMusic();
        }
        if (_isLevelTwo_1P || _isLevelTwo_2P || _isLevelTwo_3P || _isLevelTwo_4P)
        {
            _audioList.PlayLevelTwoBackgroundMusic();
        }
        if (_isLevelThree_1P || _isLevelThree_2P || _isLevelThree_3P || _isLevelThree_4P)
        {
            _audioList.PlayLevelThreeBackgroundMusic();
        }
    }
}
