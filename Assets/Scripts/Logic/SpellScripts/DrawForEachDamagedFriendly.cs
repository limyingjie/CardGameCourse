using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawForEachDamagedFriendly : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        int drawCount = 0;

        CreatureLogic[] AlliedCreatures = TurnManager.Instance.whoseTurn.table.CreaturesOnTable.ToArray();
        foreach (CreatureLogic cl in AlliedCreatures)
        {
            if (cl.Health < cl.MaxHealth) drawCount += 1;
        }
        if (TurnManager.Instance.whoseTurn.Health < TurnManager.Instance.whoseTurn.charAsset.MaxHealth) drawCount += 1;

        for (int i = 0; i < drawCount; i++)
        {
            TurnManager.Instance.whoseTurn.DrawACard();
        }
    }
}
