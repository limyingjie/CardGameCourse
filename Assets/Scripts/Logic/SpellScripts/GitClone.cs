using System.Collections;
using UnityEngine;

public class GitClone : SpellEffect {
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        CreatureLogic creature = (CreatureLogic)target;
        TurnManager.Instance.whoseTurn.GetACardNotFromDeck(creature.ca);
    }
}
