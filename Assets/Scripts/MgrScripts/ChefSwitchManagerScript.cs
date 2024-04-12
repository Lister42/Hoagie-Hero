using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefSwitchManagerScript : MonoBehaviour
{
    public GameObject chefOne;
    public GameObject chefTwo;

    public string _currentChef = TagList.CHEF_ONE;
    public SpriteRenderer _chefOneSelector;
    public SpriteRenderer _chefTwoSelector;

    private LevelManagerScript _managerScript;

    public Color _selectorColor = new (1, 117 / 255f, 59 / 255f, 204 / 255f);
    public Color _deselectColor = new (0, 0, 0, 0);

    // Start is called before the first frame update
    void Start()
    { 
        if(PlayerCreatorScript._instance._networked)
            return;        
        var players = PlayerCreatorScript._instance._players;

        //chefOne = GameObject.FindGameObjectWithTag(TagList.CHEF_ONE);
        //chefTwo = GameObject.FindGameObjectWithTag(TagList.CHEF_TWO);

        //print(players[0] + ", " + players[1]);
        chefOne = players[0];
        chefTwo = players[1];

        print(chefOne + ", " + chefTwo);

        _chefOneSelector = chefOne.transform.GetChild(4).GetComponent<SpriteRenderer>();
        _chefTwoSelector = chefTwo.transform.GetChild(4).GetComponent<SpriteRenderer>();
        if(PlayerCreatorScript._instance._singlePlayer)
            _chefTwoSelector.color = _deselectColor;

        _managerScript = GetComponent<LevelManagerScript>();
    }

    public string CurrentChef()
    {
        return _currentChef;
    }

    public GameObject GetChefOne()
    {
        return chefOne;
    }

    public GameObject GetChefTwo()
    {
        return chefTwo;
    }

    public int GetChefIndex()
    {
        if (_currentChef.Equals(TagList.CHEF_ONE))
        {
            return 0;
        }

        if (_currentChef.Equals(TagList.CHEF_TWO))
        {
            return 1;
        }

        return -1; //If neither are an option
    }
}
