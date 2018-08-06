using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnBuffFriendlyCreatures : CreatureEffect {
	public EndTurnBuffFriendlyCreatures(Player owner, CreatureLogic creature, int specialAmount):base(owner, creature, specialAmount){
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
		CreatureLogic[] FriendlyCreatures = TurnManager.Instance.whoseTurn.table.CreaturesOnTable.ToArray();
		foreach (CreatureLogic cl in FriendlyCreatures) {
			new ChangeStatsCommand (cl.ID, 1, 1, cl.Attack + 1, cl.MaxHealth + 1).AddToQueue ();
			cl.Attack += 1;
			cl.MaxHealth += 1;
		}
		new ChangeStatsCommand (creature.ID, -1, -1, creature.Attack - 1, creature.MaxHealth -1 ).AddToQueue ();
		creature.Attack -= 1;
		creature.MaxHealth -= 1;
	}
}
