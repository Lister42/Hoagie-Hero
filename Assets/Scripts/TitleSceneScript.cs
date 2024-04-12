using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;

public class TitleSceneScript : MonoBehaviour
{
    public GameObject singleLevels;
    public GameObject multiLevels;
    public GameObject numPlayers;
    public GameObject players;
    public GameObject multiSelect;
    public GameObject controls;
    public GameObject loadMessage;
    public Text loadText;

    public GameObject openControlsButton;
    public GameObject closeControlsButton;
    public GameObject singleplayerButton;
    public GameObject singleLevel1Button;
    public GameObject twoPlayersButton;
    public GameObject multiLevel1Button;
    public GameObject multiLocalSelectButton;

    public GameObject _mainScreenTileMap;
    public GameObject _levelSelectTileMap;
    public GameObject _numPlayersTileMap;
    public GameObject _controlsTileMap;
    public GameObject _controlBackgroundTileMap;
    public GameObject _multiSelectTileMap;

    private bool loading = false;
    private bool _replay;
    private AudioList _audioList;


    bool _host, _client;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(singleplayerButton);
        _replay = true;
        _audioList = GetComponent<AudioList>();
        PlayerPrefs.SetInt(TagList.SINGLE_PLAYER, 0);
    }


    public void CloseControls()
    {
        _audioList.PlayPlaceAudio();
        controls.SetActive(false);
        _mainScreenTileMap.SetActive(true);
        players.SetActive(true);
        _controlsTileMap.SetActive(false);
        _controlBackgroundTileMap.SetActive(false);
        EventSystem.current.SetSelectedGameObject(singleplayerButton);
    }

    public void OpenControls()
    {
        _audioList.PlayPlaceAudio();
        controls.SetActive(true);
        _mainScreenTileMap.SetActive(false);
        players.SetActive(false);
        _controlsTileMap.SetActive(true);
        _controlBackgroundTileMap.SetActive(true);
        EventSystem.current.SetSelectedGameObject(closeControlsButton);
    }

    public void SetNumPlayers(int players)
    {
        _audioList.PlayPlaceAudio();
        PlayerPrefs.SetInt(TagList.NUM_PLAYERS, players);
        numPlayers.SetActive(false);
        _numPlayersTileMap.SetActive(false);
        _levelSelectTileMap.SetActive(true);
        multiLevels.SetActive(true);
        EventSystem.current.SetSelectedGameObject(multiLevel1Button);
    }

    public void LoadLevel1Single()
    {
        if (loading) return;
        _audioList.PlayPlaceAudio();
        print("Loading Level 1 Single player");
        PlayerPrefs.SetInt(TagList.SINGLE_PLAYER, 1);
        PlayerPrefs.SetInt(TagList.NUM_PLAYERS, 2);
        loadMessage.SetActive(true);
        loading = true;
        StartCoroutine(LoadNewScene(TagList.LEVEL_1_1P));
    }

    public void LoadLevel2Single()
    {
        if (loading) return;
        _audioList.PlayPlaceAudio();
        print("Loading Level 2 Single player");
        PlayerPrefs.SetInt(TagList.SINGLE_PLAYER, 1);
        PlayerPrefs.SetInt(TagList.NUM_PLAYERS, 2);
        loadMessage.SetActive(true);
        loading = true;
        StartCoroutine(LoadNewScene(TagList.LEVEL_2_1P));
    }

    public void LoadLevel3Single()
    {
        if (loading) return;
        _audioList.PlayPlaceAudio();
        print("Loading Level 3 Single player");
        PlayerPrefs.SetInt(TagList.SINGLE_PLAYER, 1);
        PlayerPrefs.SetInt(TagList.NUM_PLAYERS, 2);
        loadMessage.SetActive(true);
        loading = true;
        StartCoroutine(LoadNewScene(TagList.LEVEL_3_1P));
    }

    public void LoadLevel1Multi()
    {
        if (loading) return;
        _audioList.PlayPlaceAudio();
        print("Loading Level 1 Multiplayer");
        PlayerPrefs.SetInt(TagList.SINGLE_PLAYER, 0);
        loadMessage.SetActive(true);
        loading = true;
        if (PlayerPrefs.GetInt(TagList.NUM_PLAYERS) == 2) StartCoroutine(LoadNewScene(TagList.LEVEL_1_2P));
        if (PlayerPrefs.GetInt(TagList.NUM_PLAYERS) == 3) StartCoroutine(LoadNewScene(TagList.LEVEL_1_3P));
        if (PlayerPrefs.GetInt(TagList.NUM_PLAYERS) == 4) StartCoroutine(LoadNewScene(TagList.LEVEL_1_4P));
    }

    public void LoadLevel2Multi()
    {
        if (loading) return;
        _audioList.PlayPlaceAudio();
        print("Loading Level 2 Multiplayer");
        PlayerPrefs.SetInt(TagList.SINGLE_PLAYER, 0);
        loadMessage.SetActive(true);
        loading = true;
        if (PlayerPrefs.GetInt(TagList.NUM_PLAYERS) == 2) StartCoroutine(LoadNewScene(TagList.LEVEL_2_2P));
        if (PlayerPrefs.GetInt(TagList.NUM_PLAYERS) == 3) StartCoroutine(LoadNewScene(TagList.LEVEL_2_3P));
        if (PlayerPrefs.GetInt(TagList.NUM_PLAYERS) == 4) StartCoroutine(LoadNewScene(TagList.LEVEL_2_4P));
    }
    public void LoadLevel3Multi()
    {
        if (loading) return;
        _audioList.PlayPlaceAudio();
        print("Loading Level 3 Multiplayer");
        PlayerPrefs.SetInt(TagList.SINGLE_PLAYER, 0);
        loadMessage.SetActive(true);
        loading = true;
        if (PlayerPrefs.GetInt(TagList.NUM_PLAYERS) == 2) StartCoroutine(LoadNewScene(TagList.LEVEL_3_2P));
        if (PlayerPrefs.GetInt(TagList.NUM_PLAYERS) == 3) StartCoroutine(LoadNewScene(TagList.LEVEL_3_3P));
        if (PlayerPrefs.GetInt(TagList.NUM_PLAYERS) == 4) StartCoroutine(LoadNewScene(TagList.LEVEL_3_4P));
    }



    public void Back()
    {
        _audioList.PlayPlaceAudio();
        players.SetActive(true);
        _mainScreenTileMap.SetActive(true);
        _levelSelectTileMap.SetActive(false);
        _controlBackgroundTileMap.SetActive(false);
        _controlsTileMap.SetActive(false);
        singleLevels.SetActive(false);
        multiLevels.SetActive(false);
        numPlayers.SetActive(false);
        multiSelect.SetActive(false);
        multiLevels.SetActive(false);
        _numPlayersTileMap.SetActive(false);
        _multiSelectTileMap.SetActive(false);
        EventSystem.current.SetSelectedGameObject(singleplayerButton);
    }

    private void Update()
    {
        if (loading)
        {
            loadText.color = new Color(loadText.color.r, loadText.color.g, loadText.color.b, Mathf.PingPong(Time.time, 1));
        }

        if (_replay)
        {
            _replay = false;
            StartCoroutine(TitleScreenMusic());
        }
    }

    IEnumerator LoadNewScene(string scene)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);

        while (!async.isDone)
        {
            yield return null;
        }
    }

    public void Singleplayer()
    {
        _audioList.PlayPlaceAudio();
        players.SetActive(false);
        _mainScreenTileMap.SetActive(false);
        _levelSelectTileMap.SetActive(true);
        singleLevels.SetActive(true);
        PlayerPrefs.SetInt(TagList.NETWORKED, 0);
        EventSystem.current.SetSelectedGameObject(singleLevel1Button);
    }

    public void Multiplayer()
    {
        _audioList.PlayPlaceAudio();
        players.SetActive(false);
        _mainScreenTileMap.SetActive(false);
        _multiSelectTileMap.SetActive(true);
        multiSelect.SetActive(true);
        PlayerPrefs.SetInt(TagList.NETWORKED, 0);
        EventSystem.current.SetSelectedGameObject(multiLocalSelectButton);
    }

    public void LocalMultiplayer()
    {
        _audioList.PlayPlaceAudio();
        _multiSelectTileMap.SetActive(false);
        multiSelect.SetActive(false);
        _numPlayersTileMap.SetActive(true);
        numPlayers.SetActive(true);
        PlayerPrefs.SetInt(TagList.NETWORKED, 0);
        EventSystem.current.SetSelectedGameObject(twoPlayersButton);
    }

    public void NetworkMultiplayer()
    {
        _audioList.PlayPlaceAudio();
        _multiSelectTileMap.SetActive(false);
        multiSelect.SetActive(false);
        PlayerPrefs.SetInt(TagList.NETWORKED, 1);
        loadMessage.SetActive(true);
        loading = true;
        StartCoroutine(LoadNewScene("NetworkingScene"));
    }

    private IEnumerator TitleScreenMusic()
    {
        print("Loading Title Screen Music");
        _audioList.PlayTitleScreenBackgroundMusic();
        yield return new WaitForSeconds(180);
        _replay = true;
    }

    public void Quit()
    {
        _audioList.PlayPlaceAudio();
        Application.Quit();
    }
}
