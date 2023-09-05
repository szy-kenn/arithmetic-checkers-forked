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

            switch (value)
            {
                case Operation.Add:
                    spriteRenderer.sprite = Sprite[0]; 
                    break;
                case Operation.Sub:
                    spriteRenderer.sprite = Sprite[1]; 
                    break;
                case Operation.Mul:
                    spriteRenderer.sprite = Sprite[2]; 
                    break;
                case Operation.Div:
                    spriteRenderer.sprite = Sprite[3]; 
                    break;
                default:
                    spriteRenderer.sprite = null; 
                    break;
            }
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
            HasPiece = true;
        }
        
        public void RemovePiece()
        {
            Piece = null;
            HasPiece = false;
        }
    }
}