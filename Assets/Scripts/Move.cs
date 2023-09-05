using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damath
{
    public enum MoveType {All, Normal, Capture}

    public class Move
    {
        public Cell originCell, from;
        public Cell destinationCell, to;
        public Piece capturingPiece = null;
        public Piece capturedPiece = null;
        public bool HasCapture = false;
        public float Score = 0;
        public MoveType type;

        public Move(Cell origin, Cell destination)
        {
            originCell = origin; from = originCell;
            destinationCell = destination; to = destinationCell;
            destinationCell.IsValidMove = true;
            capturingPiece = origin.Piece;
            type = MoveType.Normal;
        }
        
        public Move(Cell origin, Cell destination, Piece toCapture)
        {
            originCell = origin; from = originCell;
            destinationCell = destination; to = destinationCell;
            destinationCell.IsValidMove = true;
            capturingPiece = origin.Piece;
            capturedPiece = toCapture;
            HasCapture = true;
            type = MoveType.Capture;
        }

        public void SetScoreValue(float value)
        {
            Score = value;
        }
    }
}