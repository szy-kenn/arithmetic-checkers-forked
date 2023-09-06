using System.Collections.Generic;
using UnityEngine;

namespace Damath
{
    /// <summary>
    /// Controls the match.
    /// </summary>
    public class MatchController : MonoBehaviour
    {
        public Ruleset Rules;
        
        [Header("Match")]
        public bool IsPlaying = false;
        public Dictionary<Side, Player> Players = new();
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

        [SerializeField] Player playerPrefab;

        void Awake()
        {
            Game.Events.OnPlayerSelectCell += SelectCell;
            Game.Events.OnRequireCapture += RequireCapture;
            Game.Events.OnCellDeselect += DeselectPiece;
            Game.Events.OnPieceCapture += SelectMovedPiece;
            Game.Events.OnPieceDone += ChangeTurns;
            Game.Events.OnChangeTurn += ClearMovedPiece;
            
        }

        void OnDisable()
        {
            Game.Events.OnPlayerSelectCell -= SelectCell;
            Game.Events.OnRequireCapture -= RequireCapture;
            Game.Events.OnCellDeselect -= DeselectPiece;
            Game.Events.OnPieceCapture += SelectMovedPiece;
            Game.Events.OnPieceDone -= ChangeTurns;
            Game.Events.OnChangeTurn -= ClearMovedPiece;
        }

        void Start()
        {
            // Auto creates a classic match if none created upon starting
            if (Game.Main.Ruleset == null)
            {
                Rules = new();
                Game.Events.RulesetCreate(Rules);
            }
            Init();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {

            }
        }


        public void Init()
        {
            if (IsPlaying) return;

            CreatePlayers();

            Game.Events.MatchBegin(this);

            Reset();
        }

        public void Reset()
        {
            IsPlaying = true;
            TurnNumber = 1;
            TurnOf = Rules.FirstTurn;
            EnablePlayerControls = true;
        }

        public void CreatePlayers()
        {
            CreatePlayer(Side.Bot);
            CreatePlayer(Side.Top);
        }

        public Player CreatePlayer(Side side, string name = "Player")
        {
            var newPlayer = Instantiate(playerPrefab);
            newPlayer.SetName(name);
            newPlayer.SetSide(side);
            newPlayer.SetPlaying(true);
            Players.Add(side, newPlayer);
            Game.Events.PlayerCreate(newPlayer);
            return newPlayer;
        }

        public void CheckVictoryCondition()
        {
            foreach (var kv in Players)
            {
                Player player = kv.Value;
                if (player.PieceCount <= 0)
                {
                    //
                    break;
                }
            }
        }

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
        /// 
        /// </summary>
        /// <param name=""></param>
        public void SelectCell(Player player)
        {
            SelectedCell = player.SelectedCell;
            Game.Events.CellSelect(SelectedCell);

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
        /// 
        /// </summary>
        /// <param name="player"></param>
        public void SelectMove(Player player)
        {
            MovedPiece = SelectedPiece;

            Game.Events.MoveSelect(player.SelectedCell);
            Game.Audio.PlaySound("Move");
        }

        public void ClearMovedPiece(Side side)
        {
            MovedPiece = null;
        }

        public void CheckForKing(Piece piece)
        {
            if (piece.Side == Side.Bot)
            {
                if (piece.Row == 7) piece.Promote();
            } else
            {
                if (piece.Row == 0) piece.Promote();
            }
        }

        public void CheckForKings()
        {

        }
    }
}