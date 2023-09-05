using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damath
{
    public enum Operation {None, Add, Sub, Mul, Div}

    public class Cell : MonoBehaviour
    {
        public int Col, Row;
        public Piece Piece = null;
        public bool HasPiece = false;
        public bool IsValidMove = false;
        public Operation Operation;
        public List<Sprite> Sprite;

        SpriteRenderer spriteRenderer;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Refresh()
        {
            IsValidMove = false;
        }

        public void SetOperation(Operation value)
        {
            Operation = value;

            spriteRenderer.sprite = value switch
            {
                Operation.Add => Sprite[0],
                Operation.Sub => Sprite[1],
                Operation.Mul => Sprite[2],
                Operation.Div => Sprite[3],
                _ => null,
            };
            spriteRenderer.color = Colors.darkCerulean;
        }

        public void SetColRow(int colValue, int rowValue)
        {
            Col = colValue;
            Row = rowValue;
        }

        public Piece GetPiece()
        {
            if(Piece != null)
            {
                return Piece;
            }
            return null;
        }

        public void SetPiece(Piece piece)
        {
            if (piece == null) return;
            
            Piece = piece;
            Piece.SetCell(this);
            HasPiece = true;
            IsValidMove = false;
        }
        
        public void RemovePiece()
        {
            Piece = null;
            HasPiece = false;
        }
    }
}