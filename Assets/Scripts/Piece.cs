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
        public Player Owner = null;
        public Cell Cell;
        public int Col, Row;
        public Vector3 Position;
        public string Value;
        public Side Side;
        public Color Color;
        public Color Shadow;
        public bool IsKing = false;
        public bool CanCapture = false;
        public bool HasCaptured = false;
        public int Forward = 0;
        public List<Move> Moves = new();
        public List<Sprite> Sprites;

        RectTransform rect;
        TextMeshPro textValue;
        SpriteRenderer overlayTop;
        SpriteRenderer overlayShadow;

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

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            textValue = transform.Find("Text").GetComponent<TextMeshPro>();
            overlayTop = transform.Find("Overlay Top").GetComponent<SpriteRenderer>();
            overlayShadow = transform.Find("Overlay Shadow").GetComponent<SpriteRenderer>();
        }
        
        void Start()
        {
            textValue.text = Value;

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
            this.Value = value;
            textValue.SetText(value);
        }

        public void SetTeam(Side value)
        {
            Side = value;

            if (value == Side.Bot)
            {
                Forward = 1;
                // SetOwner(Game.Main.Match.players[0]);
            } else
            {
                Forward = -1;
                // SetOwner(Game.Main.Match.players[1]);
            }
            Owner.PieceCount += 1;
        }

        public void SetOwner(Player player)
        {
            Owner = player;
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
            textValue.color = new Color(1, 1, 1, 1);
        }

        public void Demote()
        {
            if (!IsKing) return;
            IsKing = false;
            overlayTop.sprite = Sprites[0];
            textValue.color = new Color(0, 0, 0, 1);
        }

        public void SetColor(Color top)
        {
            overlayTop.color = top;
        }
        
        public void SetColor(Color top, Color shadow)
        {
            overlayTop.color = top;
            overlayShadow.color = shadow;
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