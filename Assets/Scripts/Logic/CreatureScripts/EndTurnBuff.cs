using UnityEngine;
using System.Collections;

public class EndTurnBuff : CreatureEffect
{
    public EndTurnBuff(Player owner, CreatureLogic creature, int specialAmount) : base(owner, creature, specialAmount)
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
        //new ChangeStatsCommand(creature.ID, specialAmount, owner.Health - specialAmount).AddToQueue();
        //owner.Health -= specialAmount;
    }


}
