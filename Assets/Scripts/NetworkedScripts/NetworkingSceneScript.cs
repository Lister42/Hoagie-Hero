using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Net;
using Mirror;

public class NetworkingSceneScript : MonoBehaviour
{
    public GameObject _hostSelect;
    public GameObject _levelSelect;
    public GameObject _numPlayersSelect;
    public GameObject _clientSelect;

    public GameObject _levelOneButton;
    public GameObject _hostButton;
    public GameObject _2PlayerButton;
    public GameObject _inputField;

    public GameObject _hostSelectTileMap;
    public GameObject _levelSelectTileMap;
    public GameObject _clientTileMap;
    
    public GameObject loadMessage;
    public Text loadText;

    private AudioList _audioList;




    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JoinHost()
    {
        _hostSelect.SetActive(false);
        _hostSelectTileMap.SetActive(false);
        
        _levelSelectTileMap.SetActive(true);
        _numPlayersSelect.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_2PlayerButton);
    }

    public void LevelSelect(int level)
    {
        print("level " + level);
        PlayerPrefs.SetInt(TagList.LEVEL, level);

        NetworkManager.singleton.StartHost();
    }

    public void PlayerSelect(int numPlayers)
    {
        print("player select " + numPlayers);
        PlayerPrefs.SetInt(TagList.NUM_PLAYERS, numPlayers);

        _numPlayersSelect.SetActive(false);
        _levelSelect.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_levelOneButton);
    }

    public void JoinClient()
    {
        _hostSelect.SetActive(false);
        _hostSelectTileMap.SetActive(false);

        _clientSelect.SetActive(true);
        _clientTileMap.SetActive(true);


        EventSystem.current.SetSelectedGameObject(_inputField);
    }

    public void SetIP()
    {
        string ip = _inputField.GetComponent<InputField>().text;

        bool valid = IPAddress.TryParse(ip, out IPAddress ipAddress);

        if (valid)
        {
            NetworkManager.singleton.networkAddress = ip;
            NetworkManager.singleton.StartClient();
        }
        else
        {
            _inputField.GetComponent<InputField>().text = "";
        }
    }

    public void LevelBack()
    {
        _levelSelect.SetActive(false);
        _numPlayersSelect.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_2PlayerButton);
    }

    public void Back()
    {
        _levelSelect.SetActive(false);
        _numPlayersSelect.SetActive(false);
        _levelSelectTileMap.SetActive(false);
        _clientSelect.SetActive(false);
        _clientTileMap.SetActive(false);

        _hostSelect.SetActive(true);
        _hostSelectTileMap.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_hostButton);
    }

    public void TitleBack()
    {
        SceneManager.LoadScene("TitleScene");
    }

    IEnumerator LoadNewScene(string scene)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);

        while (!async.isDone)
        {
            yield return null;
        }
    }    
}