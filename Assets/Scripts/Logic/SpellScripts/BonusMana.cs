using UnityEngine;
using System.Collections;

public class BonusMana : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        TurnManager.Instance.whoseTurn.GetBonusMana(specialAmount);
    }
}
