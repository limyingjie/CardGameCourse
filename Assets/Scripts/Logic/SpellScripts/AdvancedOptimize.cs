using UnityEngine;
using System.Collections;

public class AdvancedOptimize : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        // Optimize enemies
        CreatureLogic[] EnemyCreatures = TurnManager.Instance.whoseTurn.otherPlayer.table.CreaturesOnTable.ToArray();
        foreach (CreatureLogic cl in EnemyCreatures)
        {
            optimize(cl);
        }

        // Optimize allies
        CreatureLogic[] AlliedCreatures = TurnManager.Instance.whoseTurn.table.CreaturesOnTable.ToArray();
        foreach (CreatureLogic cl in AlliedCreatures)
        {
            optimize(cl);
        }


    }

    void optimize(CreatureLogic creature) {
        int attackAmount = creature.Attack;
        int healthAmount = 1 - creature.Health;
        new ChangeStatsCommand(creature.ID, attackAmount, healthAmount,
            creature.Attack + attackAmount, creature.Health + healthAmount).AddToQueue();
        creature.Attack += attackAmount;
        creature.MaxHealth += healthAmount;
    }
}
