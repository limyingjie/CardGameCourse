using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayerEndTurn : CreatureEffect
{
	public HealPlayerEndTurn(Player owner, CreatureLogic creature, int specialAmount) : base(owner, creature, specialAmount)
	{ }

	public override void RegisterEventEffect()
	{
		owner.EndTurnEvent += CauseEventEffect;
		//owner.otherPlayer.EndTurnEvent += CauseEventEffect;
		Debug.Log("Registered heal effect!!!!");
	}

	public override void UnRegisterEventEffect()
	{
		owner.EndTurnEvent -= CauseEventEffect;
	}

	public override void CauseEventEffect()
	{
		new DealDamageCommand(owner.ID, specialAmount, owner.Health - specialAmount).AddToQueue();
		owner.Health -= specialAmount;
	}


}
