using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int x, y;
    public int col, row;
    
    public Vector2 cell;


    void Awake()
    {
        EventSystem.current.onCellClick += SelectMe;
    }

    void OnMouseDown()
    {
        EventSystem.current.Select(this);
    }

    void SelectMe()
    {
        if (EventSystem.current.selectedCell == this)
        {
        }
    }

    public void SetColRow(int colValue, int rowValue)
    {
        x = colValue;
        y = row;
        col = colValue;
        row = rowValue;
        cell = new Vector2(colValue, rowValue);
    }
}
