using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System;
using UnityEngine.EventSystems;

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
        public bool IsSelected = false;
        public Player SelectedBy = null;
        [SerializeField] RectTransform rect;
        [SerializeField] TextMeshPro tmp;
        [SerializeField] SpriteRenderer overlayTop;
        [SerializeField] SpriteRenderer overlayShadow;
        private static GameObject prefab;

        void Awake()
        {
            prefab = Resources.Load<GameObject>("Prefabs/Chip");
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

        public static Piece Create(int col, int row)
        {
            Cell cell = Board.GetCell(col, row);
            return Create(cell);
        }
        
        public static Piece Create(Cell cell)
        {
            Piece newPiece = Instantiate(prefab).GetComponent<Piece>();
            return newPiece;
        }

        public static Piece Create(Piece piece)
        {
            Piece newPiece = Instantiate(piece.gameObject).GetComponent<Piece>();
            return newPiece;
        }

        public void Init(Side side, string value, bool isKing)
        {

        }
        
        public List<Move> GetMoves(Piece piece)
        {
            return new List<Move>();
        }

        public void OnMouseEnter()
        {
            Debug.Log("enter");
        }

        public void OnMouseDown()
        {
            Debug.Log("clicked on piece");
            Game.Events.PieceSelected(this);
        }
        
        public void OnMouseDrag()
        {
            if (!IsSelected) return;
            Debug.Log("dragging piece");
        }

        public void Select()
        {
            if (IsSelected) return;

            IsSelected = true;
            SelectedBy = null;
            // Game.Events.PieceSelect(this);
        }

        /// <summary>
        /// Select this piece as a player.
        /// </summary>
        /// <param name="player"></param>
        public void SelectAs(Player player)
        {
            if (IsSelected) return;
            // if (player.Side != Side) return;

            IsSelected = true;
            SelectedBy = player;
            Game.Events.PlayerSelectPiece(player, this);
        }
        
        public void Deselect()
        {
            IsSelected = false;
            SelectedBy = null;
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