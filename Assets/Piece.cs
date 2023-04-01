using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System;

public enum Side {Bot, Top}

public class Move
{

}

public class Piece : MonoBehaviour
{
    public int col, row;
    public Vector2 cell;
    public Vector3 position;
    public string value;
    public Side side;
    public Color color;
    public bool IsKing = false;
    public bool HasSkipped = false;
    public bool CanCapture = false;

    RectTransform rect;
    TextMeshPro textValue;
    SpriteRenderer overlay;

    public void Refresh()
    {
        
    }

    public void CalculatePosition()
    {
        // cellX = (int)((rectTransform.anchoredPosition.x - offset) / cellSize);
        // cellY = (int)((rectTransform.anchoredPosition.y - offset) / cellSize);
        // cell = new Vector2(cellX, cellY);

        // cell = 

        // rectTransform.anchoredPosition3D = new Vector3( , , zLocation);
    }

    // Start is called before the first frame update
    void Awake()
    {
        EventSystem.current.onPieceClick += SelectMe;

        rect = GetComponent<RectTransform>();
        textValue = transform.Find("Text").GetComponent<TextMeshPro>();
        overlay = transform.Find("Overlay").GetComponent<SpriteRenderer>();
    }
    
    void Start()
    {

        if (textValue != null)
        {
            textValue.text = value;
        }

        CalculatePosition();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // On piece click event
    void OnMouseDown()
    {
        EventSystem.current.Select(this);
    }

    public void SelectMe()
    {
        if (EventSystem.current.selectedPiece == this)
        {
            EventSystem.current.HasPieceSelected = true;
        }
    }

    public void SetCell(Vector2 value)
    {
        col = (int)value[0];
        row = (int)value[1];
        cell = value;
    }

    public void SetValue(string value)
    {
        textValue.text = value;
    }

    public void SetTeam(Side value)
    {
        side = value;
    }

    public void SetColor(Color value)
    {
        overlay.color = value;
    }

    public void SetKing(bool value)
    {
        IsKing = value;
    }

    public void MoveToCell(Vector2 destinationCell)
    {
        cell = destinationCell;
        CalculatePosition();
    }
    
    public void Capture()
    {
        EventSystem.current.onPieceClick -= SelectMe;
    }

}
