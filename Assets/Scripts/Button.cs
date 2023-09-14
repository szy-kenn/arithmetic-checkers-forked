using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;
using System;

namespace Damath
{
    public class Button : MonoBehaviour, IUIElement, IHoverable, ITooltip
    {
        public bool IsVisible { get; set; }
        public Sprite Icon = null;
        public bool IsHovered { get; set; }

        [field: Header("Tooltip")]
        public bool EnableTooltip { get; set; }
        [field: TextArea] public string TooltipText { get; set; }

        [SerializeField] private UnityEngine.UI.Button button;
        [SerializeField] private TextMeshProUGUI tmpUGUI;
        
        void Awake()
        {
            button = GetComponent<UnityEngine.UI.Button>();
            tmpUGUI = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHovered = true;
            Game.UI.ShowTooltip(this, TooltipText);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            IsHovered = false;
            Game.UI.HideTooltip();
        }

        public void SetText(string value)
        {
            tmpUGUI.text = value;
        }

        public void SetIcon(Sprite icon)
        {
            if (icon != null)
            {
                Icon = icon;
            }
        }

        public void AddListener(UnityAction function)
        {
            if (button != null)
            {
                button.onClick.AddListener(function);
            }
        }
    }
}
