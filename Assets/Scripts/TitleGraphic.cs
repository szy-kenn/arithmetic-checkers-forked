using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleGraphic : MonoBehaviour
{
    [SerializeField] private RectTransform rect;

    void Start()
    {
        LeanTween.move(rect, new Vector3(0f, -30f, 0f), 3f)
        .setEaseInOutSine()
        .setLoopPingPong();
    }
}
