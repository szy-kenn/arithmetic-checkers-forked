using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Symbols {Default, Random, Custom}
public enum Pieces {Integers, Naturals}

public class Map
{
    public Dictionary<(int, int), Operation> symbols = new Dictionary<(int, int), Operation>();
    public Dictionary<(int, int), Operation> symbolsCustom = new Dictionary<(int, int), Operation>();
    public Dictionary<(int, int), (Side, Color, string, bool)> pieces = new Dictionary<(int, int), (Side, Color, string, bool)>();

    Color none = new Color(0, 0, 0, 1);

    public Map()
    {
        Init();
    }

    void Init()
    {
        // Set defaults
        SetSymbols(Symbols.Default);
        SetPieces(Pieces.Integers);        
    }

    public void SetSymbols(Dictionary<(int, int), Operation> value)
    {
        symbolsCustom = value;
    }

    public void SetSymbols(Symbols value)
    {
        switch (value)
        {
            case Symbols.Default:
                symbols = new Dictionary<(int, int), Operation>()
                {
                    {(1, 0), Operation.Add},
                    {(3, 0), Operation.Sub},
                    {(5, 0), Operation.Div},
                    {(7, 0), Operation.Mul},
                    {(0, 1), Operation.Sub},
                    {(2, 1), Operation.Add},
                    {(4, 1), Operation.Mul},
                    {(6, 1), Operation.Div},
                    {(1, 2), Operation.Div},
                    {(3, 2), Operation.Mul},
                    {(5, 2), Operation.Add},
                    {(7, 2), Operation.Sub},
                    {(0, 3), Operation.Mul},
                    {(2, 3), Operation.Div},
                    {(4, 3), Operation.Sub},
                    {(6, 3), Operation.Add},
                    {(1, 4), Operation.Add},
                    {(3, 4), Operation.Sub},
                    {(5, 4), Operation.Div},
                    {(7, 4), Operation.Mul},
                    {(0, 5), Operation.Sub},
                    {(2, 5), Operation.Add},
                    {(4, 5), Operation.Mul},
                    {(6, 5), Operation.Div},
                    {(1, 6), Operation.Div},
                    {(3, 6), Operation.Mul},
                    {(5, 6), Operation.Add},
                    {(7, 6), Operation.Sub},
                    {(0, 7), Operation.Mul},
                    {(2, 7), Operation.Div},
                    {(4, 7), Operation.Sub},
                    {(6, 7), Operation.Add},
                };
                break;
            case Symbols.Random:
                break;
            case Symbols.Custom:
                symbols = symbolsCustom;
                break;
            default:
                break;
        }
    }

    public void SetPieces(Dictionary<(int, int), (Side, Color, string, bool)> value)
    {
        pieces = value;
    }

    public void SetPieces(Pieces value)
    {
        switch (value)
        {
            case Pieces.Integers:
                pieces = new Dictionary<(int, int), (Side, Color, string, bool)>()
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
                    {(6, 7), (Side.Top, Color.red, "-11", false)}
                };
                break;
            case Pieces.Naturals:
                pieces = new Dictionary<(int, int), (Side, Color, string, bool)>()
                {
                    {(1, 0), (Side.Bot, Color.blue, "11", false)},
                    {(3, 0), (Side.Bot, Color.blue, "8", false)},
                    {(5, 0), (Side.Bot, Color.blue, "5", false)},
                    {(7, 0), (Side.Bot, Color.blue, "2", false)},
                    {(0, 1), (Side.Bot, Color.blue, "0", false)},
                    {(2, 1), (Side.Bot, Color.blue, "3", false)},
                    {(4, 1), (Side.Bot, Color.blue, "10", false)},
                    {(6, 1), (Side.Bot, Color.blue, "7", false)},
                    {(1, 2), (Side.Bot, Color.blue, "9", false)},
                    {(3, 2), (Side.Bot, Color.blue, "6", false)},
                    {(5, 2), (Side.Bot, Color.blue, "1", false)},
                    {(7, 2), (Side.Bot, Color.blue, "4", false)},

                    {(0, 5), (Side.Top, Color.red, "4", false)},
                    {(2, 5), (Side.Top, Color.red, "1", false)},
                    {(4, 5), (Side.Top, Color.red, "6", false)},
                    {(6, 5), (Side.Top, Color.red, "9", false)},
                    {(1, 6), (Side.Top, Color.red, "7", false)},
                    {(3, 6), (Side.Top, Color.red, "10", false)},
                    {(5, 6), (Side.Top, Color.red, "3", false)},
                    {(7, 6), (Side.Top, Color.red, "0", false)},
                    {(0, 7), (Side.Top, Color.red, "2", false)},
                    {(2, 7), (Side.Top, Color.red, "5", false)},
                    {(4, 7), (Side.Top, Color.red, "8", false)},
                    {(6, 7), (Side.Top, Color.red, "11", false)}
                };
                break;
            default:
                break;
        }
    }
}