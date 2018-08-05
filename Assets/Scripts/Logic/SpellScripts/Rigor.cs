using UnityEngine;
using System.Collections;

public class Rigor : SpellEffect
{

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        CreatureLogic[] CreaturesToBuff = TurnManager.Instance.whoseTurn.table.CreaturesOnTable.ToArray();
        foreach (CreatureLogic cl in CreaturesToBuff)
        {
            new ChangeStatsCommand(cl.ID, 0, 2, cl.Attack, cl.Health + 2).AddToQueue();
            cl.MaxHealth += 2;

        }
        new DealDamageCommand(TurnManager.Instance.whoseTurn.ID, -5, TurnManager.Instance.whoseTurn.Health + 5).AddToQueue();
        TurnManager.Instance.whoseTurn.Health += 5;
    }
}
