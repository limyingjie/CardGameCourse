using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawIfEmptyHand : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        if (TurnManager.Instance.whoseTurn.hand.CardsInHand.Count <= 1)
        {
            TurnManager.Instance.whoseTurn.DrawACard();
        }
    }
}
