using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Damath
{
    public class Slider : MonoBehaviour, IUIElement, IHoverable, IDragHandler, IPointerDownHandler
    {
        public bool IsVisible { get; set; }
        [Range(0, 1)] public float Value = 0f;
        public Color KnobColor = new (1f, 1f, 1f, 1f);
        public Color OnColor = new (1f, 1f, 1f, 1f);
        public Color OffColor = new (1f, 1f, 1f, 1f);
        public bool IsHovered { get; set; }

        public event Action<float> OnValueChanged;
        [SerializeField] private RectTransform knobRect;
        public Tooltip Tooltip { get; set; }
        [field: TextArea] public string TooltipText { get; set; }

        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("clicked slider");
            knobRect.anchoredPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
        }

        public void AddListener(Action<float> action)
        {
            OnValueChanged += action;
        }

        public float ValueChange()
        {
            OnValueChanged?.Invoke(Value);
            return Value;
        }
    }
}
