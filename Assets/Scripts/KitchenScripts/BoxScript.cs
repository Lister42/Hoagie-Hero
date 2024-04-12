using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BoxScript : MonoBehaviour
{
    public GameObject _item;

    CounterScript _counterScript;
    LevelManagerScript _managerScript;
    AudioList _audioList;

    SpriteRenderer _srend;
    Transform _transform;

    public bool _networked;

    public bool _pickingUp = false;

    // Start is called before the first frame update
    void Start()
    {
        _networked = PlayerCreatorScript._instance._networked;
        _audioList = GameObject.FindGameObjectWithTag(TagList.MANAGER).GetComponent<AudioList>();

        _managerScript = GameObject.FindGameObjectWithTag(TagList.MANAGER).GetComponent<LevelManagerScript>();
        _counterScript = GetComponent<CounterScript>();
        _srend = GetComponent<SpriteRenderer>();
        _transform = transform;
    }

    public void Pickup(int player)
    {
        var chef = _counterScript.chefs[player];
        if (chef == null)
        {
            Debug.LogError("Chef " + player + "is null picking up from box");
            return;
        }
        if (!chef.GetChefMovementScript()._item) // check if he's holding something
        {
            _audioList.PlayPickupAudio();
            var item = Instantiate(_item, chef.GetTransform());
            item.transform.position = new Vector2(item.transform.position.x, item.transform.position.y + 0.8f);

            var netLevelMan = FindObjectOfType<NetworkedLevelManager>();
            if (netLevelMan)
            {
                if (item.GetComponent<ItemScript>())
                {
                    item.GetComponent<ItemScript>()._hash = netLevelMan._itemCount;
                    netLevelMan._itemCount++;
                }
            }

            item.GetComponent<SpriteRenderer>().sortingOrder = 2;
            chef.GetChefMovementScript()._item = item;
            _pickingUp = true;
            StartCoroutine(Wait()); //wait so he doens't put down the item in the same frame
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.1f);
        _pickingUp = false;
    }
}
