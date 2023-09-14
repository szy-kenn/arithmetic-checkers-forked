using System.Collections;
using System.Collections.Generic;
using Damath;
using UnityEngine;

public class ToggleButtonGroup : MonoBehaviour
{
    public List<ToggleButton> Buttons;


    public void Select(ToggleButton button)
    {
        foreach (ToggleButton t in Buttons)
        {
            if (t != button)
            {
                t.SetValue(false);
                continue;
            } else
            {
                t.SetValue(true);
                continue;
            }
        }
    }
}
