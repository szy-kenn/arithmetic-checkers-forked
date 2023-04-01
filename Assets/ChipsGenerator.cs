using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum values {Integers, Naturals};
public enum preset {Default, Chess}

public class ChipsGenerator : MonoBehaviour
{

    public int maximumColumns = 8;
    public int maximumRows = 8;
    public values values = values.Integers;
    public preset map = preset.Default;

    public Board board;
    [SerializeField] Piece _piecePrefab;
    [SerializeField] float pieceScale = 2.5f;
    [SerializeField] float zLocation = -2;
    [SerializeField] float offset = 1.25f;

    void Start()
    {
        Generate(values);
    }

    void Generate(values gamemode)
    {
        switch (gamemode)
        {
            case values.Integers:
                GenerateIntegers();
                break;
            case values.Naturals:
                break;
        }
    }

    void GenerateIntegers()
    {
        foreach (var pieceData in board.pieceMap)
        {
            int col = pieceData.Item1.Item1;
            int row = pieceData.Item1.Item2;
            Side side = pieceData.Item2;
            Color color = pieceData.Item3;
            string value = pieceData.Item4;
            bool IsKing = pieceData.Item5;
            
            Piece newPiece = Instantiate(_piecePrefab, new Vector3(col, row, zLocation), Quaternion.identity);

            newPiece.transform.SetParent(transform);
            newPiece.name = $"Piece ({col}, {row})";

            newPiece.SetCell(new Vector2(col, row));
            newPiece.SetTeam(side);
            // newPiece.SetColor(color);
            newPiece.SetValue(value);
            newPiece.SetKing(IsKing);
            
            Vector2 cellPosition = new Vector2((col * pieceScale + offset),
                                                (row * pieceScale + offset));
            newPiece.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(cellPosition[0], 
                                                                                    cellPosition[1], 
                                                                                    zLocation);

            // board.pieces.Add(new Vector2(col, row), newPiece);
        }
    }

    // private void GenerateIntegers()
    // {
    //     // Color color = new Color(0, 1, 0, 1);
    //     // List<int> pieceValues = new List<int>
    //     // {
    //     //     -11, 8, 5, 2, 0, -3, 10, 7, -9, 6, -1, 4
    //     // };

    //     // int pieceIndex = 0;
        
    //     // // Generate player 1 pieces
    //     // for (int row = 0; row < maximumRows; row++)
    //     // {
    //     //     for (int col = 0; col < maximumColumns; col++)
    //     //     {
    //     //         if (col % 2 != 0)
    //     //         {
    //     //             continue;
    //     //         }
                
    //     //         var newPiece = Instantiate(_piecePrefab, new Vector3(col, row, zLocation), Quaternion.identity);

    //     //         newPiece.name = $"Piece {color} ({col}, {row}) with value of {pieceValues[pieceIndex]}";
    //     //         newPiece.transform.SetParent(transform);
                
    //     //         Vector2 cellPosition = new Vector2((col * pieceScale) + offset, (row * pieceScale) + offset);
    //     //         newPiece.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(cellPosition[0], cellPosition[1], zLocation);

    //     //         try
    //     //         {
    //     //             newPiece.SetValue(pieceValues[pieceIndex].ToString());
    //     //         } catch
    //     //         {
                    
    //     //         }
    //     //         pieceIndex++;
    //     //     }
    //     // }

    //     // Generate player 2 pieces
    // }
}
