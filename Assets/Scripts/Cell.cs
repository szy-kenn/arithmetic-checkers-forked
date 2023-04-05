using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int col, row;
    public Vector2 cell;
    public Piece piece = null;

    Piece pieceToReturn;

    void Awake()
    {
        EventSystem.current.onCellClick += SelectMe;
    }

    void OnMouseDown()
    {
        if (piece != null)
        {
            Debug.Log($"[Debug]: Selected piece ({piece.col}, {piece.row})");
            EventSystem.current.Select(piece);
        } else
        {
            Debug.Log($"[Debug]: Selected cell ({this.col}, {this.row})");
            EventSystem.current.Select(this);
        }
    }

    void SelectMe()
    {
        if (EventSystem.current.selectedCell == this)
        {
            if (piece == null) return;
        }
    }

    public void SetColRow(int colValue, int rowValue)
    {
        col = colValue;
        row = rowValue;
        cell = new Vector2(colValue, rowValue);
    }

    public Piece GetPiece()
    {
        return piece;
    }

    public void SetPiece(Piece pieceToSet)
    {
        piece = pieceToSet;
    }

    public Piece RemovePiece()
    {
        pieceToReturn = piece;
        piece = null;
        return pieceToReturn;
    }

}
