using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefsRunningScript : MonoBehaviour
{
    public GameObject _chefOnePrefab;
    public GameObject _chefTwoPrefab;
    public GameObject _food;

    public Sprite _bacon;
    public Sprite _bread;
    public Sprite _cheese;
    public Sprite _cheeseSlice;
    public Sprite _lettuce;
    public Sprite _cutLettuce;
    public Sprite _tomato;
    public Sprite _cutTomato;
    public Sprite _meatballs;
    public Sprite _meatlballHero;
    public Sprite _ham;
    public Sprite _cutHam;
    public Sprite _pan;
    public Sprite _pot;
    public Sprite _BLT;
    public Sprite _hamHoagie;
    public Sprite _hamCheeseHoagie;
    public Sprite _hamDeluxe;
    //private bool _replay;
    // Start is called before the first frame update
    void Start()
    {
        //_replay = true;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject chef = GetRandomChef();
        if (Random.Range(0, 200) == 0)
        {
            if (Random.Range(0, 2) == 0)
            {
                GameObject chefTopLeft = Instantiate(chef, new Vector3(-10, 0, 0), Quaternion.identity);
                chefTopLeft.transform.GetChild(4).gameObject.SetActive(false);
                chefTopLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(6, 0);
                GameObject food = GetRandomFood(chefTopLeft);
                food.transform.position = chefTopLeft.transform.position;
                food.transform.SetParent(chefTopLeft.transform);
                food.transform.position = new Vector2(chefTopLeft.transform.position.x, chefTopLeft.transform.position.y + 0.75f);
                food.GetComponent<SpriteRenderer>().sortingOrder = 2;
                chefTopLeft.GetComponent<SpriteRenderer>().sortingOrder = 1;
                if (!food.CompareTag(TagList.PAN)) food.transform.localScale = new Vector3(1, 1, 0);

            }
        }

        if (Random.Range(0, 200) == 0)
        {
            if (Random.Range(0, 2) == 0)
            {
                GameObject chefBottomRight = Instantiate(chef, new Vector3(10, -1.2f, 0), Quaternion.identity);
                chefBottomRight.transform.GetChild(4).gameObject.SetActive(false);
                chefBottomRight.GetComponent<Rigidbody2D>().velocity = new Vector2(-6, 0);
                GameObject food = GetRandomFood(chefBottomRight);
                food.transform.position = chefBottomRight.transform.position;
                food.transform.SetParent(chefBottomRight.transform);
                food.transform.position = new Vector2(chefBottomRight.transform.position.x, chefBottomRight.transform.position.y + 0.75f);
                food.GetComponent<SpriteRenderer>().sortingOrder = 4;
                chefBottomRight.GetComponent<SpriteRenderer>().sortingOrder = 3;
                if (!food.CompareTag(TagList.PAN)) food.transform.localScale = new Vector3(1, 1, 0);
            }
        }
    }

    private GameObject GetRandomChef()
    {
        GameObject chef;
        switch (Random.Range(0, 2))
        {
            case 0:
                chef = _chefOnePrefab;
                break;
            case 1:
                chef = _chefTwoPrefab;
                break;
            default:
                chef = _chefOnePrefab;
                break;
        }
        return chef;
    }

    private GameObject GetRandomFood(GameObject chef)
    {
        GameObject food = Instantiate(_food, chef.transform.position, Quaternion.identity);
        Sprite sprite;
        switch (Random.Range(0, 18))
        {
            case 0:
                sprite = _bacon;
                break;
            case 1:
                sprite = _bread;
                break;
            case 2:
                sprite = _cheese;
                break;
            case 3:
                sprite = _cheeseSlice;
                break;
            case 4:
                sprite = _lettuce;
                break;
            case 5:
                sprite = _cutLettuce;
                break;
            case 6:
                sprite = _tomato;
                break;
            case 7:
                sprite = _cutTomato;
                break;
            case 8:
                sprite = _meatballs;
                break;
            case 9:
                sprite = _meatlballHero;
                break;
            case 10:
                sprite = _ham;
                break;
            case 11:
                sprite = _cutHam;
                break;
            case 12:
                sprite = _pan;
                food.tag = TagList.PAN;
                break;
            case 13:
                sprite = _pot;
                break;
            case 14:
                sprite = _BLT;
                break;
            case 15:
                sprite = _hamHoagie;
                break;
            case 16:
                sprite = _hamCheeseHoagie;
                break;
            case 17:
                sprite = _hamDeluxe;
                break;
            default:
                sprite = _meatlballHero;
                break;
        }
        food.GetComponent<SpriteRenderer>().sprite = sprite;
        return food;
    }
}
