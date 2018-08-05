using UnityEngine;
using System.Collections;

public class DamageOpponentDeathrattle : CreatureEffect
{
    public DamageOpponentDeathrattle(Player owner, CreatureLogic creature, int specialAmount): base(owner, creature, specialAmount)
    {}

    // DEATHRATTLE
    public override void WhenACreatureDies()
    {
        new DealDamageCommand(owner.otherPlayer.PlayerID, specialAmount, owner.otherPlayer.Health - specialAmount).AddToQueue();
        owner.otherPlayer.Health -= specialAmount;
    }
}
