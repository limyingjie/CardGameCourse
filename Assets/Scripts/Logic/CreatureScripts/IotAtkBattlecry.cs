using UnityEngine;
using System.Collections;

public class IotAtkBattlecry : CreatureEffect
{
    public IotAtkBattlecry(Player owner, CreatureLogic creature, int specialAmount) : base(owner, creature, specialAmount)
    { }

    // BATTLECRY
    public override void WhenACreatureIsPlayed()
    {
        int attackAmount = -1;
        int healthAmount = 0;

        //it counts itself as well
        CreatureLogic[] AlliedCreatures = TurnManager.Instance.whoseTurn.table.CreaturesOnTable.ToArray();
        foreach (CreatureLogic cl in AlliedCreatures)
        {
            if (cl.ca.name.StartsWith("IoT")) attackAmount += 1;
        }

        new ChangeStatsCommand(creature.ID, attackAmount, healthAmount,
            creature.Attack + attackAmount, creature.Health + healthAmount).AddToQueue();
        creature.Attack += attackAmount;
        creature.MaxHealth += healthAmount;

    }
}
