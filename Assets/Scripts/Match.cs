using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Damath
{
    /// <summary>
    /// A match represents a "game" or a "round" of Damath. 
    /// </summary>
    public class Match : MonoBehaviour
    {
        public Ruleset Rules;
        
        [Header("Match")]
        public bool IsPlaying = false;
        public Dictionary<Side, Player> Players = new Dictionary<Side, Player>();
        public Player WhoClicked = null;
        public Cell selectedCell = null;
        public Piece selectedPiece = null;
        public Piece movedPiece = null;
        public int turnNumber;
        public Side turnOf = Side.Bot;
        public List<Move> validMoves = new List<Move>();
        public bool TurnRequiresCapture = false;
        public List<Move> mandatoryMoves = new List<Move>();
        public bool AllowControls = false;
        
        [Header("Objects")]
        public Board Board;
        public Scoreboard Scoreboard;
        public TimerManager TimerManager;
        public IndicatorHandler IndicatorHandler;
        public Cheats Cheats = null;

        [SerializeField] Player playerPrefab;

        void Start()
        {
            this.Rules = Game.Main.ruleset ?? new Ruleset();
            Init();
        }

        void OnEnable()
        {
            Game.Events.OnPlayerSelect += SetPlayerClicker;
            Game.Events.OnCellSelect += SelectCell;
        }

        void OnDisable()
        {
            Game.Events.OnPlayerSelect -= SetPlayerClicker;
            Game.Events.OnCellSelect -= SelectCell;
        }
        
        public void Init()
        {
            if (IsPlaying) return;

            CreatePlayers();
                        
            Game.Events.MatchBegin(Rules);

            // if (Rules.EnableCheats) Cheats.Init();
            Reset();
        }

        public void Reset()
        {
            IsPlaying = true;
            turnNumber = 1;
            turnOf = Side.Bot;
            AllowControls = true;
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

        public void ChangeTurns()
        {
            CheckForKing(selectedPiece);

            if (turnOf == Side.Bot)
            {
                turnOf = Side.Top;
                TimerManager.orangeTimer.Begin();
            } else if (turnOf == Side.Top)
            {
                turnOf = Side.Bot;
                TimerManager.blueTimer.Begin();
            }
            turnNumber++;
            
            Game.Events.ChangeTurn(turnOf);
            Console.Log($"[GAME]: Changed turns");

            if (Rules.EnableMandatoryCapture)
            {
                if (validMoves.Count != 0)
                {
                    foreach (var m in validMoves)
                    {
                        IndicatorHandler.Create(IndicatorType.Box, m.originCell, new Color(0.25f, 0.75f, 0.42f), true);
                    }
                }
            }

            TimerManager.blueTimer.SetTime(60f);
            TimerManager.orangeTimer.SetTime(60f);

            Refresh();
        }

        public void Refresh()
        {
            Game.Events.Refresh();
            
            selectedCell = null;
            selectedPiece = null;

            validMoves.Clear();

            // if (Rules.EnableCheats)
            // {
            //     if (Cheats.pieceMenu != null) Cheats.pieceMenu.Close();
            //     if (Cheats.toolsMenu != null) Cheats.toolsMenu.Close();
            // }
        }

        public void SetPlayerClicker(Player player)
        {
            WhoClicked = player;
        }

        public void Select(Cell cell)
        {
            //
        }

        public void SelectCell(Cell cell)
        {
            if (WhoClicked.IsModerator)
            {
                if (turnOf != WhoClicked.side) return;
            }

            selectedCell = cell;

            if (cell.piece != null)
            {   
                if (WhoClicked.side != cell.piece.side) return;

                // If a piece had previously captured, only select that piece
                if (movedPiece != null)
                {
                    SelectPiece(movedPiece);
                }

                if (TurnRequiresCapture)
                {
                    if (cell.piece.CanCapture)
                    {
                        SelectPiece(selectedCell.piece);
                    }
                } else
                {
                    SelectPiece(selectedCell.piece);
                }
            } else
            {
                if (cell.IsValidMove)
                {
                    SelectMove(selectedCell);
                } else
                {
                    Refresh();
                }
            }
        }

        public void SelectPiece(Piece piece)
        {
            Game.Events.PieceSelect(piece);

            selectedPiece = piece;
            MoveType movesToGet = MoveType.All;

            if (Rules.EnableMandatoryCapture)
            {
                if (TurnRequiresCapture)
                {
                    Game.Events.MoveTypeRequest(MoveType.Capture);
                }
            } else
            {
                Game.Events.MoveTypeRequest(MoveType.All);
            }
            
            validMoves = Board.GetMoves(piece, movesToGet);

            IndicatorHandler.Clear();
            if (validMoves.Count != 0)
            {
                IndicatorHandler.Selector.Move(selectedCell);
                foreach (Move move in validMoves)
                {
                    IndicatorHandler.Create(IndicatorType.Circle, move.destinationCell, Color.yellow);
                }
            }
        }

        /// <summary>
        /// Select move if the cell is a valid move
        /// </summary>
        public void SelectMove(Cell cell)
        {
            if (validMoves.Count == 0) return;

            List<Move> moves = new List<Move>();

            // This works but can be better
            foreach (Move move in validMoves)
            {
                if (cell == move.destinationCell)
                {
                    Game.Events.PieceMove(move);

                    Debug.Log($"[ACTION]: Moved {selectedPiece}: ({move.originCell.col}, {move.originCell.row}) -> ({move.destinationCell.col}, {move.destinationCell.row})");

                    Board.MovePiece(selectedPiece, cell);

                    if (move.HasCapture)
                    {
                        Game.Events.PieceCapture(move);

                        Debug.Log($"[ACTION]: {move.capturingPiece} captured {move.capturedPiece}");

                        IndicatorHandler.ClearAll();

                        // Check for chain captures
                        moves = Board.CheckForCaptures(selectedPiece);
                        if (moves.Count != 0) 
                        {
                            TurnRequiresCapture = true;
                            validMoves = moves;
                            break;
                        }
                    }
                    // Check if moved piece is able to be captured
                    moves = Board.CheckIfCaptureable(selectedPiece);
                    if (moves.Count != 0)
                    {
                        TurnRequiresCapture = true;
                        validMoves = moves;
                    } else
                    {
                        TurnRequiresCapture = false;
                        validMoves.Clear();
                    }
                    ChangeTurns();
                    break;
                }
            }
        }

        public void Deselect()
        {
            selectedCell = null;
            selectedPiece = null;
            validMoves.Clear();
            IndicatorHandler.Selector.Hide();
            IndicatorHandler.Clear();
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