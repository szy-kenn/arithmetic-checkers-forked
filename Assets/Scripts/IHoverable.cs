using UnityEngine.EventSystems;

namespace Damath
{
    public interface IHoverable : IPointerEnterHandler, IPointerExitHandler
    {
        public bool IsHovered { get; set; }
    }
}
