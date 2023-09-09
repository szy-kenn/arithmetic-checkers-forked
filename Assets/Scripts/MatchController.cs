using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity;
using System.Runtime.InteropServices;
using Unity.Netcode;
using System;
using System.Linq;

namespace Damath
{
    /// <summary>
    /// Controls the match.
    /// </summary>
    public class MatchController : NetworkBehaviour
    {
        [Header("Match")]
        public Ruleset Rules;
        public bool IsPlaying { get; set; }
        public bool IsOnline { get; set; }
        public Lobby Lobby;
        public List<Player> Spectators = new();
        public Player WhoClicked = null;
        public Cell SelectedCell = null;
        [SerializeField] Piece SelectedPiece;
        public Piece MovedPiece = null;
        public int TurnNumber;
        public Side TurnOf = Side.Bot;
        public List<Move> ValidMoves = new();
        public bool TurnRequiresCapture = false;
        public List<Move> MandatoryMoves = new();
        public bool EnablePlayerControls = false;
        public Dictionary<(int, int), Cell> Cellmap = new();
        [SerializeField] GameObject playerPrefab;
        [SerializeField] LobbyManager LobbyHandler;

        void Awake()
        {
            Game.Events.OnLobbyStart += BeginMatch;
            Game.Events.OnPlayerSelectCell += SelectCell;
            Game.Events.OnRequireCapture += RequireCapture;
            Game.Events.OnCellSelect += SelectCell;
            Game.Events.OnCellDeselect += DeselectPiece;
            Game.Events.OnPieceCapture += SelectMovedPiece;
            Game.Events.OnPieceDone += ChangeTurns;
            Game.Events.OnChangeTurn += ClearMovedPiece;
        }

        void OnDisable()
        {
            Game.Events.OnLobbyStart -= BeginMatch;
            Game.Events.OnPlayerSelectCell -= SelectCell;
            Game.Events.OnRequireCapture -= RequireCapture;
            Game.Events.OnCellSelect -= SelectCell;
            Game.Events.OnCellDeselect -= DeselectPiece;
            Game.Events.OnPieceCapture -= SelectMovedPiece;
            Game.Events.OnPieceDone -= ChangeTurns;
            Game.Events.OnChangeTurn -= ClearMovedPiece;
        }

        void Start()
        {
            // Auto creates a classic match if none created upon starting
            if (Game.Main.Ruleset != null)
            {
                Rules = Game.Main.Ruleset;
            } else
            {
                Rules = new();
            }
            Game.Console.Log($"Created match {Rules.Mode}");
            Game.Events.MatchCreate(this);
            Game.Events.RulesetCreate(Rules);
        }

        public void Init()
        {
            if (IsPlaying) return;
            if (Game.Main.IsHosting)
            {
                StartOnline();
            } else
            {
                StartSolo();
            }
        }

        Player CreatePlayer(Side side)
        {
            Player newPlayer = Instantiate(playerPrefab).GetComponent<Player>();
            newPlayer.SetSide(side);
            newPlayer.SetPlaying(true);
            Game.Events.PlayerCreate(newPlayer);
            return newPlayer;
        }

        void StartSolo()
        {
            CreatePlayer(Side.Bot);
            CreatePlayer(Side.Top);
            BeginMatch(true);
        }

        void StartOnline()
        {
            Lobby = Game.CreateLobby(Rules.Mode);
            Game.Network.StartHost();
            Game.Events.LobbyHost(Lobby);
        }

        public void AddPlayer(ulong clientId)
        {
            Player player;
            if (clientId == Game.Network.LocalClientId)
            {
                player = CreatePlayer(Side.Bot);
            } else
            {
                player = CreatePlayer(Side.Top);
            }
            player.SetClientId(clientId);
            player.GetComponent<NetworkObject>().Spawn();
        }

        public void BeginMatch(bool force = false)
        {
            Game.Events.MatchBegin(this);
            IsPlaying = true;
        }

        public void BeginMatch(Lobby lobby)
        {
            BeginMatch(true);
        }

        public void Reset()
        {
            IsPlaying = true;
            TurnNumber = 1;
            TurnOf = Rules.FirstTurn;
            EnablePlayerControls = true;
        }

