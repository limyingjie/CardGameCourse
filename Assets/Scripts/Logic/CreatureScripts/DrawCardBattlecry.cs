using UnityEngine;
using System.Collections;

public class DrawCardBattlecry : CreatureEffect
{
    public DrawCardBattlecry(Player owner, CreatureLogic creature, int specialAmount): base(owner, creature, specialAmount)
    {}

    // BATTLECRY
    public override void WhenACreatureIsPlayed()
    {   
        owner.DrawACard();
    }
}
