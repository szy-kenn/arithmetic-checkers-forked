using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public List<((int, int), Side, Color, string, bool)> pieceMap;

    // Abstract representation of pieces on the board
    public Dictionary<Vector2, Piece> pieces;

    Color blue = new Color(0, 0, 1, 1);
    Color orange = new Color(1, 0.647f, 0, 1);
    Color none = new Color(0, 0, 0, 1);
    
    void Start()
    {
        SetDefaultMap();
    }

    public void SetMap(preset value)
    {
        switch (value)
        {
            case preset.Default:
                SetDefaultMap();
                break;
            case preset.Chess:
                SetChessMap();
                break;
            default:
                SetDefaultMap();
                break;
        }
    }

    public Dictionary<Vector2, Piece> GetMoves(Piece piece)
    {
        Dictionary<Vector2, Piece> moves = null;
        int up = 1;
        int down = -1;

        if (piece.side == Side.Bot)
        {
            if (piece.IsKing)
            {
                //
                CheckLeft(piece, up, piece.IsKing);
                CheckRight(piece, up, piece.IsKing);
                //
                CheckLeft(piece, down, piece.IsKing);
                CheckRight(piece, down, piece.IsKing);

            } else
            {
                CheckLeft(piece, up, piece.IsKing);
                CheckRight(piece, up, piece.IsKing);
            }
        } else if (piece.side == Side.Top)
        {
            if (piece.IsKing)
            {
                // Check up
                
                // Check down

            } else
            {
                // Check up
                
                // Check down

            }
        }

        return moves;
    }

    Dictionary<Vector2, Piece> CheckLeft(Piece piece, int yDirection, bool isKing)
    {
        Dictionary<Vector2, Piece> destinationCells = null;
        Piece captureablePieces = null;
        int nextEnemyPiece = 0;
        int left = piece.col - 1;

        for (int row = piece.col + yDirection; row < Constants.maximumRows; row++)
        {
            if (left < 0) break;
            if (nextEnemyPiece >= 2) break;

            Piece cellToCheck = pieces[new Vector2(left, row)];

            if (cellToCheck == null)
            {
                if (captureablePieces != null)
                {
                    piece.CanCapture = true;
                    destinationCells.Add(new Vector2(left, row), captureablePieces);
                } else
                {
                    destinationCells.Add(new Vector2(left, row), cellToCheck);
                }

                if (!piece.IsKing) break;

                if (captureablePieces != null)
                {
                    piece.CanCapture = true;
                    
                    if (!piece.IsKing) break;
                }

            } else if (cellToCheck.side == piece.side)
            {
                break;
            } else
            {
                nextEnemyPiece++;
                captureablePieces = cellToCheck;
            }

            left--;
        }

        return destinationCells;
    }

    Move CheckRight(Piece piece, int yDirection, bool isKing)
    {
        int left = piece.row + 1;
        

        return new Move();
    }

    void SetDefaultMap()
    {
        pieceMap = new List<((int, int), Side, Color, string, bool)>
        {
            ((1, 0), Side.Bot, blue, "-11", false),
            ((3, 0), Side.Bot, blue, "8", false),
            ((5, 0), Side.Bot, blue, "-5", false),
            ((7, 0), Side.Bot, blue, "2", false),
            ((0, 1), Side.Bot, blue, "0", false),
            ((2, 1), Side.Bot, blue, "-3", false),
            ((4, 1), Side.Bot, blue, "10", false),
            ((6, 1), Side.Bot, blue, "-7", false),
            ((1, 2), Side.Bot, blue, "-9", false),
            ((3, 2), Side.Bot, blue, "6", false),
            ((5, 2), Side.Bot, blue, "-1", false),
            ((7, 2), Side.Bot, blue, "4", false),

            ((0, 5), Side.Top, orange, "4", false),
            ((2, 5), Side.Top, orange, "-1", false),
            ((4, 5), Side.Top, orange, "6", false),
            ((6, 5), Side.Top, orange, "-9", false),
            ((1, 6), Side.Top, orange, "-7", false),
            ((3, 6), Side.Top, orange, "10", false),
            ((5, 6), Side.Top, orange, "-3", false),
            ((7, 6), Side.Top, orange, "0", false),
            ((0, 7), Side.Top, orange, "2", false),
            ((2, 7), Side.Top, orange, "-5", false),
            ((4, 7), Side.Top, orange, "8", false),
            ((6, 7), Side.Top, orange, "-11", false),
        };
    }

    void SetChessMap()
    {
        // Needs changing
        pieceMap = new List<((int, int), Side, Color, string, bool)>
        {
            ((1, 0), Side.Bot, blue, "-11", false),
            ((3, 0), Side.Bot, blue, "8", false),
            ((5, 0), Side.Bot, blue, "-5", false),
            ((7, 0), Side.Bot, blue, "2", false),
            ((0, 1), Side.Bot, blue, "0", false),
            ((2, 1), Side.Bot, blue, "-3", false),
            ((4, 1), Side.Bot, blue, "10", false),
            ((6, 1), Side.Bot, blue, "-7", false),
            ((1, 2), Side.Bot, blue, "-9", false),
            ((3, 2), Side.Bot, blue, "6", false),
            ((5, 2), Side.Bot, blue, "-1", false),
            ((7, 2), Side.Bot, blue, "4", false),

            ((0, 5), Side.Top, orange, "4", false),
            ((2, 5), Side.Top, orange, "-1", false),
            ((4, 5), Side.Top, orange, "6", false),
            ((6, 5), Side.Top, orange, "-9", false),
            ((1, 6), Side.Top, orange, "-7", false),
            ((3, 6), Side.Top, orange, "10", false),
            ((5, 6), Side.Top, orange, "-3", false),
            ((7, 6), Side.Top, orange, "0", false),
            ((0, 7), Side.Top, orange, "2", false),
            ((2, 7), Side.Top, orange, "-5", false),
            ((4, 7), Side.Top, orange, "8", false),
            ((6, 7), Side.Top, orange, "-11", false),
        };
    }

}