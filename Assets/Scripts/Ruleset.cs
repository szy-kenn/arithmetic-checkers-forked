
namespace Damath
{
    /// <summary>
    /// A ruleset defines the rules for a match.
    /// It is created and modified only in the Title Scene.
    /// </summary>
    public class Ruleset
    {
        public enum Type {Standard, Speed, Custom}
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
        // public SymbolMap SymbolMap;
        // public PieceMap PieceMap;

        public Ruleset()
        {
            SetStandard();
        }

        public Ruleset(Type value)
        {
            switch (value)
            {
                case Type.Standard:
                    SetStandard();
                    break;
                    
                case Type.Speed:
                    SetSpeed();
                    break;

                case Type.Custom:
                    SetStandard();
                    break;

                default:
                    SetStandard();
                    break;
            }
        }

        public void SetValue(string rule, int value)
        {

        }

        public void SetStandard()
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