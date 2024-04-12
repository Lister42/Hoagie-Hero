using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderScript : MonoBehaviour
{
    public Order _order;

    public Image _hoagieImage;
    public Image _ingredient1;
    public Image _ingredient2;
    public Image _ingredient3;
    public Image _ingredient4;

    public Text _hoagieName;

    public Sprite H;
    public Sprite HC;
    public Sprite HLT;
    public Sprite BLT;
    public Sprite MH;

    public Sprite bacon;
    public Sprite meatballs;
    public Sprite tomato_sauce;
    public Sprite ham;
    public Sprite lettuce;
    public Sprite tomato;
    public Sprite cheese;
    public Sprite toasted;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetOrder(Order o)
    {

        if (!_ingredient2.enabled)
            _ingredient2.enabled = true;
        if (!_ingredient3.enabled)
            _ingredient3.enabled = true;
        if (!_ingredient4.enabled)
            _ingredient4.enabled = true;

        _order = o;
        

        if (_order.level == 1) //Level 1 Hoagies
        {
            
            _hoagieImage.sprite = _order.type switch
            {
                0 => H,
                1 => HC,
                2 => HLT,
                3 => BLT,
                _ => H
            };
            _hoagieName.text = _order.type switch
            {
                0 => "Ham Hoagie",
                1 => "Cheese Hoagie",
                2 => "Ham Deluxe",
                3 => "BLT BEGGER",
                _ => "Ham Hoagie"
            };
            _ingredient1.sprite = _order.type switch
            {
                3 => bacon,
                _ => ham
            };
            _ingredient2.sprite = _order.type switch
            {
                0 => null,
                1 => cheese,
                _ => lettuce
            };
            _ingredient3.sprite = _order.type switch
            {
                0 => null,
                1 => null,
                _ => tomato
            };
            _ingredient4.sprite = _order.grilled switch
            {
                true => toasted,
                false => null
            };
        }

        else if (_order.level == 2)
        {
            _hoagieImage.sprite = _order.type switch
            {
                0 => H,
                1 => HC,
                2 => HLT,
                3 => MH,
                _ => H
            };
            _hoagieName.text = _order.type switch
            {
                0 => "Ham Hoagie",
                1 => "Cheese Hoagie",
                2 => "Ham Deluxe",
                3 => "Meatball Hero",
                _ => "Ham Hoagie"
            };
            _ingredient1.sprite = _order.type switch
            {
                3 => meatballs,
                _ => ham
            };
            _ingredient2.sprite = _order.type switch
            {
                0 => null,
                1 => cheese,
                3 => tomato_sauce,
                _ => lettuce
            };
            _ingredient3.sprite = _order.type switch
            {
                2 => tomato,
                _ => null
            };
            _ingredient4.sprite = _order.grilled switch
            {
                true => toasted,
                false => null
            };
        }
        else if (_order.level == 3)
        {
            _hoagieImage.sprite = _order.type switch
            {
                0 => H,
                1 => HC,
                2 => HLT,
                3 => BLT,
                4 => MH,
                _ => H
            };
            _hoagieName.text = _order.type switch
            {
                0 => "Ham Hoagie",
                1 => "Cheese Hoagie",
                2 => "Ham Deluxe",
                3 => "BLT BEGGER",
                4 => "Meatball Hero",
                _ => "Ham Hoagie"
            };
            _ingredient1.sprite = _order.type switch
            {
                3 => bacon,
                4 => meatballs,
                _ => ham
            };
            _ingredient2.sprite = _order.type switch
            {
                0 => null,
                1 => cheese,
                4 => tomato_sauce,
                _ => lettuce
            };
            _ingredient3.sprite = _order.type switch
            {
                2 => tomato,
                3 => tomato,
                _ => null
            };
            _ingredient4.sprite = _order.grilled switch
            {
                true => toasted,
                false => null
            };
        }

        if (!_ingredient2.sprite)
            _ingredient2.enabled = false;
        if (!_ingredient3.sprite)
            _ingredient3.enabled = false;
        if (!_ingredient4.sprite)
            _ingredient4.enabled = false;

        if (_order.grilled)
        {
            _hoagieImage.color = new(.7f, .7f, .7f);
        }
        else
        {
            _hoagieImage.color = Color.white;
        }
    }
}
