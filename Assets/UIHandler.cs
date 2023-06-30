using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Main;
    public List<Sprite> icons;
    public GameObject windowPrefab;
    public GameObject choicePrefab;

    void Awake()
    {
        Main = this;
    }

    /// <summary>
    /// Creates an empty window.
    /// </summary>
    public Window CreateWindow()
    {
        var newWindow = Instantiate(windowPrefab);
        newWindow.transform.SetParent(transform);
        return newWindow.GetComponent<Window>();
    }
}
