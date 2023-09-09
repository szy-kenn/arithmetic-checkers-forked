using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System;

namespace Damath
{
    public class Piece : MonoBehaviour
    {
        public Cell Cell;
        public int Col, Row;
        public string Value;
        public Side Side;
        public Player Owner;
        public bool IsKing = false;
        public bool CanCapture = false;
        public bool HasCaptured = false;
        public int Forward = 0;
        public List<Move> Moves = new();
        public List<Sprite> Sprites;
        public Color Color;
        public Color Shadow;

        [SerializeField] RectTransform rect;
        [SerializeField] TextMeshPro tmp;
        [SerializeField] SpriteRenderer overlayTop;
        [SerializeField] SpriteRenderer overlayShadow;

        public Piece(Side side, Color color, string value, bool isKing)
        {
            Side = side;
            Color = color;
            Value = value;
            IsKing = isKing;
        }

        public List<Move> GetMoves(Piece piece)
        {
            return new List<Move>();
        }
        
        void Start()
        {
            tmp.text = Value;

            if (IsKing)
            {
                overlayTop.sprite = Sprites[1];
            } else
            {
                overlayTop.sprite = Sprites[0];
            }
        }

        public void SetCell(Cell cell)
        {
            Cell = cell;
            Col = cell.Col;
            Row = cell.Row;
        }

        public void SetValue(string value)
        {
            Value = value;
            tmp.SetText(value);
        }

        public void SetSide(Side value)
        {
            Side = value;
            if (value == Side.Bot)
            {
                Forward = 1;
                SetColor(Colors.RichCerulean, Colors.darkJungleBlue);
            } else if (value == Side.Top)
            {
                Forward = -1;
                SetColor(Colors.PersimmonOrange, Colors.burntUmber);
            }
        }
        
        public void SetColor(Color top, Color shadow)
        {
            overlayTop.color = top;
            overlayShadow.color = shadow;
        }

        public void SetOwner(Player player)
        {
            Owner = player;
            player.Pieces.Add(this);
        }

        public void SetKing(bool value)
        {
            if (value)
            {
                Promote();
            } else
            {
                Demote();
            }
        }
        
        public void Promote()
        {
            if (IsKing) return;
            IsKing = true;
            overlayTop.sprite = Sprites[1];
            tmp.color = new Color(1, 1, 1, 1);
        }

        public void Demote()
        {
            if (!IsKing) return;
            IsKing = false;
            overlayTop.sprite = Sprites[0];
            tmp.color = new Color(0, 0, 0, 1);
        }

        public void Capture()
        {

        }

        public void Remove()
        {
            Destroy(this.gameObject);
        }
    }
}