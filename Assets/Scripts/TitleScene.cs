using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damath
{
    public class TitleScene : MonoBehaviour
    {
        protected Ruleset ruleset = null;

        void Awake()
        {

        }

        public void SetRuleset(Ruleset ruleset)
        {
            this.ruleset = ruleset;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                this.ruleset = new Ruleset(Ruleset.Gamemode.Classic);
                Console.Log("Created ruleset \"Classic\".");
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                this.ruleset = new Ruleset(Ruleset.Gamemode.Speed);
                Console.Log("Created ruleset \"Speed\".");
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!Game.Main.HasMatch && ruleset != null)
                {
                    Game.Main.CreateMatch(ruleset, start: true);
                }
            }
        }
    }
}
