using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    public static Game Main;
    public Settings Settings;
    public Board Board;
    public Scoreboard Scoreboard;
    public Rules Rules;
    public Cheats Cheats;
    public IndicatorHandler IndicatorHandler;
    
    [Header("Match")]
    public List<Player> players = new List<Player>();
    public Dictionary<(int, int), Cell> cells = new Dictionary<(int, int), Cell>();
    public bool IsPlaying = false;
    public Cell selectedCell;
    public Piece selectedPiece = null;
    public Piece movedPiece = null;
    public int turnNumber;
    public Side turn = Side.Bot;
    public List<Move> validMoves = new List<Move>();
    public bool TurnRequiresCapture = false;
    public List<Move> mandatoryMoves = new List<Move>();
    public bool CheatsEnabled = true;

    [Header("Prefabs")]
    [SerializeField] Board _boardPrefab;
    [SerializeField] Scoreboard _scoreboardPrefab;
    [SerializeField] Player _playerPrefab;

    void Awake()
    {
        Main = this;
    }

    public event Action initialize;
    public event Action refresh;
    public event Action createBoard;
    public event Action onCellClick;
    public event Action onPieceClick;
    public event Action onPieceMove;
    public event Action onPieceCapture;

    // Selection
    public event Action<Cell> moveSelectionIndicator;
    public event Action<List<Move>> createMoveIndicatorHandler;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        if (initialize != null)
        {
            initialize();
        }

        InitializePlayers();
        Board = CreateBoard(); 
        Scoreboard.Init();
        if (CheatsEnabled) Cheats.Init();
        Rules = new Rules();

        IsPlaying = true;
        turnNumber = 1;
        turn = Side.Bot;
    }

    public void ChangeTurns()
    {
        Debug.Log($"[GAME]: Changed turns");

        CheckForKing(selectedPiece);

        if (turn == Side.Bot)
        {
            turn = Side.Top;
        } else if (turn == Side.Top)
        {
            turn = Side.Bot;
        }
        turnNumber++;

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

        Refresh();
    }

    public Board CreateBoard()
    {
        Debug.Log("Board created");
        var newBoard = Instantiate(_boardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newBoard.transform.SetParent(transform);
        return newBoard;
    }

    public Board CreateBoard(Map map)
    {
        Debug.Log("[DEBUG]: Board created with map");
        var newBoard = Instantiate(_boardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newBoard.transform.SetParent(transform);
        newBoard.Map = map;
        return newBoard;
    }

    public void InitializePlayers()
    {
            var botPlayer = Instantiate(_playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            botPlayer.transform.SetParent(transform);
            botPlayer.side = Side.Bot;

            var topPlayer = Instantiate(_playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            topPlayer.transform.SetParent(transform);
            topPlayer.side = Side.Top;
            
            players.Add(botPlayer);
            players.Add(topPlayer);
    }

    public void Refresh()
    {
        Debug.Log($"Refreshed");
        if (refresh != null)
        {
            refresh();
        }

        selectedCell = null;
        selectedPiece = null;
        validMoves.Clear();
        IndicatorHandler.selector.Hide();
        IndicatorHandler.Clear();

        if (CheatsEnabled)
        {
            if (Cheats.pieceMenu != null) Cheats.pieceMenu.Close();
            if (Cheats.toolsMenu != null) Cheats.toolsMenu.Close();
        }
    }

    public void Select(Player player, Cell cell)
    {
        if (!player.IsModerator)
        {
            if (player.side != turn) return;
        }    

        // Debug.Log($"[DEBUG]: {player} clicked on {cell}");
        if (onCellClick != null)
        {
            onCellClick();
        }

        selectedCell = cell;

        // Cell has piece
        if (cell.piece != null)
        {   
            if (player.side != cell.piece.side) return;

            if (onPieceClick != null)
            {
                onPieceClick();
            }

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
            if (validMoves.Count != 0)
            {
                if (selectedPiece == null) return;
                SelectMove(selectedCell);
            } else
            {
                Refresh();
            }
        }
    }

    public void SelectPiece(Piece piece)
    {
        selectedPiece = piece;
        validMoves = Board.GetMoves(piece);

        IndicatorHandler.Clear();

        if (validMoves.Count != 0)
        {
            IndicatorHandler.selector.Move(selectedCell);
            foreach (Move move in validMoves)
            {
                IndicatorHandler.Create(IndicatorType.Circle, move.destinationCell, Color.yellow);
            }
        }
    }

    public void SelectMove(Cell cell)
    {
        List<Move> moves = new List<Move>();

        // This works but can be better
        foreach (Move move in validMoves)
        {
            if (cell == move.destinationCell)
            {
                Debug.Log($"[ACTION]: Moved {selectedPiece}: ({move.originCell.col}, {move.originCell.row}) -> ({move.destinationCell.col}, {move.destinationCell.row})");
                if (onPieceMove != null)
                {
                    onPieceMove();
                }

                Board.MovePiece(selectedPiece, cell);

                if (move.HasCapture)
                {
                    Debug.Log($"[ACTION]: {move.capturingPiece} captured {move.capturedPiece}");
                    if (onPieceCapture != null)
                    {
                        onPieceCapture();
                    }

                    Board.Capture(move);
                    Scoreboard.Compute(move);
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
                Refresh();
                break;
            }
        }
    }

    public void Deselect()
    {
        if (selectedPiece != null)
        {
            selectedPiece = null;
        }

        Refresh();
    }

    public void CheckForKing(Piece piece)
    {
        if (piece.side == Side.Bot)
        {
            if (piece.row == 7)
            {
                piece.Promote();
            }
        } else
        {
            if (piece.row == 0)
            {
                piece.Promote();
            }
        }
    }

    public void CheckForKings()
    {

    }

    public void MoveSelectionIndicator(Cell cell)
    {
        if (moveSelectionIndicator != null)
        {
            moveSelectionIndicator(cell);
        }
    }
}
 