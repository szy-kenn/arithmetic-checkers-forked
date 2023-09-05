using System.Collections.Generic;
using UnityEngine;

namespace Damath
{
    public class Board : MonoBehaviour
    {
        [Header("Board Settings")]
        [SerializeField] int maximumColumns = 8;
        [SerializeField] int maximumRows = 8;
        public Themes Theme;

        public Ruleset Rules { get; private set; }
        public Dictionary<(int, int), Cell> Cellmap = new();
        public Dictionary<Side, Player> Players = new();
        public Piece SelectedPiece = null;
        public List<Move> ValidMoves = new();
        public MoveType MovesToGet = MoveType.All;
        public bool TurnRequiresCapture = false;

        [Header("Objects")]
        [SerializeField] GameObject grid;
        [SerializeField] GameObject coordinates;
        [SerializeField] GameObject graveyardB;
        [SerializeField] GameObject graveyardT;
        [SerializeField] GameObject boardPrefab;
        [SerializeField] Cell cellPrefab;
        [SerializeField] Piece piecePrefab;

        RectTransform rectTransform;

        void Awake()
        {
            rectTransform = this.GetComponent<RectTransform>();
            Theme = transform.Find("Theme").GetComponent<Themes>();
        }

        void Start()
        {
            //
        }

        void OnEnable()
        {
            Game.Events.OnRulesetCreate += ReceiveRuleset;
            Game.Events.OnMatchBegin += Init;
            Game.Events.OnPieceSelect += SelectPiece;
            Game.Events.OnPieceDeselect += ClearValidMoves;
            Game.Events.OnMoveSelect += SelectMove;
            Game.Events.OnMoveTypeRequest += UpdateMoveType;
            Game.Events.OnRequireCapture += UpdateRequireCaptureState;
            Game.Events.OnPlayerCreate += AddPlayer;
            Game.Events.OnPieceDone += CheckForKing;
            Game.Events.OnPieceDone += ClearValidMoves;
            Game.Events.OnChangeTurn += ClearValidMoves;
        }

        void OnDisable()
        {
            Game.Events.OnRulesetCreate -= ReceiveRuleset;
            Game.Events.OnMatchBegin -= Init;
            Game.Events.OnPieceSelect -= SelectPiece;
            Game.Events.OnPieceDeselect -= ClearValidMoves;
            Game.Events.OnMoveSelect -= SelectMove;
            Game.Events.OnMoveTypeRequest -= UpdateMoveType;
            Game.Events.OnRequireCapture -= UpdateRequireCaptureState;
            Game.Events.OnPlayerCreate -= AddPlayer;
            Game.Events.OnPieceDone -= CheckForKing;
            Game.Events.OnPieceDone -= ClearValidMoves;
            Game.Events.OnChangeTurn -= ClearValidMoves;
        }


        #region Initializing methods
        public void Init(MatchController match)
        {
            GenerateCells();
            GeneratePieces();

            if (Settings.EnableDebugMode)
            {
                Game.Console.Log("[DEBUG]: Board initialized");
            }
        }

        /// <summary>
        /// Generates board cells from the map.
        /// </summary>
        Dictionary<(int, int), Cell>  GenerateCells()
        {
            for (int row = 0; row < maximumRows; row++)
            {
                for (int col = 0; col < maximumColumns; col++)
                {
                    var newCell = Instantiate(cellPrefab, new Vector3(col, row, 0), Quaternion.identity);
                    newCell.name = $"Cell ({col}, {row})";
                    newCell.transform.SetParent(grid.transform);
                    
                    var rect = newCell.GetComponent<RectTransform>();
                    float cellPositionX = col * Constants.CellSize + Constants.CellOffset;
                    float cellPositionY = row * Constants.CellSize + Constants.CellOffset;
                    rect.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(cellPositionX - 0.25f,
                                                                                        cellPositionY - 0.25f,
                                                                                        0); // Idk why, but I didn't have to subtract .25 from this before
                    rect.GetComponent<RectTransform>().localScale = new Vector2(Constants.CellSize, Constants.CellSize);
                    
                    newCell.SetColRow(col, row);
                    if (Rules.Symbols.Map.ContainsKey((col, row)))
                    {
                        newCell.SetOperation(Rules.Symbols.Map[(col, row)]);
                    }
                    Cellmap[(col, row)] = newCell;
                }
            }

            Game.Events.BoardUpdateCellmap(Cellmap);
            return Cellmap;
        }

