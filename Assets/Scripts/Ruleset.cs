using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damath
{
    public enum Gamemode {Classic, Speed, Custom}

    /// <summary>
    /// A ruleset defines the rules for a match.
    /// It is created and modified only in the Title Scene.
    /// </summary>
    public class Ruleset
    {
        public bool EnableCheats;
        public bool EnableMandatoryCapture;
        public bool EnableScoring;
        public bool EnablePromotion;
        public bool EnableChainCapture;
        public bool EnableCapture;
        public bool EnableTimer;
        public bool EnableTurnTimer;
        public bool EnableGlobalTimer;
        public float GlobalTimerSeconds;
        public float TurnTimerSeconds;
        public Side FirstTurn;

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
            GlobalTimerSeconds = 1200f;
            TurnTimerSeconds = 60f;
            FirstTurn = Side.Bot;
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
            GlobalTimerSeconds = 300f;
            TurnTimerSeconds = 15f;
            FirstTurn = Side.Bot;
        }
    }
}