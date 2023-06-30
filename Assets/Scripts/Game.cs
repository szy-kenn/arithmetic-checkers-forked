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
    public Cheats Cheats;
    public Indicators Indicators;
    public List<GameObject> indicators;
    
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

    // Indicators
    public event Action showAllIndicators;
    public event Action hideAllIndicators;

    // Selection
    public event Action<Cell> moveSelectionIndicator;
    public event Action<List<Move>> createMoveIndicators;

    void Start()
    {
        // These shouldn't be here, for testing only

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

        IsPlaying = true;
        turnNumber = 1;
        turn = Side.Bot;
    }

    public void ChangeTurns()
    {
        Debug.Log($"Changed turns");
        CheckForKing(selectedPiece);
        Refresh();

        if (turn == Side.Bot)
        {
            turn = Side.Top;
        } else if (turn == Side.Top)
        {
            turn = Side.Bot;
        }

        turnNumber++;
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
        Debug.Log("Board created with map");
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
        selectedCell = null;
        selectedPiece = null;
        validMoves.Clear();
        Indicators.Selector.Hide();
        Indicators.Clear();

        if (CheatsEnabled)
        {
            Cheats.pieceMenu.Close();
            Cheats.toolsMenu.Close();
        }

        if (refresh != null)
        {
            refresh();
        }
    }

    public void ShowIndicators()
    {
        if (showAllIndicators != null)
        {
            showAllIndicators();
        }
    }

    public void HideIndicators()
    {
        if (hideAllIndicators != null)
        {
            hideAllIndicators();
        }
    }

    public void Select(Player player, Cell cell)
    {
        if (!player.IsModerator)
        {
            if (player.side != turn) return;
        }    

        Debug.Log($"[DEBUG]: {player} clicked on {cell}");
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
                if (cell.piece.HasCapture)
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
            }
        }
    }

    public void SelectPiece(Piece piece)
    {
        selectedPiece = piece;
        validMoves = Board.GetMoves(piece);

        Indicators.Clear();

        if (validMoves.Count != 0)
        {
            Indicators.Selector.Move(selectedCell);
            foreach (Move move in validMoves)
            {
                Debug.Log($"Possible move: {move.destinationCell}");
                Indicators.Create(IndicatorType.Move, move.destinationCell);
            }
        }
    }

    public void SelectMove(Cell cell)
    {
        foreach (Move move in validMoves)
        {
            if (cell == move.destinationCell)
            {
                if (onPieceMove != null)
                {
                    onPieceMove();
                }

                Board.MovePiece(selectedPiece, cell);
                Debug.Log($"{move.HasCapture}");
                Debug.Log($"{move.capturedPiece}");

                if (move.HasCapture)
                {
                    if (onPieceCapture != null)
                    {
                        onPieceCapture();
                    }
                    Debug.Log($"Captured {move.capturedPiece}");
                    Board.Capture(move.capturedPiece);
                    Scoreboard.Compute(move);
                }
                ChangeTurns();
                break;
            } else
            {
                continue;
            }
        }
        Refresh();
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
 