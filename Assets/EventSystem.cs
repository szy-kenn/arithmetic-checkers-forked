using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystem : MonoBehaviour
{
    public static EventSystem current;
    public Cell selectedCell;
    public Piece selectedPiece;

    public bool HasPieceSelected = false;

    void Awake()
    {
        current = this;
    }
    public event Action onCellClick;
    public event Action onPieceClick;
    // public event Action showIndicator;
    public event Action hideIndicator;
    public event Action<Vector2> moveIndicator;


    void FixedUpdate()
    {
        CheckForClicks();
    }

    void CheckForClicks()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }

        if (Input.GetMouseButtonDown(1))
        {
            Deselect();
        }
    }

    void Refresh()
    {
        selectedPiece = null;
        Deselect();
    }

    public void Deselect()
    {
        if (selectedPiece != null)
        {
            selectedPiece = null;
            // HideIndicator();
        }
    }

    public void Select(Piece piece)
    {
        if (onPieceClick != null)
        {
            MoveIndicator(new Vector2(piece.col, piece.row));
            // ShowIndicator();
            selectedPiece = piece;
            onPieceClick();
            
            Debug.Log($"Debug]: Selected piece ({piece.col}, {piece.row})");
        }
    }

    public void Select(Cell cell)
    {
        if (onCellClick != null)
        {
            MoveIndicator(new Vector2(cell.col, cell.row));
            // ShowIndicator();
            selectedCell = cell;
            onCellClick();

            Debug.Log($"Debug]: Selected cell ({cell.col}, {cell.row})");
        }
    }

    // public void ShowIndicator()
    // {
    //     if (showIndicator != null)
    //     {
    //         showIndicator();
    //     }
    // }

    // public void HideIndicator()
    // {
    //     if (hideIndicator != null)
    //     {
    //         hideIndicator();
    //     }
    // }

    public void MoveIndicator(Vector2 cell)
    {
        if (moveIndicator != null)
        {
            moveIndicator(cell);
        }
    }
}
 