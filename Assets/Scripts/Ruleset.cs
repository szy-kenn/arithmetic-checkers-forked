using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damath
{
    /// <summary>
    /// A ruleset defines the rules for a match.
    /// It is created and modified only in the Title Scene.
    /// </summary>
    public class Ruleset
    {
        public enum Gamemode {Classic, Speed, Custom}
        public bool EnableCheats;
        public bool EnableMandatoryCapture;
        public bool EnableScoring;
        public bool EnablePromotion;
        public bool EnableChainCapture;
        public bool EnableCapture;
        public bool EnableTimer;
        public bool EnableTurnTimer;
        public bool EnableGlobalTimer;
        public float turnTimerSeconds;
        public float globalTimerSeconds;

        public Ruleset()
        {
            SetClassic();
        }

        public Ruleset(Gamemode mode)
        {
            switch (mode)
            {
                case Gamemode.Classic:
                    SetClassic();
                    break;
                    
                case Gamemode.Speed:
                    SetClassic();
                    break;

                case Gamemode.Custom:
                    SetClassic();
                    break;

                default:
                    SetClassic();
                    break;
            }
        }

        public void SetValue(string rule, int value)
        {

        }

        public void SetClassic()
        {
            EnableMandatoryCapture = true;
            EnablePromotion = true;
            EnableChainCapture = true;
            EnableCapture = true;
            EnableTimer = true;
            EnableTurnTimer = true;
            EnableGlobalTimer = true;
            turnTimerSeconds = 60f;
            globalTimerSeconds = 1200f;
        }

        public void SetSpeed()
        {
            EnableMandatoryCapture = true;
            EnablePromotion = true;
            EnableChainCapture = true;
            EnableCapture = true;
            EnableTimer = true;
            EnableTurnTimer = true;
            EnableGlobalTimer = true;
            turnTimerSeconds = 15f;
            globalTimerSeconds = 300f;
        }
    }
}