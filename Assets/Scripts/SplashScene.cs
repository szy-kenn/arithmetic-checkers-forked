using UnityEngine;

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