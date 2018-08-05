using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBothHero : CreatureEffect
{
    public DamageBothHero(Player owner, CreatureLogic creature, int specialAmount) : base(owner, creature, specialAmount)
    {}

    // BATTLECRY
    public override void WhenACreatureIsPlayed()
    {
        new DealDamageCommand(owner.otherPlayer.PlayerID, specialAmount, owner.otherPlayer.Health - specialAmount).AddToQueue();
        owner.otherPlayer.Health -= specialAmount;
        new DealDamageCommand(owner.PlayerID, specialAmount, owner.Health - specialAmount).AddToQueue();
        owner.Health -= specialAmount;
    }
}

