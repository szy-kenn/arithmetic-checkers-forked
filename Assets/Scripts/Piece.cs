using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System;

public enum Side {Bot, Top}

public class Piece : MonoBehaviour
{
    public Player owner = null;
    public Cell cell;
    public int col, row;
    public Vector3 position;
    public string value;
    public Side side;
    public Color color;
    public bool IsKing = false;
    public bool CanCapture = false;
    public bool HasCaptured = false;
    public int forward = 0;
    public List<Move> moves = new List<Move>();
    public List<Sprite> sprites;

    RectTransform rect;
    TextMeshPro textValue;
    SpriteRenderer overlay;

    public Piece(Side side, Color color, string value, bool IsKing)
    {
        this.side = side;
        this.color = color;
        this.value = value;
        this.IsKing = IsKing;
    }

    public List<Move> GetMoves(Piece piece)
    {
        return new List<Move>();
    }

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        textValue = transform.Find("Text").GetComponent<TextMeshPro>();
        overlay = transform.Find("Overlay").GetComponent<SpriteRenderer>();
    }
    
    void Start()
    {
        textValue.text = value;

        if (IsKing)
        {
            overlay.sprite = sprites[1];
        } else
        {
            overlay.sprite = sprites[0];
        }
    }

    public void SetCell(Cell cell)
    {
        this.cell = cell;
        this.col = cell.col;
        this.row = cell.row;
    }

    public void SetValue(string value)
    {
        this.value = value;
        textValue.text = value;
    }

    public void SetTeam(Side value)
    {
        side = value;

        if (value == Side.Bot)
        {
            forward = 1;
            SetOwner(Game.Main.players[0]);
        } else
        {
            forward = -1;
            SetOwner(Game.Main.players[1]);
        }
    }

    public void SetOwner(Player player)
    {
        owner = player;
    }

    public void SetColor(Color value)
    {
        overlay.color = value;
    }

    public void SetKing(bool value)
    {
        IsKing = value;
    }

    public void AddMove(Move value)
    {
        moves.Add(value);
    }

    public void ClearMoves()
    {
        moves.Clear();
    }

    public void Remove()
    {
        Destroy(this.gameObject);
    }

    public void Promote()
    {
        IsKing = true;
        overlay.sprite = sprites[1];
        textValue.color = new Color(1, 1, 1, 1);
    }

    public void Demote()
    {
        IsKing = false;
        overlay.sprite = sprites[0];
        textValue.color = new Color(0, 0, 0, 1);
    }
}
