using UnityEngine;
using System.Collections;

public class EndTurnSmallBuff : CreatureEffect
{
    public EndTurnSmallBuff(Player owner, CreatureLogic creature, int specialAmount) : base(owner, creature, specialAmount)
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
        new ChangeStatsCommand(creature.ID, 1, 0, creature.Attack + 1 ,creature.Health + 0).AddToQueue();
        creature.Attack += 1;
        creature.MaxHealth += 0;
    }


}
