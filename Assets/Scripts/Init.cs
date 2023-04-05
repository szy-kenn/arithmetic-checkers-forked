using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum pieceValues {Integers, Naturals};
public enum pieceMap {Classic, Chess}

public class Init : MonoBehaviour
{
    public pieceValues values;
    public pieceMap map;
    public Dictionary<(int, int), Cell> cells = new Dictionary<(int, int), Cell>();
    // Abstract representation of the initial baord
    public Dictionary<(int, int), Piece> pieces = new Dictionary<(int, int), Piece>();

    [SerializeField] int maximumColumns = 8;
    [SerializeField] int maximumRows = 8;
    [SerializeField] Board board;
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
}
