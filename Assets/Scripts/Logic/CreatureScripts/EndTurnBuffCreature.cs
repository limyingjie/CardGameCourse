using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnBuffCreature : CreatureEffect {
	
	public EndTurnBuffCreature(Player owner, CreatureLogic creature, int specialAmount) : base(owner, creature, specialAmount)
	{ }
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
		new ChangeStatsCommand(creature.ID, 2, 2, creature.Attack + 2, creature.Health + 2).AddToQueue();
		creature.Attack += 2;
		creature.MaxHealth += 2;
	}

}
