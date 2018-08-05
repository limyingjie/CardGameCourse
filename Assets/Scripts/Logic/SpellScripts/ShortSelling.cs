using UnityEngine;
using System.Collections;

public class ShortSelling : SpellEffect
{

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        new DealDamageCommand(target.ID, specialAmount, healthAfter: target.Health - specialAmount).AddToQueue();
        target.Health -= specialAmount;

        new DealDamageCommand(TurnManager.Instance.whoseTurn.otherPlayer.PlayerID, specialAmount, 
            TurnManager.Instance.whoseTurn.otherPlayer.Health - specialAmount).AddToQueue();
        TurnManager.Instance.whoseTurn.otherPlayer.Health -= specialAmount;
    }
}
