using UnityEngine;
using System.Collections;

public class DamageRandomOpponentCreature : CreatureEffect
{  
    public DamageRandomOpponentCreature(Player owner, CreatureLogic creature, int specialAmount): base(owner, creature, specialAmount)
    {}

    public override void RegisterEventEffect()
    {
        owner.EndTurnEvent += CauseEventEffect;
        //owner.otherPlayer.EndTurnEvent += CauseEventEffect;
        Debug.Log("Registered shoot effect!!!!");
    }

    public override void UnRegisterEventEffect()
    {
        owner.EndTurnEvent -= CauseEventEffect;
    }

    public override void CauseEventEffect()
    {
        if (owner.otherPlayer.table.CreaturesOnTable.Count > 0)
        {
            int index = Random.Range(0, owner.otherPlayer.table.CreaturesOnTable.Count);
            CreatureLogic targetCreature = owner.otherPlayer.table.CreaturesOnTable[index];
            Debug.Log("Shoot random creature for specialAmount: " + specialAmount);
            new DealDamageCommand(targetCreature.ID, specialAmount, targetCreature.Health - specialAmount).AddToQueue();
            targetCreature.Health -= specialAmount;
        }
    }


}
