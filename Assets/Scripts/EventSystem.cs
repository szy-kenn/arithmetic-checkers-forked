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

    public event Action initializeCells;
    public event Action onCellClick;
    public event Action onPieceClick;
    // public event Action showIndicator;
    public event Action hideIndicator;
    public event Action<Cell> moveIndicator;

    [SerializeField]
    public Dictionary<(int, int), Cell> BoardArray = new Dictionary<(int, int), Cell>();

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

    void InitializeCells()
    {
        if (initializeCells != null)
        {
            //
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
            // MoveIndicator();
            // ShowIndicator();
            selectedPiece = piece;
            onPieceClick();
        }
    }

    public void Select(Cell cell)
    {
        if (onCellClick != null)
        {
            // MoveIndicator(new Vector2(cell.col, cell.row));
            // ShowIndicator();
            selectedCell = cell;
            onCellClick();
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

    public void MoveIndicator(Cell cell)
    {
        if (moveIndicator != null)
        {
            moveIndicator(cell);
        }
    }

    public Cell GetCell(int col, int row)
    {
        return BoardArray[(col, row)];
    }
}
 