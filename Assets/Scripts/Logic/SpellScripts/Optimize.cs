using UnityEngine;
using System.Collections;

public class Optimize : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        CreatureLogic creature = (CreatureLogic)target;

        int attackAmount = creature.Attack;
        int healthAmount = 1 - creature.Health;
        new ChangeStatsCommand(creature.ID, attackAmount, healthAmount,
            creature.Attack + attackAmount, creature.Health + healthAmount).AddToQueue();
        creature.Attack += attackAmount;
        creature.MaxHealth += healthAmount;
    }
}
