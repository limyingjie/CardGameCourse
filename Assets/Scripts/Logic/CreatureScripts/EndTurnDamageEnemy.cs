using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnDamageEnemy : CreatureEffect {

	public EndTurnDamageEnemy(Player owner, CreatureLogic creature, int specialAmount):base(owner, creature, specialAmount){
	}

	public override void RegisterEventEffect()
	{
		owner.EndTurnEvent += CauseEventEffect;
		//owner.otherPlayer.EndTurnEvent += CauseEventEffect;
		Debug.Log("Registered buff effect!!!!");
	}

	public override void UnRegisterEventEffect()
	{
		owner.EndTurnEvent -= CauseEventEffect;
	}
	public override void CauseEventEffect()
	{
		CreatureLogic[] EnemyCreatures = TurnManager.Instance.whoseTurn.otherPlayer.table.CreaturesOnTable.ToArray();
		foreach (CreatureLogic cl in EnemyCreatures)
		{
			new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue();
			cl.Health -= specialAmount;
		}
	}


}
