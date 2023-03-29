using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public GameObject indicator;
    RectTransform rectTransform;

    void Awake()
    {
    }

    void Start()
    {
        EventSystem.current.moveIndicator += Move;
        rectTransform = GetComponent<RectTransform>();
    }

    void Show()
    {
        indicator.SetActive(true);
    }

    void Hide()
    {
        indicator.SetActive(false);
    }

    void Move(int cellX, int cellY)
    {
        float x = (float)((cellX * 2.5) + 1.25);
        float y = (float)((cellY * 2.5) + 1.25);

        Debug.Log($"Moved indicator to ({cellX}, {cellY}).");
        rectTransform.anchoredPosition = new Vector2(x, y);
    }
}
