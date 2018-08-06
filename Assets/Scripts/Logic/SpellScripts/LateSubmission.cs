using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateSubmission : SpellEffect {
	public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
	{
		Player allyPlayer = TurnManager.Instance.whoseTurn;
		new DealDamageCommand(allyPlayer.ID, specialAmount, healthAfter: allyPlayer.Health - specialAmount).AddToQueue();
		allyPlayer.Health -= specialAmount;

	}
}
