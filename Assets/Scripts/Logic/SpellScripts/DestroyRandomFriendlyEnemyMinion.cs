using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRandomFriendlyEnemyMinion : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {

        int enemyCreatures = TurnManager.Instance.whoseTurn.otherPlayer.table.CreaturesOnTable.Count;
        if (enemyCreatures > 0)
        {
            int index = Random.Range(0, enemyCreatures);
            CreatureLogic targetCreature = TurnManager.Instance.whoseTurn.otherPlayer.table.CreaturesOnTable[index];
            new DealDamageCommand(targetCreature.ID, 99, targetCreature.Health - 99).AddToQueue();
            targetCreature.Health -= 99;
        }

        int friendlyCreatures = TurnManager.Instance.whoseTurn.table.CreaturesOnTable.Count;
        if (friendlyCreatures > 0)
        {
            int index = Random.Range(0, friendlyCreatures);
            CreatureLogic targetCreature = TurnManager.Instance.whoseTurn.table.CreaturesOnTable[index];
            new DealDamageCommand(targetCreature.ID, 99, targetCreature.Health - 99).AddToQueue();
            targetCreature.Health -= 99;
        }
    }
}
