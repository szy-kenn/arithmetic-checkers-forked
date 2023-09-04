using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        public Dictionary<Side, Player> Players = new Dictionary<Side, Player>();
        public Player WhoClicked = null;
        public Cell SelectedCell = null;
        public Piece SelectedPiece = null;
        public Piece MovedPiece = null;
        public int TurnNumber;
        public Side TurnOf = Side.Bot;
        public List<Move> ValidMoves = new List<Move>();
        public bool TurnRequiresCapture = false;
        public List<Move> MandatoryMoves = new List<Move>();
        public bool EnablePlayerControls = false;
        public Dictionary<(int, int), Cell> Cellmap = new();
        
        [Header("Objects")]
        public TimerManager TimerManager;
        public Cheats Cheats = null;

        [SerializeField] Player playerPrefab;

        void OnEnable()
        {
            Game.Events.OnPlayerSelect += SetPlayerClicker;
            Game.Events.OnCellSelect += SelectCell;
            Game.Events.OnCellDeselect += ClearValidMoves;
            Game.Events.OnRequireCapture += RequireCapture;
            Game.Events.OnPieceDone += ChangeTurns;
            Game.Events.OnCellReturn += ReceiveCell;
        }

        void OnDisable()
        {
            Game.Events.OnPlayerSelect -= SetPlayerClicker;
            Game.Events.OnCellSelect -= SelectCell;
            Game.Events.OnCellDeselect -= ClearValidMoves;
            Game.Events.OnRequireCapture -= RequireCapture;
            Game.Events.OnPieceDone -= ChangeTurns;
            Game.Events.OnCellReturn -= ReceiveCell;
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

            // if (Rules.EnableCheats) Cheats.Init();
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

        public void ReceiveCell(Cell cell)
        {
            SelectCell(cell);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RequireCapture(bool value)
        {
            TurnRequiresCapture = value;
        }

        public void ClearValidMoves()
        {
            List<Move> moves = new();
            Game.Events.BoardUpdateMoves(moves);
        }

        public void ClearValidMoves(Cell cell)
        {
            ClearValidMoves();
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

        public void SelectCellAsModerator(Cell cell)
        {
            
        }


        public void SelectCell(int col, int row)
        {
            Game.Events.CellRequest(col, row); 
        }

        /// <summary>
        /// Main.
        /// </summary>
        /// <param name="cell"></param>
        public void SelectCell(Cell cell)
        {
            if (TurnOf != WhoClicked.Side)
            {
                if (!WhoClicked.IsModerator) return;
            }

            SelectedCell = cell;

            // Cell has piece
            if (cell.HasPiece)
            {   
                if (WhoClicked.Side != cell.piece.side)
                {
                    Game.Events.CellDeselect(cell);
                    return;
                }

                // If a piece had previously captured, only select that piece
                if (MovedPiece != null)
                {
                    SelectPiece(MovedPiece);
                    return;
                }

                if (TurnRequiresCapture)
                {
                    if (cell.piece.CanCapture)
                    {
                        SelectPiece(SelectedCell.piece);
                    } else
                    {
                        Game.Events.CellDeselect(cell);
                        return;
                    }
                } else
                {
                    SelectPiece(SelectedCell.piece);
                }
                return;

            } else
            {
                if (cell.IsValidMove)
                {
                    SelectMove(SelectedCell);
                } else
                {
                    Game.Events.CellDeselect(cell);
                    return;
                }
            }
        }

        public void SelectPiece(Piece piece)
        {
            Game.Events.PieceSelect(piece);
        }

        /// <summary>
        /// Select move if the cell is a valid move
        /// </summary>
        public void SelectMove(Cell cell)
        {
            Game.Events.MoveSelect(cell);
        }

        public void CheckForKing(Piece piece)
        {
            if (piece.side == Side.Bot)
            {
                if (piece.row == 7) piece.Promote();
            } else
            {
                if (piece.row == 0) piece.Promote();
            }
        }

        public void CheckForKings()
        {

        }
    }
}