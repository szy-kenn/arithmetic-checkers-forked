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
        public float Score = 0f;
        public bool IsPlaying = false;
        public bool IsModerator = false;
        public bool IsAI = false;
        public Cell SelectedCell = null;

        void Start()
        {
            this.name = $"Player ({Name})";
        }

        void Awake()
        {
            Game.Events.OnMatchBegin += SetConsoleOperator;
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
            this.name = $"Player {value}";
            this.Name = name;
            return value;
        }

        public bool SetPlaying(bool value)
        {
            this.IsPlaying = value;
            return value;
        }

        public Side SetSide(Side value)
        {
            this.Side = value;
            return value;
        }

        public void SetScore(float value)
        {
            this.Score = value;
        }

        void DetectRaycast()
        {
            if (!IsPlaying) return;

            if (Input.GetMouseButton(0))
            {
                Game.Events.PlayerHold(this);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Game.Events.PlayerClick(this);

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider == null) return;

                Click(hit);
            }

            if (Input.GetMouseButtonDown(1))
            {
                Game.Events.PlayerClick(this);

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider == null) return;
                if (!IsModerator)
                {
                    Click(hit);
                    return;
                }

                // switch (hit.collider.tag)
                // {
                //     case "Cell":
                //         selectedCell = hit.collider.gameObject.GetComponent<Cell>();

                //         if (Game.Main.Match.Rules.EnableCheats)
                //         {
                //             Game.Main.Match.Cheats.Select(selectedCell); 
                //             Game.Main.Match.Cheats.CreatePieceMenu();
                //         }
                //         break;

                //     case "Background":
                //         Game.Main.Match.Cheats.CreateToolsMenu(); 
                //         break;

                //     default:
                //         break;
                // }
            }
        }

        public void Click(RaycastHit2D hit)
        {
            Game.Events.PlayerSelect(this);

            if (hit.collider.tag == "Cell")
            {
                SelectedCell = hit.collider.gameObject.GetComponent<Cell>();

                Game.Events.CellSelect(SelectedCell);
            } else
            {
                Game.Events.PlayerDeselect(this);
            }
            // Add more else-if statements to add more components that can be detected by the Raycast
            // Make sure to include every game objects with tags to avoid getting not detected
        }
    }
}