using UnityEngine;

namespace Damath
{
    public class TitleScene : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Game.Main.Ruleset == null)
                {
                    Game.Main.CreateMatch(Ruleset.Type.Standard, start: true);
                }
            }
        }
    }
}
