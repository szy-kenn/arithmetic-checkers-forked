using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public Color color;
    public Sprite sprite;
    public bool ForceDisplay = false;
    public RectTransform c_rect;
    public Sprite c_sprite;
    public SpriteRenderer c_spriteRenderer;

    void Awake()
    {
        c_rect = GetComponent<RectTransform>();
        c_sprite = GetComponent<Sprite>();
        c_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Move(Cell cell)
    {
        gameObject.SetActive(true);
        transform.position = cell.transform.position;
    }

    public void Delete()
    {
        Destroy(this);
    }

    public void SetColor(Color value)
    {
        c_spriteRenderer.color = value;
    }
}
