using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace Damath
{
    public class Button : MonoBehaviour
    {
        public string text = "Button";
        public Sprite icon = null;
        public bool HasIcon = false;

        UnityEngine.UI.Button button;
        TextMeshProUGUI tmp;
        SpriteRenderer spriteRenderer;

        void Awake()
        {
            button = GetComponent<UnityEngine.UI.Button>();
            tmp = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            spriteRenderer = transform.Find("Icon").GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            tmp.text = text;
        }

        public void SetText(string text)
        {
            this.text = text;
            tmp.text = text;
        }

        public void SetIcon(Sprite icon)
        {
            if (icon != null)
            {
                this.icon = icon;
                spriteRenderer.sprite = icon;
            }
        }

        public void AddListener(UnityAction callback)
        {
            if (button != null)
            {
                button.onClick.AddListener(callback);
            }
        }
    }
}
