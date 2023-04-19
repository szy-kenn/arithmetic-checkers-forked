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

        rect = GetComponent<RectTransform>();
        sprite = GetComponent<SpriteRenderer>();

        sprite.color = indicatorColor;
    }

    void Start()
    {
        EventSystem.current.moveIndicator += MoveTo;

    }

    void Show()
    {
        selectionIndicatorPrefab.SetActive(true);
    }

    void Hide()
    {
        selectionIndicatorPrefab.SetActive(false);
    }

    void MoveTo(Cell cell)
    {
        selectionIndicatorPrefab.transform.position = cell.transform.position;
    }

    void ChangeColor()
    {

    }
}
