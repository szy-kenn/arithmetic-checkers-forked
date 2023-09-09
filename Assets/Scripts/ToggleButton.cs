using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;

namespace Damath
{
    public class ToggleButton : MonoBehaviour, IUIElement, ITooltip
    {
        public bool IsVisible { get; set; }
        public Sprite Icon = null;
        public bool Value { get; set; }
        public Tooltip Tooltip { get; set; }
        [field: TextArea, SerializeField] public string TooltipText { get; set; }
        public bool IsHovered { get; set; }
        private UnityEngine.UI.Button button;
        private TextMeshProUGUI tmpUGUI;
  
        void Awake()
        {
            button = GetComponent<UnityEngine.UI.Button>();
            tmpUGUI = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            Tooltip = Game.UI.CreateTooltip(this, TooltipText);
            Value = false;
            AddListener(ToggleValue);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHovered = true;
            Tooltip.SetVisible(true);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            IsHovered = false;
            Tooltip.SetVisible(false);
        }

        public bool GetValue()
        {
            return Value;
        }

        public void ToggleValue()
        {
            Value = !Value;
        }

        public void SetText(string value)
        {
            tmpUGUI.text = value;
        }
        
        public void SetTooltip(string value)
        {
            Tooltip.SetText(value);
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