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
        public Piece SelectedPiece = null;
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
            Game.Events.OnPlayerLeftClick += CheckPlayer;
            Game.Events.OnRequireCapture += RequireCapture;
            Game.Events.OnPieceDone += ChangeTurns;
        }

        void OnDisable()
        {
            Game.Events.OnPlayerLeftClick -= CheckPlayer;
            Game.Events.OnRequireCapture -= RequireCapture;
            Game.Events.OnPieceDone -= ChangeTurns;
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
        public void ChangeTurns(Piece piece)
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

        public void Refresh()
        {
            Game.Events.Refresh();
        }

        public void SetPlayerClicker(Player player)
        {
            WhoClicked = player;
        }

        /// <summary>
        /// Perform player checks.
        /// </summary>
        /// <param name="player"></param>
        public void CheckPlayer(Player player)
        {
            if (!player.IsPlaying) return;
            if (!player.IsModerator)
            {
                if (TurnOf != player.Side) return;
            }

            SelectCell(player);
        }

        /// <summary>
        /// Cell selection method.
        /// </summary>
        /// <param name="player">The player who selected the cell.</param>
        public void SelectCell(Player player)
        {
            Game.Events.CellSelect(player.SelectedCell);
            SelectedCell = player.SelectedCell;

            // Cell has piece
            if (SelectedCell.HasPiece)
            {
                if (SelectedCell.Piece.Side == player.Side)
                {
                    // // If a piece had previously captured, only select that piece

                    if (!TurnRequiresCapture)
                    {
                        SelectPiece(SelectedCell.Piece);
                        return;
                    } else // Turn requires capture
                    {
                        if (SelectedCell.Piece.CanCapture)
                        {
                            SelectPiece(SelectedCell.Piece);
                        } else
                        {
                            DeselectPiece(SelectedCell.Piece);
                        }
                    }
                } else
                {
                    DeselectPiece(SelectedCell.Piece);
                }
            } else
            {
                if (SelectedCell.IsValidMove)
                {
                    SelectMove(SelectedCell);
                } else
                {
                    DeselectPiece(SelectedCell.Piece);
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

        public void DeselectPiece(Piece piece)
        {
            SelectedPiece = null;
            Game.Events.PieceDeselect(piece);
        }

        /// <summary>
        /// Select move if the cell is a valid move
        /// </summary>
        public void SelectMove(Cell cell)
        {
            Game.Events.MoveSelect(cell);
            Game.Audio.PlaySound("Move");
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