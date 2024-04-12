using System.Collections;
using System.Collections.Generic;

public class Order
{
    public bool ham, bacon, lettuce, tomato, cheese, meatballs, tomato_sauce, grilled;
    public int type;
    public int level;
    public Order(int type, bool toast, int level)
    {
        this.type = type;
        this.level = level;
        grilled = toast;

        switch (level)
        {
            case 1:
                switch (type)
                {
                    case 0:
                        ham = true;
                        break;
                    case 1:
                        ham = true;
                        cheese = true;
                        break;
                    case 2:
                        ham = true;
                        lettuce = true;
                        tomato = true;
                        break;
                    case 3:
                        bacon = true;
                        lettuce = true;
                        tomato = true;
                        break;
                    default:
                        ham = true;
                        break;
                }
                break;
            case 2:
                switch (type)
                {
                    case 0:
                        ham = true;
                        break;
                    case 1:
                        ham = true;
                        cheese = true;
                        break;
                    case 2:
                        ham = true;
                        lettuce = true;
                        tomato = true;
                        break;
                    case 3:
                        meatballs = true;
                        tomato_sauce = true;
                        break;
                    default:
                        ham = true;
                        break;
                }
                break;
            case 3:
                switch (type)
                {
                    case 0:
                        ham = true;
                        break;
                    case 1:
                        ham = true;
                        cheese = true;
                        break;
                    case 2:
                        ham = true;
                        lettuce = true;
                        tomato = true;
                        break;
                    case 3:
                        bacon = true;
                        lettuce = true;
                        tomato = true;
                        break;
                    case 4:
                        meatballs = true;
                        tomato_sauce = true;
                        break;
                    default:
                        ham = true;
                        break;
                }
                break;
        }
    }
}