        /// <summary>
        /// Generates the pieces and assigns it to its respective cell.
        /// </summary>
        void GeneratePieces()
        {
            GameObject pieceGroup = new("Pieces");
            pieceGroup.transform.SetParent(grid.transform);

            foreach (var pieceData in Rules.Pieces.Map)
            {
                int col = pieceData.Key.Item1;
                int row = pieceData.Key.Item2;
                Side side = pieceData.Value.Item1;
                string value = pieceData.Value.Item2;
                bool IsKing = pieceData.Value.Item3;
                
                Cell cell = GetCell(col, row);
    
                Piece newPiece = Instantiate(piecePrefab, new Vector3(col, row, 0), Quaternion.identity);
                newPiece.name = $"Piece ({value})";
                newPiece.transform.SetParent(pieceGroup.transform);
                newPiece.transform.position = cell.transform.position;

                newPiece.SetCell(cell);
                newPiece.SetOwner(Players[side]);
                newPiece.SetTeam(side);
                if (side == Side.Bot)
                {
                    newPiece.SetColor(Theme.botChipColor, Theme.botChipShadow);
                } else
                {
                    newPiece.SetColor(Theme.topChipColor, Theme.topChipShadow);
                }
                newPiece.SetValue(value);
                newPiece.SetKing(IsKing);

                cell.SetPiece(newPiece);
            }
        }
        #endregion

        public void AddPlayer(Player player)
        {
            if (Settings.EnableDebugMode)
            {
                Game.Console.Log($"[BOARD]: Created {player}");
            }
            Players.Add(player.Side, player);
        }

        public void ClearValidMoves()
        {
            if (Settings.EnableDebugMode)
            {
                Debug.Log("[BOARD]: Cleared valid moves.");
            }

            foreach (var move in ValidMoves)
            {
                move.destinationCell.IsValidMove = false;
            }
            ValidMoves.Clear();
        }
        public void ClearValidMoves(Cell cell)
        {
            ClearValidMoves();
        }
        public void ClearValidMoves(Side side)
        {
            ClearValidMoves();
        }

        public void ClearValidMoves(Piece piece)
        {
            ClearValidMoves();
        }

        public void ReceiveRuleset(Ruleset rules)
        {
            Rules = rules;
        }

        /// <summary>
        /// Determines the move type to get.
        /// </summary>
        public void UpdateMoveType(MoveType moveType)
        {
            this.MovesToGet = moveType;
        }

