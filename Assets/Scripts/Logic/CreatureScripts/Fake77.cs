using UnityEngine;
using System.Collections;

public class Fake77 : CreatureEffect
{
    public Fake77(Player owner, CreatureLogic creature, int specialAmount) : base(owner, creature, specialAmount)
    { }

    // BATTLECRY
    public override void WhenACreatureIsPlayed()
    {
        int attackAmount = 6;
        int healthAmount = 6;

        new ChangeStatsCommand(creature.ID, attackAmount, healthAmount,
            creature.Attack + attackAmount, creature.Health + healthAmount).AddToQueue();

    }
}
