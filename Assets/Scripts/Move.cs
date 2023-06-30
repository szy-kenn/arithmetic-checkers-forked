using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType {All, Normal, Capture}

public class Move
{
    public Cell originCell, from;
    public Cell destinationCell, to;
    public Piece capturingPiece;
    public Piece capturedPiece;
    public bool HasCapture;
    public float score;
    public MoveType type;

    public Move()
    {

    }

    public Move(Cell origin, Cell destination)
    {
        originCell = origin;
        destinationCell = destination;
    }
    
    public Move(Cell origin, Cell destination, Piece captureable)
    {
        originCell = origin;
        destinationCell = destination;
        capturingPiece = origin.piece;
        capturedPiece = captureable;
        HasCapture = true;
    }
}
