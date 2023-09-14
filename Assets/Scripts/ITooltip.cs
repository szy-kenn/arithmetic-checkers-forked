

namespace Damath
{
    public interface ITooltip : IHoverable
    {
        public bool EnableTooltip { get; set; }
        public string TooltipText { get; set; }
    }
}