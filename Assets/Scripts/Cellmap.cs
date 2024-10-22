using System.Collections.Generic;

namespace Damath
{
    public class Cellmap<T>
    {
        public Dictionary<(int, int), T> Map { get; set; }

        public void SetMap(Dictionary<(int, int), T> value)
        {
            Map = value;
        }
    }
}