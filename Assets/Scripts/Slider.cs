using UnityEngine;
using UnityEngine.EventSystems;

namespace Damath
{
    public class Slider : MonoBehaviour, IUIElement, IHoverable, ITooltip, IDragHandler
    {
        public bool IsVisible { get; set; }
        public float Value = 0f;
        public float MinRange = 0f;
        public float MaxRange = 1f;
        public Color KnobColor = new(1f, 1f, 1f, 1f);
        public Tooltip Tooltip { get; set; }
        [field: TextArea] public string TooltipText { get; set; }
        public bool IsHovered { get; set; }
        [SerializeField] private RectTransform knobRect;

        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
        }

        public float OnValueChanged()
        {
            return Value;
        }
    }
}
