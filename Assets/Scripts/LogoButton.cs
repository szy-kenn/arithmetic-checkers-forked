using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class LogoButton : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private Button button;

    public void Spin()
    {
        LeanTween.cancel(rect);
        LeanTween.rotate(rect, 360f, 1f)
        .setEaseOutExpo()
        .setOnComplete( () =>
        {
            LeanTween.rotate(rect, 0f, 0.5f);
        });
    }
}
