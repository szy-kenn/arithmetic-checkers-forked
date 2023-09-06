using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damath
{
    public class Player : MonoBehaviour
    {
        public string Name = "Player";
        public Side Side;
        public int PieceCount = 0;
        public List<Piece> Pieces;
        public List<Piece> CapturedPieces;
        public float Score = 0f;
        public bool IsControllable = true;
        public bool IsPlaying = false;
        public bool IsModerator = false;
        public bool IsAI = false;
        public Cell SelectedCell = null;
        RaycastHit2D hit;

        void Start()
        {
            this.name = $"Player ({Name})";

            Game.Events.OnMatchBegin += SetConsoleOperator;
            Game.Events.OnCellDeselect += DeselectCell;
        }

        void OnDisable()
        {
            Game.Events.OnMatchBegin -= SetConsoleOperator;
            Game.Events.OnCellDeselect -= DeselectCell;
        }

        void Update()
        {
            DetectRaycast();
            
            // Debug
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (!IsPlaying) return;
                SetConsoleOperator();
            }
        }

        public void DeselectCell(Cell cell)
        {
            SelectedCell = null;
        }

        void SetConsoleOperator()
        {
            Game.Console.SetOperator(this);
            Debug.Log($"Set operator for {Game.Console} to {this}");
        }

        void SetConsoleOperator(MatchController match)
        {
            SetConsoleOperator();
        }

        public string SetName(string value)
        {
            name = $"Player {value}";
            Name = value;
            return value;
        }

        public void SetPlaying(bool value)
        {
            IsPlaying = value;
        }

        public void SetSide(Side value)
        {
            Side = value;
        }

        public void SetScore(float value)
        {
            Score = value;
        }

        void DetectRaycast()
        {
            if (Input.GetMouseButton(0))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Game.Events.PlayerHold(this);
                }
                
                if (Input.GetMouseButtonUp(0))
                {
                    Game.Events.PlayerRelease(this);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                CastRay();
                LeftClick();
            }

            if (Input.GetMouseButtonDown(1))
            {
                CastRay();
                RightClick();
            }
        }

        RaycastHit2D CastRay()
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);            
            return hit;
        }

        public void LeftClick()
        {
            if (hit.collider == null) return;

            if (hit.collider.CompareTag("Cell"))
            {
                SelectedCell = hit.collider.gameObject.GetComponent<Cell>();
                Game.Events.PlayerSelectCell(this);
            } else if (hit.collider.CompareTag("Background"))
            {
                Debug.Log("Left clicked background");
                Game.Events.CellDeselect(SelectedCell);
            }
            Game.Events.PlayerLeftClick(this);
        }

        public void RightClick()
        {
            if (hit.collider == null) return;

            if (hit.collider.CompareTag("Cell"))
            {
                SelectedCell = hit.collider.gameObject.GetComponent<Cell>();
                Game.Events.PlayerSelectCell(this);
            } else if (hit.collider.CompareTag("Background"))
            {
                Debug.Log("Right clicked background");
                Game.Events.CellDeselect(SelectedCell);
            }
            Game.Events.PlayerRightClick(this);
        }
    }
}