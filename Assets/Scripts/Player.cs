using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;
using Unity.VisualScripting;

namespace Damath
{
    public class Actor : MonoBehaviour
    {
        public string Name = "Actor";
        public NetworkObject networkObject { get; set; }
        public RaycastHit2D Hit;

        void Awake()
        {
            networkObject = GetComponent<NetworkObject>();
        }
    }

    public class Spectator : Actor
    {
        public bool IsModerator = false;
    }

    public class Player : Actor
    {
        public ulong Id { get; private set; }
        public Side Side;
        public List<Piece> Pieces;
        public List<Piece> CapturedPieces;
        public float Score = 0f;
        public bool IsPlaying = false;
        public bool IsModerator = false;
        public bool IsAI = false;
        public Cell SelectedCell = null;

        void Awake()
        {

        }

        void Start()
        {
            Init();
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

        public void Init()
        {
            Name = Game.Main.Nickname;
            name = $"{Game.Main.Nickname} (Player)";
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

        public void SetClientId(ulong value)
        {
            Id = value;
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
                
                    if (Input.GetMouseButtonUp(0))
                    {
                        Game.Events.PlayerRelease(this);
                    }
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
            Hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);            
            return Hit;
        }
        
        public void LeftClick()
        {
            if (Hit.collider == null) return;

            if (Hit.collider.CompareTag("Cell"))
            {
                SelectCell();
            } else if (Hit.collider.CompareTag("Background"))
            {
                Debug.Log("Left clicked background");
                Game.Events.CellDeselect(SelectedCell);
            }
            Game.Events.PlayerLeftClick(this);
        }

        public void RightClick()
        {
            if (Hit.collider == null) return;

            if (Hit.collider.CompareTag("Cell"))
            {
                SelectCell();
            } else if (Hit.collider.CompareTag("Background"))
            {
                Debug.Log("Right clicked background");
                Game.Events.CellDeselect(SelectedCell);
            }
            Game.Events.PlayerRightClick(this);
        }

        public void SelectCell()
        {
            SelectedCell = Hit.collider.gameObject.GetComponent<Cell>();
            Game.Events.PlayerSelectCell(this);
        }
    }
}