using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AverageFilter : SpellEffect {

	public override void ActivateEffect(int specialAmount = 0, ICharacter target = null){
		CreatureLogic[] EnemyCreatures = TurnManager.Instance.whoseTurn.otherPlayer.table.CreaturesOnTable.ToArray();
		foreach (CreatureLogic cl in EnemyCreatures)
		{
			if (cl.Attack <=2) {
				new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue();
				cl.Health -= specialAmount;
			}
			if (cl.Attack >5) {
				new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue();
				cl.Health -= specialAmount;
			}
		}


		// Dmg allies
		CreatureLogic[] AlliedCreatures = TurnManager.Instance.whoseTurn.table.CreaturesOnTable.ToArray();
		foreach (CreatureLogic cl in AlliedCreatures)
		{
			if (cl.Attack <=2) {
				new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue();
				cl.Health -= specialAmount;
			}
			if (cl.Attack >5) {
				new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue();
				cl.Health -= specialAmount;
			}
		}
	}


}
