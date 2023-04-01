using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGenerator : MonoBehaviour
{
    public int maximumColumns = 8;
    public int maximumRows = 8;

    [SerializeField]
    private int _width, _height;

    [SerializeField]
    private Cell _cell;

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        for (int col = 0; col < maximumColumns; col++)
        {
            for (int row = 0; row < maximumRows; row++)
            {
                var generatedCell = Instantiate(_cell, new Vector3(col, row, -1f), Quaternion.identity);
                generatedCell.name = $"Cell ({col}, {row})";
                generatedCell.transform.SetParent(transform);

                generatedCell.GetComponent<RectTransform>().localScale = new Vector2(2.5f, 2.5f);
                
                Vector2 cellPosition = new Vector2((col * 2.5f) + 1.25f, (row * 2.5f) + 1.25f);
                generatedCell.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(cellPosition[0], cellPosition[1], -0.1f);
                generatedCell.SetColRow(col, row);
            }
        }
    }
}
