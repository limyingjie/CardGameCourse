using UnityEngine;
using System.Collections;

public class DamageAllCharacters : SpellEffect {

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {   
        // Dmg enemies
        CreatureLogic[] EnemyCreatures = TurnManager.Instance.whoseTurn.otherPlayer.table.CreaturesOnTable.ToArray();
        foreach (CreatureLogic cl in EnemyCreatures)
        {
            new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue();
            cl.Health -= specialAmount;
        }
        Player enemyPlayer = TurnManager.Instance.whoseTurn.otherPlayer;
        new DealDamageCommand(enemyPlayer.ID, specialAmount, healthAfter: enemyPlayer.Health - specialAmount).AddToQueue();
        enemyPlayer.Health -= specialAmount;

        // Dmg allies
        CreatureLogic[] AlliedCreatures = TurnManager.Instance.whoseTurn.table.CreaturesOnTable.ToArray();
        foreach (CreatureLogic cl in AlliedCreatures)
        {
            new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue();
            cl.Health -= specialAmount;
        }
        Player allyPlayer = TurnManager.Instance.whoseTurn;
        new DealDamageCommand(allyPlayer.ID, specialAmount, healthAfter: allyPlayer.Health - specialAmount).AddToQueue();
        allyPlayer.Health -= specialAmount;

    }
}
