using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovesHandler : MonoBehaviour
{
    public Dictionary<Vector2, Piece> moves;
    public GameObject movesIndicatorPrefab;
    public Color indicatorColor = Color.yellow;
    RectTransform rect;
    SpriteRenderer sprite;

    void Awake()
    {
        
    }

    void Reset()
    {
        moves = null;
        Hide();
    }
    
    void Start()
    {
        rect = GetComponent<RectTransform>();
        sprite = GetComponent<SpriteRenderer>();

        sprite.color = indicatorColor;
    }

    void Display()
    {
        if (moves != null)
        {
            foreach (var move in moves)
            {
                int col = (int)move.Key[0];
                int row = (int)move.Key[1];

                var indicateMove = Instantiate(movesIndicatorPrefab, new Vector3(col, row, Constants.zLocation), Quaternion.identity);
                indicateMove.name = "Move";
                indicateMove.transform.SetParent(transform);
            }
        }
    }

    void Hide()
    {
        // Destroy all move indicator prefabs
    }

    void Add(Vector2 cell, Piece pieceToCapture)
    {
        // Add a move
    }

    void Move(Vector2 cell)
    {
        float x = (float)(cell[0] * 2.5 + 1.25);
        float y = (float)(cell[1] * 2.5 + 1.25);

        rect.anchoredPosition = new Vector2(x, y);
    }

    void ChangeColor()
    {

    }
}
