using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    public GameObject selectionIndicatorPrefab;
    public Color indicatorColor = Color.yellow;
    RectTransform rect;
    SpriteRenderer sprite;

    void Awake()
    {
        // EventSystem.current.showIndicator += Show;
        // EventSystem.current.hideIndicator += Hide;
        // EventSystem.current.moveIndicator += Move;

        rect = GetComponent<RectTransform>();
        sprite = GetComponent<SpriteRenderer>();

        sprite.color = indicatorColor;
    }

    void Start()
    {

    }

    void Show()
    {
        selectionIndicatorPrefab.SetActive(true);
    }

    void Hide()
    {
        selectionIndicatorPrefab.SetActive(false);
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
