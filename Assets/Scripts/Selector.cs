using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public Color color = Color.yellow;
    public Sprite sprite;

    void Awake()
    {
        RectTransform rect = this.GetComponent<RectTransform>();
        Sprite sprite = this.GetComponent<Sprite>();
        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    // Moves selector to cell
    public void Move(Cell cell, bool Show=true)
    {
        this.gameObject.SetActive(Show);
        this.transform.position = cell.transform.position;
    }
}
