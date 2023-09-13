using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;
using Unity.VisualScripting;
using System.Linq;
using System.Threading.Tasks;

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
        public bool IsControllable = false;
        public bool IsAI = false;
        public Cell SelectedCell = null;
        public Piece SelectedPiece = null;
        private Piece HeldPiece = null;
        private bool IsHolding;
        public float MouseHeld = 0f;

        void Awake()
        {

        }

        void Start()
        {
            Init();
            Game.Events.OnMatchBegin += SetConsoleOperator;
            Game.Events.OnDeselect += Deselect;
            Game.Events.OnLobbyStart += InitOnline;
        }

        void OnDisable()
        {
            Game.Events.OnMatchBegin -= SetConsoleOperator;
            Game.Events.OnDeselect -= Deselect;
            Game.Events.OnPieceMove -= IMadeAMove;
            Game.Events.OnLobbyStart -= InitOnline;
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

        public void InitOnline(Lobby lobby)
        {
            Game.Events.OnPieceMove += IMadeAMove;
        }

        public void Init()
        {
            Name = Game.Main.Nickname;
            name = $"{Game.Main.Nickname} (Player)";
            Game.Events.PlayerCreate(this);
        }

        public void Deselect()
        {
            SelectedCell = null;
            SelectedPiece = null;
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
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                Select();
            }

            if (Input.GetMouseButton(0))
            {
                MouseHeld += 1 * Time.deltaTime;

                if (MouseHeld >= Settings.PieceGrabDelay)
                {
                    if (HeldPiece != null)
                    {
                        HeldPiece.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    }
                    
                    Debug.Log("hold");
                    Game.Events.PlayerHold(this);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log("release");
                    Game.Events.PlayerRelease(this);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                MouseHeld = 0f;

                Select();
            }

            if (Input.GetMouseButtonUp(1))
            {
                // CastRay();
            }
        }

        void CastRay()
        {
            if (!IsControllable) return;

            Hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);  
        }
        
        void Select()
        {
            CastRay();

            if (Hit.collider == null) return;

            if (Hit.collider.CompareTag("Cell"))
            {
                SelectedCell = Hit.collider.gameObject.GetComponent<Cell>();
                // Game.Events.PlayerSelectCell(this, SelectedCell);

            } else if (Hit.collider.CompareTag("Piece"))
            {
                SelectedPiece = Hit.collider.gameObject.GetComponent<Piece>();
                // Game.Events.PlayerSelectPiece(this, SelectedPiece);

            } else if (Hit.collider.CompareTag("Background"))
            {
                Debug.Log("Left clicked background");
                Game.Events.CellDeselect(SelectedCell);
            }
            Game.Events.PlayerLeftClick(this);        
        }

        public void SelectCell()
        {
            
        }

        public void HoldPiece(Piece piece)
        {
            HeldPiece = Piece.Create(piece);
            Game.Events.PlayerHoldPiece(this, HeldPiece);
        }

        private void IMadeAMove(Move move)
        {
            Debug.Log($"I made a move");
            
            int[] moveData = new int[]
            {
                move.originCell.Col,
                move.originCell.Row,
                move.destinationCell.Col,
                move.destinationCell.Row
            };
          
            Game.Console.Log("Sending move data to server...");
            SendMoveDataServerRpc(moveData);
        }

        [ServerRpc(RequireOwnership = false)]
        public void SendMoveDataServerRpc(int[] moveData, ServerRpcParams serverRpcParams = default)
        {
            Game.Console.Log("Received move data");
            var senderClientId = serverRpcParams.Receive.SenderClientId;

            Game.Console.Log("Sending move data to clients...");
            ReceiveMoveDataClientRpc(moveData, GetClientsExcept(senderClientId));
        }

        private ClientRpcParams GetClientsExcept(ulong exceptedClientId)
        {
            var target = new ClientRpcParams()
            {
                Send = new ClientRpcSendParams()
                {
                    // This should get players from the lobby tho (?)
                    TargetClientIds = Network.Main.ConnectedClientsIds.Where(x => x != exceptedClientId).ToArray()
                }
            };
            return target;
        }

        [ClientRpc]
        public void ReceiveMoveDataClientRpc(int[] command, ClientRpcParams clientRpcParams)
        {
            Game.Console.Log("Received client rpc");
            Game.Console.Log($"Someone made a move {command}");
        }
    }
}