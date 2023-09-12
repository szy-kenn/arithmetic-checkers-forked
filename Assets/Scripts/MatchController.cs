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
    public class MatchController : MonoBehaviour
    {
        [Header("Match")]
        public Ruleset Rules;
        public bool IsPlaying { get; set; }
        public bool IsOnline { get; set; }
        public Lobby Lobby;
        [SerializeField] private  Cell SelectedCell = null;
        [SerializeField] private Piece SelectedPiece = null;
        public bool EnablePlayerControls = false;
        [SerializeField] GameObject playerPrefab;
        [SerializeField] LobbyManager LobbyHandler;

        void Awake()
        {
            Game.Events.OnLobbyStart += BeginMatch;
        }

        void OnDisable()
        {
            Game.Events.OnLobbyStart -= BeginMatch;
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
        }

        public void Init()
        {
            if (IsPlaying) return;

            if (Network.Main.IsListening)
            {
                StartOnline();
            } else
            {
                StartSolo();
            }
        }

        public Player CreatePlayer(Side side)
        {
            Player newPlayer = Instantiate(playerPrefab).GetComponent<Player>();
            newPlayer.SetSide(side);
            newPlayer.SetPlaying(true);
            Game.Events.PlayerCreate(newPlayer);
            return newPlayer;
        }

        public void AddPlayer()
        {

        }
        
        void StartOnline()
        {
            Rules = Network.Main.Lobby.Ruleset;

            // This should be called before the match starts (pre-initialization)
            Game.Events.RulesetCreate(Rules);
            BeginMatch(true);
        }

        void StartSolo()
        {
            // This should be called before the match starts (pre-initialization)
            Game.Events.RulesetCreate(Rules);

            CreatePlayer(Side.Bot).IsControllable = true;
            CreatePlayer(Side.Top).IsControllable = true;
            BeginMatch(true);
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

        // public void ChangeTurns(Piece piece)
        // {
        //     ChangeTurns();
        // }

        // /// <summary>
        // /// Selects cell as player.
        // /// </summary>
        // /// <param name=""></param>
        // public void SelectCell(Player player, Cell cell)
        // {
        //     SelectedCell = cell;
        //     Game.Events.CellSelect(cell);
            
        //     if (TurnOf != player.Side) return;
            
        //     if (SelectedCell.HasPiece)
        //     {
        //         SelectPiece(player, cell);

        //         if (SelectedCell.Piece.Side != player.Side) return;

        //         if (MovedPiece != null)
        //         {
        //             if (SelectedCell.Piece != MovedPiece) return;
        //         }

        //         if (TurnRequiresCapture)
        //         {
        //             if (!SelectedCell.Piece.CanCapture) return;
        //         }

        //         SelectPiece(SelectedCell.Piece);
        //         return;

        //     } else
        //     {
        //         if (!SelectedCell.IsValidMove) return;

        //         SelectMove(player);
        //     }
        //     DeselectPiece();
        // }
        
        // public void SelectPiece(Player player, Piece piece)
        // {
        //     if (SelectedPiece != null) Game.Events.PieceDeselect(piece);

        //     player.SelectedPiece = piece;
        //     SelectedPiece = piece;

        //     Game.Events.PieceSelect(piece);
        //     Game.Audio.PlaySound("Select");
        // }

        // public void SelectPiece(Piece piece)
        // {
        //     if (SelectedPiece != null) Game.Events.PieceDeselect(piece);
        //     SelectedPiece = piece;
        //     Game.Events.PieceSelect(piece);
        //     Game.Audio.PlaySound("Select");
        // }

        // public void DeselectPiece()
        // {
        //     Game.Events.PieceDeselect(SelectedPiece);
        //     SelectedPiece = null;
        // }

        // public void DeselectPiece(Cell cell)
        // {
        //     DeselectPiece();
        // }


        // /// <summary>
        // /// Move select by player.
        // /// </summary>
        // /// <param name="player"></param>
        // public void SelectMove(Player player)
        // {
        //     MovedPiece = SelectedPiece;
        //     Game.Events.PlayerSelectMove(player, player.SelectedCell);
        //     Game.Audio.PlaySound("Move");
        // }

        // public void ClearMovedPiece(Side side)
        // {
        //     MovedPiece = null;
        // }

        // private void GetPlayerCommand(List<string> args)
        // {
        //     string command = string.Join(" ", args.ToArray());

        //     ExecuteCommandServerRpc(command);
        // }
        
        // [ServerRpc(RequireOwnership = false)]
        // public void ExecuteCommandServerRpc(string command, ServerRpcParams serverRpcParams = default)
        // {
        //     var clientId = serverRpcParams.Receive.SenderClientId;
        //     if (Game.Network.ConnectedClients.ContainsKey(clientId))
        //     {
        //         var client = Game.Network.ConnectedClients[clientId];

        //         client.PlayerObject.GetComponent<Player>();
        //     }
        // }

        // public void FlipBoard()
        // {

        // }
    }
}