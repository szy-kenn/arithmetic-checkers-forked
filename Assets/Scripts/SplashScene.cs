using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Damath
{
    public class SplashScene : MonoBehaviour
    {
        float splashDuration = 1f;
        
        void Start()
        {
            Game.Main.LoadScene("Title", playTransition: true, delayInSeconds: 1f);
        }
    }
}