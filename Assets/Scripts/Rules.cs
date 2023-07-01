using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules
{
    public bool EnableMandatoryCapture;
    public bool AllowPromotion;
    public bool AllowChainCapture;
    public bool AllowCapture;
    public bool EnableTimer;
    public bool EnableTurnTimer;
    public bool EnableGlobalTimer;

    public Rules()
    {
        SetDefaults();
    }

    public void SetDefaults()
    {
        EnableMandatoryCapture = true;
        AllowPromotion = true;
        AllowChainCapture = true;
        AllowCapture = true;
        EnableTimer = true;
        EnableTurnTimer = true;
        EnableGlobalTimer = true;
    }
}
