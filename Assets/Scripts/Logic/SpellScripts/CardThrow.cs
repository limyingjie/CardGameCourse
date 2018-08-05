using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardThrow : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        int cardCount = TurnManager.Instance.whoseTurn.hand.CardsInHand.Count;
        if (cardCount > 0)
        {
            HandVisual PlayerHand = TurnManager.Instance.whoseTurn.PArea.handVisual;
            for (int i = 0; i < cardCount; i++)
            {
                CardLogic cl = TurnManager.Instance.whoseTurn.hand.CardsInHand[0];
                GameObject card = IDHolder.GetGameObjectWithID(cl.UniqueCardID);
                PlayerHand.RemoveCard(card);
                GameObject.Destroy(card);
            }

            int damage = cardCount * specialAmount;
            new DealDamageCommand(TurnManager.Instance.whoseTurn.otherPlayer.PlayerID, damage, TurnManager.Instance.whoseTurn.otherPlayer.Health - damage).AddToQueue();
            TurnManager.Instance.whoseTurn.otherPlayer.Health -= damage;
            //TODO: test
        }
    }
}
