using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAllCreatures : SpellEffect {

	public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
	{   
		// Dmg enemies
		CreatureLogic[] EnemyCreatures = TurnManager.Instance.whoseTurn.otherPlayer.table.CreaturesOnTable.ToArray();
		foreach (CreatureLogic cl in EnemyCreatures)
		{
			new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue();
			cl.Health -= specialAmount;
		}


		// Dmg allies
		CreatureLogic[] AlliedCreatures = TurnManager.Instance.whoseTurn.table.CreaturesOnTable.ToArray();
		foreach (CreatureLogic cl in AlliedCreatures)
		{
			new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue();
			cl.Health -= specialAmount;
		}


	}
}
