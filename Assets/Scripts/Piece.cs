using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System;

namespace Damath
{
    public enum Side {Bot, Top, Spectator}

    public class Piece : MonoBehaviour
    {
        public Player owner = null;
        public Cell cell;
        public int col, row;
        public Vector3 position;
        public string value;
        public Side side;
        public Color color;
        public Color shadow;
        public bool IsKing = false;
        public bool CanCapture = false;
        public bool HasCaptured = false;
        public int forward = 0;
        public List<Move> moves = new();
        public List<Sprite> sprites;

        RectTransform rect;
        TextMeshPro textValue;
        SpriteRenderer overlayTop;
        SpriteRenderer overlayShadow;

        public Piece(Side side, Color color, string value, bool IsKing)
        {
            this.side = side;
            this.color = color;
            this.value = value;
            this.IsKing = IsKing;
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
            textValue.text = value;

            if (IsKing)
            {
                overlayTop.sprite = sprites[1];
            } else
            {
                overlayTop.sprite = sprites[0];
            }
        }

        public void SetCell(Cell cell)
        {
            this.cell = cell;
            this.col = cell.col;
            this.row = cell.row;
        }

        public void SetValue(string value)
        {
            this.value = value;
            textValue.SetText(value);
        }

        public void SetTeam(Side value)
        {
            side = value;

            if (value == Side.Bot)
            {
                forward = 1;
                // SetOwner(Game.Main.Match.players[0]);
            } else
            {
                forward = -1;
                // SetOwner(Game.Main.Match.players[1]);
            }
            owner.PieceCount += 1;
        }

        public void SetOwner(Player player)
        {
            owner = player;
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
            overlayTop.sprite = sprites[1];
            textValue.color = new Color(1, 1, 1, 1);
        }

        public void Demote()
        {
            if (!IsKing) return;
            IsKing = false;
            overlayTop.sprite = sprites[0];
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