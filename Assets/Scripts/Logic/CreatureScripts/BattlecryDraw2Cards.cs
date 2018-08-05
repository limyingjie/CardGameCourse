using UnityEngine;
using System.Collections;

public class BattlecryDraw2Cards : CreatureEffect
{
    public BattlecryDraw2Cards(Player owner, CreatureLogic creature, int specialAmount) : base(owner, creature, specialAmount)
    { }

    // BATTLECRY
    public override void WhenACreatureIsPlayed()
    {
        owner.DrawACard();
        owner.DrawACard();
    }
}
