using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class OrderManagerScript : MonoBehaviour
{
    public List<Order> _orders = new List<Order>();
    public OrderScript _order1;
    public OrderScript _order2;
    public OrderScript _order3;
    public int _remaining = 3;

    LevelManagerScript _levelManager;

    bool _done;

    public Text _orderText;

    int _ordersLeft = 7;
    

    void Start()
    {
        _levelManager = GetComponent<LevelManagerScript>();


        if (PlayerCreatorScript._instance._networked) return;

        CreateOrders();


        //Randomize order list
        var l = _orders.OrderBy(item => Random.value);

        _orders = new List<Order>();

        foreach (var item in l)
        {
            _orders.Add(item);
        }
        _order1.SetOrder(_orders[0]);
        _order2.SetOrder(_orders[1]);
        _order3.SetOrder(_orders[2]);
        _orders.RemoveRange(0, 3);
    }

    // Update is called once per frame
    void Update()
    {
        if(_done) return;

        if (_remaining <= 0)
        {
            _levelManager.Over();
            _remaining = 3;
        }   
    }

    public bool CorrectOrder(BreadScript bread)
    {
        int type = -1;

        if (_levelManager._isLevelOne_1P || _levelManager._isLevelOne_2P || _levelManager._isLevelOne_3P || _levelManager._isLevelOne_4P)
        {
            if (bread.ham && !bread.cheese && !bread.tomato && !bread.lettuce && !bread.bacon)
                type = 0;
            if (bread.ham && bread.cheese && !bread.tomato && !bread.lettuce && !bread.bacon)
                type = 1;
            if (bread.ham && !bread.cheese && bread.tomato && bread.lettuce && !bread.bacon)
                type = 2;
            if (!bread.ham && !bread.cheese && bread.tomato && bread.lettuce && bread.bacon)
                type = 3;

            if (type < 0)
            {
                return false;
            }

            if (_order1._order.type == type && _order1._order.grilled == bread.grilled)
            {
                OrderDone(_order1);
                return true;
            }
            else if (_order2._order.type == type && _order2._order.grilled == bread.grilled)
            {
                OrderDone(_order2);
                return true;
            }
            else if (_order3._order.type == type && _order3._order.grilled == bread.grilled)
            {
                OrderDone(_order3);
                return true;
            }
        }
        else if (_levelManager._isLevelTwo_1P || _levelManager._isLevelTwo_2P || _levelManager._isLevelTwo_3P || _levelManager._isLevelTwo_4P)
        {
            if (bread.ham && !bread.cheese && !bread.tomato && !bread.lettuce && !bread.meatballs && !bread.tomato_sauce)
                type = 0;
            if (bread.ham && bread.cheese && !bread.tomato && !bread.lettuce && !bread.meatballs && !bread.tomato_sauce)
                type = 1;
            if (bread.ham && !bread.cheese && bread.tomato && bread.lettuce && !bread.meatballs && !bread.tomato_sauce)
                type = 2;
            if (!bread.ham && !bread.cheese && !bread.tomato && !bread.lettuce  && bread.meatballs && bread.tomato_sauce)
                type = 3;

            if (_order1._order.type == type && _order1._order.grilled == bread.grilled)
            {
                OrderDone(_order1);
                return true;
            }
            else if (_order2._order.type == type && _order2._order.grilled == bread.grilled)
            {
                OrderDone(_order2);
                return true;
            }
            else if (_order3._order.type == type && _order3._order.grilled == bread.grilled)
            {
                OrderDone(_order3);
                return true;
            }
        }
        else if (_levelManager._isLevelThree_1P || _levelManager._isLevelThree_2P || _levelManager._isLevelThree_3P || _levelManager._isLevelThree_4P)
        {
            if (bread.ham && !bread.cheese && !bread.tomato && !bread.lettuce && !bread.meatballs && !bread.tomato_sauce && !bread.bacon)
                type = 0;
            if (bread.ham && bread.cheese && !bread.tomato && !bread.lettuce && !bread.meatballs && !bread.tomato_sauce && !bread.bacon)
                type = 1;
            if (bread.ham && !bread.cheese && bread.tomato && bread.lettuce && !bread.meatballs && !bread.tomato_sauce && !bread.bacon)
                type = 2;
            if (!bread.ham && !bread.cheese && bread.tomato && bread.lettuce && !bread.meatballs && !bread.tomato_sauce && bread.bacon)
                type = 3;
            if (!bread.ham && !bread.cheese && !bread.tomato && !bread.lettuce && bread.meatballs && bread.tomato_sauce && !bread.bacon)
                type = 4;
            if (_order1._order.type == type && _order1._order.grilled == bread.grilled)
            {
                OrderDone(_order1);
                return true;
            }
            else if (_order2._order.type == type && _order2._order.grilled == bread.grilled)
            {
                OrderDone(_order2);
                return true;
            }
            else if (_order3._order.type == type && _order3._order.grilled == bread.grilled)
            {
                OrderDone(_order3);
                return true;
            }
        }


        return false;
    }

    void OrderDone(OrderScript orderScript)
    {
        if (_orders.Count > 0)
        {
            orderScript.SetOrder(_orders[0]);
            _orders.RemoveAt(0);
        }
        else
        {
            orderScript.gameObject.SetActive(false);
            _remaining--;
        }

        _ordersLeft--;

        _orderText.text = "ORDERS: " + _ordersLeft;
    }

    public void CreateOrders()
    {
        // === Level 1 Orders === //
        if (_levelManager._isLevelOne_1P)
        {
            _orders.Add(new(0, false, 1));
            _orders.Add(new(1, true, 1));
            _orders.Add(new(2, false, 1));
            _orders.Add(new(1, true, 1));
            _orders.Add(new(2, false, 1));
            _orders.Add(new(0, true, 1));
            _orders.Add(new(3, false, 1));
        }
        else if (_levelManager._isLevelOne_2P)
        {
            _orders.Add(new(0, false, 1));
            _orders.Add(new(1, true, 1));
            _orders.Add(new(2, false, 1));
            _orders.Add(new(1, true, 1));
            _orders.Add(new(2, false, 1));
            _orders.Add(new(0, true, 1));
            _orders.Add(new(3, false, 1));
        }
        else if (_levelManager._isLevelOne_3P)
        {
            _orders.Add(new(1, false, 1));
            _orders.Add(new(1, true, 1));
            _orders.Add(new(1, true, 1));
            _orders.Add(new(2, false, 1));
            _orders.Add(new(2, true, 1));
            _orders.Add(new(3, false, 1));
            _orders.Add(new(3, true, 1));
        }
        else if (_levelManager._isLevelOne_4P)
        {
            _orders.Add(new(1, false, 1));
            _orders.Add(new(1, true, 1));
            _orders.Add(new(2, false, 1));
            _orders.Add(new(2, true, 1));
            _orders.Add(new(3, false, 1));
            _orders.Add(new(3, true, 1));
            _orders.Add(new(3, true, 1));
        }
        // === END === //

        // === Level 2 Orders === //
        else if (_levelManager._isLevelTwo_1P)
        {
            _orders.Add(new(0, true, 2));
            _orders.Add(new(1, false, 2));
            _orders.Add(new(1, true, 2));
            _orders.Add(new(2, false, 2));
            _orders.Add(new(2, true, 2));
            _orders.Add(new(3, false, 2));
            _orders.Add(new(3, false, 2));
        }
        else if (_levelManager._isLevelTwo_2P)
        {
            _orders.Add(new(0, true, 2));
            _orders.Add(new(1, false, 2));
            _orders.Add(new(1, true, 2));
            _orders.Add(new(2, false, 2));
            _orders.Add(new(2, true, 2));
            _orders.Add(new(3, false, 2));
            _orders.Add(new(3, false, 2));
        }
        else if (_levelManager._isLevelTwo_3P)
        {
            _orders.Add(new(1, true, 2));
            _orders.Add(new(1, false, 2));
            _orders.Add(new(2, true, 2));
            _orders.Add(new(2, false, 2));
            _orders.Add(new(3, false, 2));
            _orders.Add(new(3, false, 2));
            _orders.Add(new(3, true, 2));
        }
        else if (_levelManager._isLevelTwo_4P)
        {
            _orders.Add(new(1, true, 2));
            _orders.Add(new(1, false, 2));
            _orders.Add(new(2, true, 2));
            _orders.Add(new(2, false, 2));
            _orders.Add(new(3, false, 2));
            _orders.Add(new(3, false, 2));
            _orders.Add(new(3, true, 2));
        }
        // === END === //

        // === Level 3 Orders === //
        else if (_levelManager._isLevelThree_1P)
        {
            _orders.Add(new(0, true, 3));
            _orders.Add(new(1, true, 3));
            _orders.Add(new(2, false, 3));
            _orders.Add(new(2, true, 3));
            _orders.Add(new(3, false, 3));
            _orders.Add(new(3, true, 3));
            _orders.Add(new(4, false, 3));
        }
        else if (_levelManager._isLevelThree_2P)
        {
            _orders.Add(new(0, true, 3));
            _orders.Add(new(1, true, 3));
            _orders.Add(new(2, false, 3));
            _orders.Add(new(2, true, 3));
            _orders.Add(new(3, false, 3));
            _orders.Add(new(3, true, 3));
            _orders.Add(new(4, false, 3));
        }
        else if (_levelManager._isLevelThree_3P)
        {
            _orders.Add(new(4, true, 3));
            _orders.Add(new(3, false, 3));
            _orders.Add(new(2, false, 3));
            _orders.Add(new(2, true, 3));
            _orders.Add(new(3, false, 3));
            _orders.Add(new(3, true, 3));
            _orders.Add(new(4, false, 3));
        }
        else if (_levelManager._isLevelThree_4P)
        {
            _orders.Add(new(4, true, 3));
            _orders.Add(new(3, false, 3));
            _orders.Add(new(2, false, 3));
            _orders.Add(new(2, true, 3));
            _orders.Add(new(3, false, 3));
            _orders.Add(new(3, true, 3));
            _orders.Add(new(4, false, 3));
        }
        // === END === //
    }
}
