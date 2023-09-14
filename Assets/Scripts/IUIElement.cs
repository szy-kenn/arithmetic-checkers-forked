using System;
using UnityEngine.EventSystems;

namespace Damath
{
    /// <summary>
    /// Base interface for Damath UI elements.
    /// </summary>
    public interface IUIElement : IHoverable
    {
        public bool IsVisible { get; set; }   
        
        public static event EventHandler<PointerEventData> OnTooltipCreate;
    }
}
