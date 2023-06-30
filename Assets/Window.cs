using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Window : MonoBehaviour
{
    public List<Choice> choices;
    public bool IsVisible = false;
    RectTransform c_rect;

    void Awake()
    {
        c_rect = GetComponent<RectTransform>();
    }

    public void AddChoice(UnityAction function, string text, Sprite icon=null)
    {
        var newChoice = Instantiate(UIHandler.Main.choicePrefab);
        newChoice.transform.SetParent(transform);
        newChoice.transform.localScale = new Vector3(1f, 1f, 1f);

        Choice c_choice = newChoice.GetComponent<Choice>();
        c_choice.Init(function, text, icon);    
        c_choice.AddListener(Close);    
        choices.Add(c_choice); 
    }

    public void ClearChoices()
    {
        if (choices.Count != 0)
        {
            foreach (Choice c in choices)
            {
                choices.Remove(c);
                Destroy(c.gameObject);
            }
        }
    }

    public void Open(Vector3 position)
    {
        IsVisible = true;
        c_rect.position = position;
        c_rect.pivot = new Vector2(0f, 1f);
        this.gameObject.SetActive(true);

        if (choices.Count != 0)
        {
            foreach (var c in choices)
            {
                c.gameObject.SetActive(true);
            }
        }
    }

    public void Close()
    {
        if (choices.Count != 0)
        {
            foreach (var c in choices)
            {
                c.gameObject.SetActive(false);
            }
        }

        this.gameObject.SetActive(false);
        IsVisible = false;

    }
}
