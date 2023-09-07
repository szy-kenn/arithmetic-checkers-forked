
namespace Damath
{
    public interface ITooltip : IHoverable
    {
        Tooltip Tooltip { get; set; }
        string TooltipText { get; set; }
    }
}