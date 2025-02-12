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
        public Piece HeldPiece = null;
        public Piece MovedPiece = null;
        public bool IsHolding = false;
        public bool HasCapture = false;
        public bool IsTurn = false;
        public int TurnNumber = 0;
        [SerializeField] private float MouseHoldTime = 0f;

        void Awake()
        {

        }

        void Start()
        {
            Game.Events.OnMatchBegin += SetConsoleOperator;
            Game.Events.OnLobbyStart += InitOnline;
            Game.Events.OnChangeTurn += SetTurn;
            Game.Events.OnPieceDone += Deselect;
            Game.Events.OnChangeTurn += SetTurn;
            
            Init();
        }

        void OnDisable()
        {
            Game.Events.OnMatchBegin -= SetConsoleOperator;
            Game.Events.OnPieceMove -= IMadeAMove;
            Game.Events.OnLobbyStart -= InitOnline;
            Game.Events.OnPieceDone -= Deselect;
            Game.Events.OnChangeTurn -= SetTurn;
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

        public void SetTurn(Side currentTurn)
        {
            if (currentTurn == Side)
            {
                IsTurn = true;
            } else
            {
                IsTurn = false;
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
            if (Input.GetMouseButtonDown(0))
            {
                CastRay();
                Game.Events.PlayerLeftClick(this);    
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                CastRay();
                Game.Events.PlayerRightClick(this);    
            }

            if (Input.GetMouseButton(0))
            {
                MouseHoldTime += 1 * Time.deltaTime;

                if (MouseHoldTime >= Settings.PieceGrabDelay)
                {
                    IsHolding = true;

                    if (HeldPiece != null)
                    {
                        HeldPiece.transform.position = Camera.main.ScreenToWorldPoint( new Vector3( Input.mousePosition.x, Input.mousePosition.y, 1));
                    } else
                    {
                        HeldPiece = SelectedPiece;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                MouseHoldTime = 0f;
                CastRay();

                if (IsHolding)
                {
                    IsHolding = false;
                    Game.Events.PlayerRelease(this);
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                // CastRay();
            }
        }
        
        /// <summary>
        /// Casts a ray then caches hit object.
        /// </summary>
        void CastRay()
        {
            if (!IsControllable) return;

            Hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);  

            if (Hit.collider == null) return;

            if (Hit.collider.CompareTag("Cell"))
            {
                SelectedCell = Hit.collider.gameObject.GetComponent<Cell>();
                SelectCell(SelectedCell);

            } else if (Hit.collider.CompareTag("Background"))
            {
                Debug.Log("Left clicked background");
                Game.Events.CellDeselect(SelectedCell);
            }
        }

        public void SelectCell(Cell cell)
        {
            SelectedCell = cell;

            if (SelectedCell.HasPiece)
            {
                SelectPiece(SelectedCell.Piece);
            } else
            {
                SelectMovecell(SelectedCell);
            }
        }

        public void SelectPiece(Piece piece)
        {
            if (SelectedCell.Piece.Side == Side)
            {
                if (SelectedPiece != null)
                {
                    if (SelectedPiece == piece)
                    {
                        ReleaseHeldPiece();
                        return;
                    } else
                    {
                        Deselect();
                        return;
                    }
                }

                if (MovedPiece != null)
                {
                    if (piece != MovedPiece)
                    {
                        Deselect();
                        return;
                    }
                } else
                {
                    SelectedPiece = piece;
                    Game.Events.PlayerSelectPiece(this, SelectedPiece);
                    Game.Audio.PlaySound("Select");
                    return;
                }
            }

        }

        public void SelectMovecell(Cell cell = null)
        {
            if (cell != null) SelectedCell = cell;

            if (SelectedCell.IsValidMove)
            {
                if (!IsTurn) return;

                Debug.Log("move");
                Game.Events.PlayerSelectMovecell(this, SelectedCell);
                Game.Audio.PlaySound("Move");
                return;
            }

            Deselect();
        }

        public void ReleaseHeldPiece()
        {
            HeldPiece?.ResetPosition();       
            HeldPiece = null;
        }

        public void Deselect()
        {
            SelectedPiece = null;
            ReleaseHeldPiece();

            Game.Events.Deselect();
            Game.Events.PlayerDeselect(this);
        }

        public void Deselect(Piece piece)
        {
            Deselect();
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