using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType {All, Normal, Capture}

public class Move
{
    public Cell originCell, from;
    public Cell destinationCell, to;
    public Piece capturingPiece = null;
    public Piece capturedPiece = null;
    public bool HasCapture = false;
    public float score = 0;
    public MoveType type;

    public Move(Cell origin, Cell destination)
    {
        originCell = origin; from = originCell;
        destinationCell = destination; to = destinationCell;
        destinationCell.IsValidMove = true;
        capturingPiece = origin.piece;
        type = MoveType.Normal;
    }
    
    public Move(Cell origin, Cell destination, Piece toCapture)
    {
        originCell = origin; from = originCell;
        destinationCell = destination; to = destinationCell;
        destinationCell.IsValidMove = true;
        capturingPiece = origin.piece;
        capturedPiece = toCapture;
        HasCapture = true;
        type = MoveType.Capture;
    }
}
