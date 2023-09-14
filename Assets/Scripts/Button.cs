using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace Damath
{
    public class Button : MonoBehaviour, IUIElement, ITooltip
    {
        public bool IsVisible { get; set; }
        public Sprite Icon = null;
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
            // Tooltip = Game.UI.CreateTooltip(this, TooltipText);
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
