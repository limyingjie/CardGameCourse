using UnityEngine;
using System.Collections;

public class Diligence : SpellEffect {

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        CreatureLogic[] CreaturesToDamage = TurnManager.Instance.whoseTurn.otherPlayer.table.CreaturesOnTable.ToArray();
        foreach (CreatureLogic cl in CreaturesToDamage)
        {
            if (cl.Attack < 4) // <4 ATK
            {
                new DealDamageCommand(cl.ID, 99, healthAfter: cl.Health - 99).AddToQueue();
                cl.Health -= 99;
            }
        }
    }
}
