using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum pieceValues {Integers, Naturals};
public enum pieceMap {Classic, Chess}

public enum MoveType {Normal, Capture}

public class Init : MonoBehaviour
{
    public pieceValues values;
    public pieceMap map;
    public Dictionary<(int, int), Cell> cells = new Dictionary<(int, int), Cell>();
    // Abstract representation of the initial baord
    public Dictionary<(int, int), Piece> pieces = new Dictionary<(int, int), Piece>();

    [SerializeField] int maximumColumns = 8;
    [SerializeField] int maximumRows = 8;
    [SerializeField] Cell _cellPrefab;
    [SerializeField] Piece _piecePrefab;
    
    void Start()
    {
        GenerateCells();
        GeneratePieces();
    }

    void GeneratePieces()
    {
        switch (values)
        {
            case pieceValues.Integers:
                GenerateIntegers();
                break;
            case pieceValues.Naturals:
                break;
        }
    }

    void GenerateCells()
    {
        for (int row = 0; row < maximumRows; row++)
        {
            for (int col = 0; col < maximumColumns; col++)
            {
                var newCell = Instantiate(_cellPrefab, new Vector3(col, row, 0), Quaternion.identity);
                newCell.name = $"Cell ({col}, {row})";
                newCell.transform.SetParent(transform);
                newCell.GetComponent<RectTransform>().localScale = new Vector2(Constants.cellSize, Constants.cellSize);
                
                float cellPositionX = col * Constants.cellSize + Constants.cellOffset;
                float cellPositionY = row * Constants.cellSize + Constants.cellOffset;
                newCell.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(cellPositionX - 0.25f,
                                                                                             cellPositionY - 0.25f,
                                                                                             Constants.cellZLocation); // Idk why, but I didn't have to subtract .25 from this before
                newCell.SetColRow(col, row);
                cells.Add((col, row), newCell);
            }
        }
    }

    void GenerateIntegers()
    {
        foreach (var pieceData in MapData.classic)
        {
            int col = pieceData.Key.Item1;
            int row = pieceData.Key.Item2;
            Side side = pieceData.Value.Item1;
            Color color = pieceData.Value.Item2;
            string value = pieceData.Value.Item3;
            bool IsKing = pieceData.Value.Item4;
            
            Piece newPiece = Instantiate(_piecePrefab, new Vector3(col, row, 0), Quaternion.identity);
            newPiece.name = $"Piece ({col}, {row})";
            newPiece.transform.SetParent(transform);
            cells[(col, row)].SetPiece(newPiece); 
            // cells[(col, row)].piece.transform.SetParent(cells[(col, row)].transform);

            newPiece.SetTeam(side);
            newPiece.SetColor(color);
            newPiece.SetValue(value);
            newPiece.SetKing(IsKing);
            
            float cellPositionX = (col * Constants.pieceScale + Constants.cellOffset);
            float cellPositionY = (row * Constants.pieceScale + Constants.cellOffset);
            newPiece.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(cellPositionX - 0.25f, 
                                                                                    cellPositionY - 0.25f, 
                                                                                    Constants.pieceZLocation);
        }
    }

    public void GetMoves(Piece piece, MoveType moveType)
    {
        int up = 1;
        int down = -1;

        if (piece.side == Side.Bot)
        {
            CheckLeft(piece, up, moveType);
            CheckRight(piece, up, moveType);
        } else if (piece.side == Side.Top)
        {
            CheckLeft(piece, down, moveType);
            CheckRight(piece, down, moveType);
        }
    }

    Dictionary<(int, int), Piece> CheckLeft(Piece piece, int direction, MoveType moveType)
    {
        Dictionary<(int, int), Piece> moves = new Dictionary<(int, int), Piece>();

        int col = piece.col - 1;
        int row = piece.row + direction;
        int nextEnemyPiece = 0;
        List<Piece> captureables = new List<Piece>();

        while (row < maximumRows)
        {
            if (col < 0) break;
            if (nextEnemyPiece >= 2) break;

            Cell cellToCheck = cells[(col, row)];

            if (cellToCheck.piece == null)
            {
                //
            } else
            {
                if (cellToCheck.piece.side == piece.side)
                {
                    break;
                } else
                {
                    nextEnemyPiece++;
                    captureables.Add(cells[(col, row)].piece);
                }
            }

            col--;
        }

        return moves;
    }

    void CheckRight(Piece piece, int direction, MoveType moveType)
    {
        int right = -1;
    }
}
