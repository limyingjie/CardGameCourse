using UnityEngine;
using System.Collections;

public class DrawCardBattlecryDeathrattle : CreatureEffect
{
    public DrawCardBattlecryDeathrattle(Player owner, CreatureLogic creature, int specialAmount): base(owner, creature, specialAmount)
    {}

    // BATTLECRY
    public override void WhenACreatureIsPlayed()
    {
        owner.DrawACard();
    }

    // DEATHRATTLE
    public override void WhenACreatureDies()
    {
        owner.DrawACard();
    }
}
