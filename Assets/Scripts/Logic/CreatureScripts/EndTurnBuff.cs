using UnityEngine;
using System.Collections;

public class EndTurnBuff : CreatureEffect
{
    public EndTurnBuff(Player owner, CreatureLogic creature, int specialAmount) : base(owner, creature, specialAmount)
    {}

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
        new ChangeStatsCommand(creature.ID, 2, 1, creature.Attack + 2, creature.Health + 1).AddToQueue();
        creature.Attack += 2;
        creature.MaxHealth += 1;
    }
}
