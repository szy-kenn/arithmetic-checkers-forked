using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Piece : MonoBehaviour
{
    public (int, int) cell;
    TextMeshPro textMeshPro;
    public int value;
    RectTransform rectTransform;
    public int cellX, cellY;

    public string color;

    public void Refresh()
    {
        
    }

    Piece(int value, string color)
    {
        this.value = value;
        this.color = color;
    }

    public void CalculatePosition()
    {
        cellX = (int)((rectTransform.anchoredPosition.x - 1.25) / 2.5);
        cellY = (int)((rectTransform.anchoredPosition.y - 1.25) / 2.5);
        cell = (cellX, cellY);
    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponentInChildren<TextMeshPro>();

        CalculatePosition();

        EventSystem.current.onPieceClick += SelectMe;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        EventSystem.current.MoveIndicator(cellX, cellY);
        
        EventSystem.current.SelectPiece(this);
    }

    public void SelectMe()
    {
        if(EventSystem.current.selectedPiece == this)
        {
            Debug.Log($"Im Piece{cellX}{cellY}");
        }
    }

    void OnGUI()
    {

    }
}
