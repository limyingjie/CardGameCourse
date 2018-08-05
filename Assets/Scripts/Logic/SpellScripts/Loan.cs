using UnityEngine;
using System.Collections;

public class Loan : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        TurnManager.Instance.whoseTurn.DrawACard();
        TurnManager.Instance.whoseTurn.DrawACard();

        new DealDamageCommand(TurnManager.Instance.whoseTurn.otherPlayer.PlayerID, -7, TurnManager.Instance.whoseTurn.otherPlayer.Health + 7).AddToQueue();
        TurnManager.Instance.whoseTurn.otherPlayer.Health += 7;
    }
}
