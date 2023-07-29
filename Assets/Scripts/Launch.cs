using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damath
{
    public class Launch : MonoBehaviour
    {
        void Start()
        {
            Game.Main.LoadScene("Splash");
        }
    }
}
