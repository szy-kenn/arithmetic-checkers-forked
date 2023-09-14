using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damath
{
    public class Settings
    {
        public static bool EnableDebugMode = true;
        public static bool EnableConsole = true;
        public static float masterVolume = 1.0f;
        public static float soundVolume = 1.0f;
        public static float musicVolume = 1.0f;
        public static bool EnableAnimations = true;
        public static float AnimationFactor = 0.5f;
        public static float PieceGrabDelay = 0.1f; // milliseconds

        public class KeyBinds
        {
            public static KeyCode OpenDeveloperConsole = KeyCode.BackQuote;
        }
    }
}
