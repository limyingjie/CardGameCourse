using UnityEngine;
using System.Collections;

public class ChangeStatsTarget : SpellEffect
{

    /// <summary>
    /// WARNING: The param specialAmount is fucked up, and must follow special conventions. <para/>
    /// It must be a 4 digit postive number. <para/>
    /// 1st digit: Sign digit for attack change. 0: -ve, 1: +ve <para/>
    /// 2nd digit: Magnitude for attack change (0-9) <para/>
    /// 3rd digit: Sign digit for health change. 0: -ve, 1: +ve <para/>
    /// 4th digit: Magnitude for health change (0-9) <para/>
    /// Example: 213 (read as 0213) means +2/-3
    /// </summary>
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        CreatureLogic creature = (CreatureLogic) target;
        bool attackSign = (specialAmount / 1000 % 10) != 0;
        int attackMagnitude = (specialAmount / 100 % 10);
        bool healthSign = (specialAmount / 10 % 10) != 0;
        int healthMagnitude = (specialAmount / 1 % 10);

        int attackAmount = attackMagnitude;
        if (attackSign) attackAmount = -attackAmount;
        int healthAmount = healthMagnitude;
        if (healthSign) healthAmount = -healthAmount;

        new ChangeStatsCommand(creature.ID, attackAmount, healthAmount , 
            creature.Attack + attackAmount, creature.Health + healthAmount).AddToQueue();
        creature.Attack += attackAmount;
        creature.MaxHealth += healthAmount;
    }
}