        public void UpdateRequireCaptureState(bool value)
        {
            this.TurnRequiresCapture = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="piece"></param>
        public void SelectPiece(Piece piece)
        {
            SelectedPiece = piece;

            if (!TurnRequiresCapture)
            {
                ValidMoves = GetPieceMoves(piece);
            } else
            {
                ValidMoves = GetPieceMoves(piece, MoveType.Capture);
            }
            
            Game.Events.BoardUpdateValidMoves(ValidMoves);
        }

        public void SelectMove(Cell cell)
        {
            if (ValidMoves.Count == 0) return;

            foreach (Move move in ValidMoves)
            {
                if (cell != move.destinationCell) continue;

                if (cell == move.destinationCell)
                {
                    MovePiece(move);
                    break;
                }
            }
        }

        /// <summary>
        /// Perform a piece move given a Move object.
        /// </summary>
        public void MovePiece(Move move)
        {
            Piece pieceToMove = move.originCell.Piece;
            Game.Events.PieceMove(move);
            Game.Console.Log($"[ACTION]: Moved {pieceToMove.Value}: ({move.originCell.Col}, {move.originCell.Row}) -> ({move.destinationCell.Col}, {move.destinationCell.Row})");
            AnimateMove(move);

            if (!move.HasCapture)
            {
                DonePiece(pieceToMove);
            } else
            {
                CapturePiece(move);
            }
        }
        
        /// <summary>
        /// Perform a piece capture given a move.
        /// </summary>
        public void CapturePiece(Move move)
        {
            Game.Events.PieceCapture(move);
            Game.Audio.PlaySound("Capture");
            Debug.Log($"[ACTION]: {move.capturingPiece} captured {move.capturedPiece}");
            AnimateCapture(move);

            // Check for chain captures
            List<Move> moves = GetPieceMoves(SelectedPiece, MoveType.Capture);

            if (moves.Count == 0) // Has no more succeeding captures
            {
                DonePiece(SelectedPiece);
            } else // Has at least one succeeding capture
            {
                Game.Events.BoardUpdateCaptureables(moves);
                Game.Events.RequireCapture(true);
            }              
        }

        /// <summary>
        /// Marks a piece "done" to end piece actions.
        /// </summary>
        public void DonePiece(Piece piece)
        {
            List<Move> moves = CheckIfCaptureable(piece);

            if (moves.Count == 0) // Piece is NOT captureable
            {
                Game.Events.RequireCapture(false);
                ClearValidMoves();
            } else // Piece is captureable
            {
                Game.Events.BoardUpdateCaptureables(moves);
                Game.Events.RequireCapture(true);
            }
            
            Game.Events.PieceDone(piece);
        }

        /// <summary>
        /// Animates the moved piece in the scene.
        /// </summary>
        void AnimateMove(Move move)
        {
            move.destinationCell.SetPiece(move.originCell.Piece);
            move.originCell.RemovePiece();

            // (move.originCell.Piece, move.destinationCell.Piece) = (move.destinationCell.Piece, move.originCell.Piece);

            if (Settings.EnableAnimations)
            {
                LeanTween.move(move.destinationCell.Piece.gameObject, move.destinationCell.transform.position, 0.5f).setEaseOutExpo();
            }
        }

        /// <summary>
        /// Animates the capture.
        /// </summary>
        void AnimateCapture(Move move)
        {
            Piece capturedPiece = move.capturedPiece;
            RectTransform rect = capturedPiece.GetComponent<RectTransform>();

            if (capturedPiece.Side == Side.Bot) // Blue player captured
            {
                capturedPiece.gameObject.transform.SetParent(graveyardT.transform);
                rect.anchorMin = new Vector2(1f, 0.5f);
                rect.anchorMax = new Vector2(1f, 1f);

                if (Settings.EnableAnimations)
                {
                    LeanTween.move(capturedPiece.gameObject, graveyardT.transform.position, 0.5f).setEaseOutExpo();
                }
            } else // Orange player captured
            {
                capturedPiece.gameObject.transform.SetParent(graveyardB.transform);
                rect.anchorMin = new Vector2(0.5f, 0f);
                rect.anchorMax = new Vector2(0.5f, 0f);

                if (Settings.EnableAnimations)
                {
                    LeanTween.move(capturedPiece.gameObject, graveyardB.transform.position, 0.5f).setEaseOutExpo();
                }
            }

            GetCell(capturedPiece).RemovePiece();
            move.capturingPiece.HasCaptured = true;
            move.capturingPiece.CanCapture = false;
        }

        // /// <summary>
        // /// Checks the piece for any possible actions.
        // /// </summary>
        // public void CheckPiece(Piece piece)
        // {
        //     List<Move> moves;

        //     CheckForKing(piece);
        //     // Check if moved piece is able to be captured
        //     moves = CheckIfCaptureable(piece);
        //     if (moves.Count != 0) // Piece is captureable
        //     {
        //         Game.Events.BoardUpdateValidMoves(moves);
        //         Game.Events.RequireCapture(true);
        //     } else // Piece is NOT captureable
        //     {
        //         DonePiece(SelectedPiece);
        //     }
        // }

        /// <summary>
        /// This checks the 4 surrounding cells of the given piece (SE, SW, NE, NW).
        /// Returns all found moves with captures.
        /// </summary>
        public List<Move> CheckIfCaptureable(Piece piece)
        {
            List<Move> moves = new();

            for (int col = piece.Col - 1 ;  col < piece.Col + 2 ; col += 2)
            {
                if (col < 0 || col > 7) continue;

                for (int row = piece.Row - 1 ;  row < piece.Row + 2 ; row += 2)
                {
                    if (row < 0 || row > 7) continue;

                    Cell cellToCheck = GetCell(col, row);

                    if (cellToCheck.Piece != null)
                    {
                        moves.AddRange(GetPieceMoves(cellToCheck.Piece, MoveType.Capture));
                    }
                }
            }
            return moves;
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

        /// <summary>
        /// Returns the valid moves of the piece.
        /// </summary>
        public List<Move> GetPieceMoves(Piece piece)
        {
            List<Move> moves = new();
            int up = 1;
            int down = -1;
            int above = piece.Row + 1;
            int below = piece.Row - 1;

            if (piece.Side == Side.Bot)
            {
                // Forward check
                moves.AddRange(CheckLeft(piece, above, up, MovesToGet));
                moves.AddRange(CheckRight(piece, above, up, MovesToGet));
                // Backward check
                moves.AddRange(CheckLeft(piece, below, down, MovesToGet));
                moves.AddRange(CheckRight(piece, below, down, MovesToGet));
            } else if (piece.Side == Side.Top)
            {
                // Forward check
                moves.AddRange(CheckLeft(piece, below, down, MovesToGet));
                moves.AddRange(CheckRight(piece, below, down, MovesToGet));
                // Backward check
                moves.AddRange(CheckLeft(piece, above, up, MovesToGet));
                moves.AddRange(CheckRight(piece, above, up, MovesToGet));
            }

            return moves;
        }

        /// <summary>
        /// Returns the valid moves of the piece.
        /// </summary>
        public List<Move> GetPieceMoves(Piece piece, MoveType moveType = default)
        {
            List<Move> moves = new();
            int up = 1;
            int down = -1;
            int above = piece.Row + 1;
            int below = piece.Row - 1;

            if (piece.Side == Side.Bot)
            {
                // Forward check
                moves.AddRange(CheckLeft(piece, above, up, moveType));
                moves.AddRange(CheckRight(piece, above, up, moveType));
                // Backward check
                moves.AddRange(CheckLeft(piece, below, down, moveType));
                moves.AddRange(CheckRight(piece, below, down, moveType));
            } else if (piece.Side == Side.Top)
            {
                // Forward check
                moves.AddRange(CheckLeft(piece, below, down, moveType));
                moves.AddRange(CheckRight(piece, below, down, moveType));
                // Backward check
                moves.AddRange(CheckLeft(piece, above, up, moveType));
                moves.AddRange(CheckRight(piece, above, up, moveType));
            }

            return moves;
        }

        /// <summary>
        /// 
        /// </summary>
        List<Move> CheckLeft(Piece piece, int startingRow, int direction, MoveType moveType)
        {
            List<Move> moves = new();
            List<Move> captureMoves = new();
            Cell cellToCapture = null;
            int nextEnemyPiece = 0;
            int left = piece.Col - 1;

            for (int row = startingRow ; row < maximumRows ; row += direction)
            {
                if (left < 0 || left > 7) break;    //
                if (row < 0 || row > 7) break;      // Out of bounds
                if (nextEnemyPiece > 1) break;      // Two successive pieces

                Cell cellToCheck = GetCell(left, row);

                if (cellToCheck.Piece == null)  // Next cell is empty cell
                {
                    if (cellToCapture != null)  // There's a captureable cell
                    {
                        piece.CanCapture = true;
                        captureMoves.Add(new Move(GetCell(piece.Col, piece.Row), cellToCheck, cellToCapture.Piece));
                        if (piece.IsKing) moves.Clear();
                    } else
                    {
                        if (piece.Forward != direction)
                        {
                            if (!piece.IsKing) break;
                        }
                        moves.Add(new Move(GetCell(piece.Col, piece.Row), cellToCheck));
                    }

                    if (!piece.IsKing) break;

                } else if (cellToCheck.Piece.Side == piece.Side)    // Next cell has allied piece
                {
                    break;
                } else  // Next cell has enemy piece
                {
                    nextEnemyPiece += 1;
                    cellToCapture = cellToCheck;
                }
                left -= 1;  // Move selector diagonally
            }

            // Return moves
            switch (moveType)
            {
                case MoveType.Normal:
                    return moves;
                case MoveType.Capture:
                    return captureMoves;
                default:
                    moves.AddRange(captureMoves);
                    return moves;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        List<Move> CheckRight(Piece piece, int startingRow, int direction, MoveType moveType)
        {
            List<Move> moves = new();
            List<Move> captureMoves = new();
            int nextEnemyPiece = 0;
            Cell cellToCapture = null;
            int right = piece.Col + 1;

            for (int row = startingRow; row < maximumRows ; row += direction)
            {
                if (right < 0 || right > 7) break;      //
                if (row < 0 || row > 7) break;          // Out of bounds
                if (nextEnemyPiece > 1) break;          // Two successive pieces

                Cell cellToCheck = GetCell(right, row);

                if (cellToCheck.Piece == null)  // Next cell is empty cell
                {
                    if (cellToCapture != null)  // There's a captureable cell
                    {
                        piece.CanCapture = true;
                        captureMoves.Add(new Move(GetCell(piece.Col, piece.Row), cellToCheck, cellToCapture.Piece));
                        if (piece.IsKing) moves.Clear();
                    } else
                    {
                        if (piece.Forward != direction)
                        {
                            if (!piece.IsKing) break;
                        }
                        moves.Add(new Move(GetCell(piece.Col, piece.Row), cellToCheck));
                    }

                    if (!piece.IsKing) break;

                } else if (cellToCheck.Piece.Side == piece.Side)    // Next cell has allied piece
                {
                    break;
                } else  // Next cell has enemy piece
                {
                    nextEnemyPiece += 1;
                    cellToCapture = cellToCheck;
                }
                right += 1;  // Move selector diagonally
            }

            // Return moves
            switch (moveType)
            {
                case MoveType.Normal:
                    return moves;
                case MoveType.Capture:
                    return captureMoves;
                default:
                    moves.AddRange(captureMoves);
                    return moves;
            }
        }

        public Cell GetCell(int col, int row)
        {
            return Cellmap[(col, row)];
        }

        public Cell GetCell(Piece piece)
        {
            return Cellmap[(piece.Col, piece.Row)];
        }

        public Dictionary<(int, int), Cell> GetCells()
        {
            return Cellmap;
        }
    }
}