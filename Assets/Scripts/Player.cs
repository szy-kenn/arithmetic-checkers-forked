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
                RaycastHit2D hit = CastRay();
                if (hit.collider == null) return;

                Click(hit);
                Game.Events.PlayerLeftClick(this);
            }

            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit2D hit = CastRay();
                if (hit.collider == null) return;

                Click(hit);
                Game.Events.PlayerRightClick(this);
            }
        }

        RaycastHit2D CastRay()
        {
            return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        }

        public void Click(RaycastHit2D hit)
        {
            if (hit.collider.CompareTag("Cell"))
            {
                SelectedCell = hit.collider.gameObject.GetComponent<Cell>();
            }
            // Add more else-if statements to add more components that can be detected by the Raycast
            // Make sure to include every game objects with tags to avoid getting not detected
        }
    }
}