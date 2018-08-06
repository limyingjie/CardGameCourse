using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxPooling : SpellEffect {

	public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
	{   
		// Dmg enemies
		CreatureLogic[] EnemyCreatures = TurnManager.Instance.whoseTurn.otherPlayer.table.CreaturesOnTable.ToArray();
		int highestAttackValue = -1;
		foreach (CreatureLogic cl in EnemyCreatures)
		{
			if (cl.Attack > highestAttackValue) {
				highestAttackValue = cl.Attack;
			} 

		}
		int count = 0;
		foreach (CreatureLogic cl in EnemyCreatures) {
			if (cl.Attack != highestAttackValue) {
				specialAmount = cl.Health;
				new DealDamageCommand (cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue ();
				cl.Health -= specialAmount;
			} else {
				count += 1;
				if(count>1){
					specialAmount = cl.Health;
					new DealDamageCommand (cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue ();
					cl.Health -= specialAmount;
				}
			}

		}

	    highestAttackValue = 0;
		// Dmg allies
		CreatureLogic[] AlliedCreatures = TurnManager.Instance.whoseTurn.table.CreaturesOnTable.ToArray();
		foreach (CreatureLogic cl in AlliedCreatures)
		{
			if (cl.Attack >= highestAttackValue) {
				highestAttackValue = cl.Attack;
			}

		}
		count = 0;
		foreach (CreatureLogic cl in AlliedCreatures) {
			if (cl.Attack != highestAttackValue) {
				specialAmount = cl.Health;
				new DealDamageCommand (cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue ();
				cl.Health -= specialAmount;
			} else {
				count += 1;
				if(count>1){
					specialAmount = cl.Health;
					new DealDamageCommand (cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue ();
					cl.Health -= specialAmount;
				}
			}
		}
	
	}
}