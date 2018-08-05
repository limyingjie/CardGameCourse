using UnityEngine;
using System.Collections;

public class HealEndTurn : CreatureEffect
{
    public HealEndTurn(Player owner, CreatureLogic creature, int specialAmount) : base(owner, creature, specialAmount)
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
        new DealDamageCommand(creature.ID, specialAmount, creature.Health - specialAmount).AddToQueue();
        creature.Health -= specialAmount;
    }


}
