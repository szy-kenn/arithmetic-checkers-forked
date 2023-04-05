using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    Color none = new Color(0, 0, 0, 1);

    public static Dictionary<(int, int), (Side, Color, string, bool)> classic = new Dictionary<(int, int), (Side, Color, string, bool)>()
    {
        {(1, 0), (Side.Bot, Color.blue, "-11", false)},
        {(3, 0), (Side.Bot, Color.blue, "8", false)},
        {(5, 0), (Side.Bot, Color.blue, "-5", false)},
        {(7, 0), (Side.Bot, Color.blue, "2", false)},
        {(0, 1), (Side.Bot, Color.blue, "0", false)},
        {(2, 1), (Side.Bot, Color.blue, "-3", false)},
        {(4, 1), (Side.Bot, Color.blue, "10", false)},
        {(6, 1), (Side.Bot, Color.blue, "-7", false)},
        {(1, 2), (Side.Bot, Color.blue, "-9", false)},
        {(3, 2), (Side.Bot, Color.blue, "6", false)},
        {(5, 2), (Side.Bot, Color.blue, "-1", false)},
        {(7, 2), (Side.Bot, Color.blue, "4", false)},

        {(0, 5), (Side.Top, Color.red, "4", false)},
        {(2, 5), (Side.Top, Color.red, "-1", false)},
        {(4, 5), (Side.Top, Color.red, "6", false)},
        {(6, 5), (Side.Top, Color.red, "-9", false)},
        {(1, 6), (Side.Top, Color.red, "-7", false)},
        {(3, 6), (Side.Top, Color.red, "10", false)},
        {(5, 6), (Side.Top, Color.red, "-3", false)},
        {(7, 6), (Side.Top, Color.red, "0", false)},
        {(0, 7), (Side.Top, Color.red, "2", false)},
        {(2, 7), (Side.Top, Color.red, "-5", false)},
        {(4, 7), (Side.Top, Color.red, "8", false)},
        {(6, 7), (Side.Top, Color.red, "-11", false)},
    };
} 

    

