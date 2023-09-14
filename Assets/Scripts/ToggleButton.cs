using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;

namespace Damath
{
    public class ToggleButton : MonoBehaviour, IUIElement
    {
        public Sprite Icon = null;
        [field: SerializeField] public bool Value { get; set; }
        public bool IsVisible { get; set; }
        public bool IsHovered { get; set; }
        [SerializeField] private UnityEngine.UI.Toggle button;
        [SerializeField] private TextMeshProUGUI tmpUGUI;

        void Start()
        {
            // Tooltip = Game.UI.CreateTooltip(this, TooltipText);
            Value = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHovered = true;
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            IsHovered = false;
        }

        public bool GetValue()
        {
            return Value;
        }

        public void ToggleValue()
        {
            Value = !Value;
        }
        
        public void SetValue(bool value)
        {
            Value = value;
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

        public void AddListener(UnityAction<bool> function)
        {
            if (button != null)
            {
                button.onValueChanged.AddListener(function);
            }
        }
    }
}