        // public void CheckVictoryCondition()
        // {
        //     foreach (var kv in Players)
        //     {
        //         Player player = kv.Value;
        //         if (player.PieceCount <= 0)
        //         {
        //             //
        //             break;
        //         }
        //     }
        // }

        /// <summary>
        /// 
        /// </summary>
        public void RequireCapture(bool value)
        {
            TurnRequiresCapture = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ChangeTurns()
        {
            if (TurnOf == Side.Bot)
            {
                TurnOf = Side.Top;
                // TimerManager.orangeTimer.Begin();
            } else if (TurnOf == Side.Top)
            {
                TurnOf = Side.Bot;
                // TimerManager.blueTimer.Begin();
            }
            TurnNumber++;
            
            Game.Events.ChangeTurn(TurnOf);
            
            // Console.Log($"[GAME]: Changed turns");

            // TimerManager.blueTimer.SetTime(60f);
            // TimerManager.orangeTimer.SetTime(60f);
        }

        public void ChangeTurns(Piece piece)
        {
            ChangeTurns();
        }

        /// <summary>
        /// Perform player checks.
        /// </summary>
        /// <param name="player"></param>
        public bool PerformPlayerChecks(Player player)
        {
            if (!player.IsPlaying) return false;
            if (!player.IsModerator)
            {
                if (TurnOf != player.Side) return false;
                if (player.SelectedCell.HasPiece)
                {
                    if (player.SelectedCell.Piece.Side != player.Side) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Select cell while checking conditions.
        /// </summary>
        /// <param name=""></param>
        public void SelectCell(Player player)
        {
            SelectedCell = player.SelectedCell;

            if (!PerformPlayerChecks(player)) return;
            
            if (SelectedCell.HasPiece)
            {
                if (MovedPiece != null)
                {
                    if (SelectedCell.Piece != MovedPiece) return;
                }

                if (TurnRequiresCapture)
                {
                    if (!SelectedCell.Piece.CanCapture) return;
                }

                SelectPiece(SelectedCell.Piece);
                return;

            } else
            {
                if (SelectedCell.IsValidMove)
                {
                    SelectMove(player);
                }
            }
            DeselectPiece();
        }

        /// <summary>
        /// This force selects the cell.
        /// </summary>
        /// <param name="cell"></param>
        public void SelectCell(Cell cell)
        {
            SelectedCell = cell;

            if (SelectedCell.HasPiece)
            {
                SelectPiece(SelectedCell.Piece);
            } else
            {
                if (SelectedCell.IsValidMove)
                {
                    SelectMove(SelectedCell);
                }
            }
        }

        public void SelectPiece(Piece piece)
        {
            if (SelectedPiece != null)
            {
                Game.Events.PieceDeselect(piece);
            }
            SelectedPiece = piece;
            Game.Events.PieceSelect(piece);
            Game.Audio.PlaySound("Select");
        }
        
        public void SelectMovedPiece(Move move)
        {
            if (MovedPiece != null)
            {
                SelectedPiece = MovedPiece;
                Game.Events.PieceSelect(SelectedPiece);
            }
        }

        public void DeselectPiece()
        {
            Game.Events.PieceDeselect(SelectedPiece);
            SelectedPiece = null;
        }

        public void DeselectPiece(Cell cell)
        {
            DeselectPiece();
        }

        public void DeselectPiece(Piece piece)
        {
            DeselectPiece();
        }

        /// <summary>
        /// Move select by player.
        /// </summary>
        /// <param name="player"></param>
        public void SelectMove(Player player)
        {
            MovedPiece = SelectedPiece;
            Game.Events.PlayerSelectMove(player.SelectedCell);
            Game.Audio.PlaySound("Move");
        }

        public void SelectMove(Cell cell)
        {
            Game.Events.MoveSelect(SelectedCell);
        }

        public void ClearMovedPiece(Side side)
        {
            MovedPiece = null;
        }

        private void GetPlayerCommand(List<string> args)
        {
            string command = string.Join(" ", args.ToArray());

            ExecuteCommandServerRpc(command);
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void ExecuteCommandServerRpc(string command, ServerRpcParams serverRpcParams = default)
        {
            var clientId = serverRpcParams.Receive.SenderClientId;
            if (Game.Network.ConnectedClients.ContainsKey(clientId))
            {
                var client = Game.Network.ConnectedClients[clientId];

                client.PlayerObject.GetComponent<Player>();
            }
        }
    }
}