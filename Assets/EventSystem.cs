using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem current;
    public Piece selectedPiece;

    void Awake()
    {
        current = this;
    }

    public event Action onPieceClick;
    // public event Action showIndicator;
    // public event Action hideIndicator;
    public event Action<int, int> moveIndicator;

    public void SelectPiece(Piece piece)
    {
        if (onPieceClick != null)
        {
            selectedPiece = piece;
            onPieceClick();
            Debug.Log($"[Debug]: Selected piece ({piece.cellX}, {piece.cellY}).");
        }
    }

    // public void ShowIndicator()
    // {
    //     if (showIndicator != null)
    //     {

    //     }
    // }

    // public void HideIndicator()
    // {
    //     if (hideIndicator != null)
    //     {
            
    //     }
    // }

    public void MoveIndicator(int cellX, int cellY)
    {
        if (moveIndicator != null)
        {
            moveIndicator(cellX, cellY);
            Debug.Log($"[Debug]: Moved indicator ({cellX}, {cellY}).");
        }
    }
}
 