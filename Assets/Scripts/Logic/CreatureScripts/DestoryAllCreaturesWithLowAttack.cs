using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryAllCreaturesWithLowAttack : CreatureEffect {
	public DestoryAllCreaturesWithLowAttack(Player owner, CreatureLogic creature, int specialAmount): base(owner, creature, specialAmount)
	{}
	public override void WhenACreatureIsPlayed(){
		CreatureLogic[] EnemyCreatures = TurnManager.Instance.whoseTurn.otherPlayer.table.CreaturesOnTable.ToArray();
		foreach (CreatureLogic cl in EnemyCreatures)
		{
			if (cl.Attack <=2) {
				new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue();
				cl.Health -= specialAmount;
			}

		}


	}
}
