using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBurn : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        TurnManager.Instance.whoseTurn.otherPlayer.Overload += specialAmount;
    }
}
