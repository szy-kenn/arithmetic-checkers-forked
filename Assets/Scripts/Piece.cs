using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System;

public enum Side {Bot, Top}

public class Piece : MonoBehaviour
{
    public int col, row;
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
    
    public void CalculatePosition(Vector2 cell)
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
}
