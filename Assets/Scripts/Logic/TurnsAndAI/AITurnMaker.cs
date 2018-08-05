using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//this class will take all decisions for AI. 

public class AITurnMaker: TurnMaker {

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        // dispay a message that it is enemy`s turn
        new ShowMessageCommand("Enemy`s Turn!", 2.0f).AddToQueue();
        p.DrawACard();
        StartCoroutine(MakeAITurn());
    }

    // THE LOGIC FOR AI
    protected IEnumerator MakeAITurn()
    {
        bool strategyAttackFirst = false;
        if (Random.Range(0, 2) == 0)
            strategyAttackFirst = true;

        while (MakeOneAIMove(strategyAttackFirst))
        {
            yield return null;
        }

        InsertDelay(1f);

        TurnManager.Instance.EndTurn();
    }

    bool MakeOneAIMove(bool attackFirst)
    {
        if (Command.CardDrawPending())
            return true;
        else if (attackFirst)
            return AttackWithACreature() || PlayACardFromHand() || UseHeroPower();
        else 
            return PlayACardFromHand() || AttackWithACreature() || UseHeroPower();
    }

    bool PlayACardFromHand()
    {
        foreach (CardLogic c in p.hand.CardsInHand)
        {
            if (c.CanBePlayed)
            {
                if (c.ca.MaxHealth == 0)
                {
                    // code to play a spell from hand
                    // TODO: depending on the targeting options, select a random target.
                    if (c.ca.Targets == TargetingOptions.NoTarget)
                    {
                        p.PlayASpellFromHand(c, null);
                        InsertDelay(1.5f);
                        //Debug.Log("Card: " + c.ca.name + " can be played");
                        return true;
                    }
                    // Selecting random target for spell that target enemy chars
                    // NOTE: supposed to be TargetingOptions.EnemyCharacters
                    // but we dont want the AI shooting themselves with their burn spells
                    else if (c.ca.Targets == TargetingOptions.AllCharacters)
                    {
                        int index = Random.Range(0, p.otherPlayer.table.CreaturesOnTable.Count + 2);
                        Debug.Log(index);
                        if (index >= p.otherPlayer.table.CreaturesOnTable.Count)
                        {
                            p.PlayASpellFromHand(c, p.otherPlayer);
                            Debug.Log("AI going face with spell card");
                            InsertDelay(1.5f);
                            return true;
                        }
                        else
                        {
                            CreatureLogic targetCreature = p.otherPlayer.table.CreaturesOnTable[index];
                            //int creatureUniqueID = targetCreature.UniqueCreatureID;
                            p.PlayASpellFromHand(c, targetCreature);
                            Debug.Log("AI targeting random creature with spell card");
                            InsertDelay(1.5f);
                            return true;
                        }
                    }

                    else if (c.ca.Targets == TargetingOptions.YourCreatures)
                    {
                        int index = Random.Range(0, p.table.CreaturesOnTable.Count);
                        Debug.Log(index);

                        CreatureLogic targetCreature = p.table.CreaturesOnTable[index];
                        //int creatureUniqueID = targetCreature.UniqueCreatureID;
                        p.PlayASpellFromHand(c, targetCreature);
                        Debug.Log("AI targeting random friendly creature with spell card");
                        InsertDelay(1.5f);
                        return true;
                    }

                    else if (c.ca.Targets == TargetingOptions.YourCharacters)
                    {
                        int index = Random.Range(0, p.table.CreaturesOnTable.Count + 2);
                        Debug.Log(index);
                        if (index >= p.table.CreaturesOnTable.Count)
                        {
                            p.PlayASpellFromHand(c, p);
                            Debug.Log("AI targeting self with spell card");
                            InsertDelay(1.5f);
                            return true;
                        }
                        else
                        {
                            CreatureLogic targetCreature = p.table.CreaturesOnTable[index];
                            //int creatureUniqueID = targetCreature.UniqueCreatureID;
                            p.PlayASpellFromHand(c, targetCreature);
                            Debug.Log("AI targeting random friendly creature with spell card");
                            InsertDelay(1.5f);
                            return true;
                        }
                    }


                }
                else
                {
                    // it is a creature card
                    p.PlayACreatureFromHand(c, 0);
                    InsertDelay(1.5f);
                    return true;
                }

            }
            //Debug.Log("Card: " + c.ca.name + " can NOT be played");
        }
        return false;
    }

    bool UseHeroPower()
    {
        if (p.ManaLeft >= 2 && !p.usedHeroPowerThisTurn)
        {
            // use HP
            p.UseHeroPower();
            InsertDelay(1.5f);
            //Debug.Log("AI used hero power");
            return true;
        }
        return false;
    }

    bool AttackWithACreature()
    {
        foreach (CreatureLogic cl in p.table.CreaturesOnTable)
        {
            if (cl.AttacksLeftThisTurn > 0)
            {
                // attack a random target with a creature
                if (p.otherPlayer.table.CreaturesOnTable.Count > 0)
                {
                    // find list of the indexes of creatures with taunt
                    List<int> creaturesWithTaunt = new List<int>();
                    for (int i = 0; i < p.otherPlayer.table.CreaturesOnTable.Count; i++) {
                        if (p.otherPlayer.table.CreaturesOnTable[i].ca.Taunt == true) creaturesWithTaunt.Add(i);
                    }
                    if (creaturesWithTaunt.Count == 0)
                    {
                        Debug.Log("No enemy taunt creatures");
                        int index = Random.Range(0, p.otherPlayer.table.CreaturesOnTable.Count);
                        CreatureLogic targetCreature = p.otherPlayer.table.CreaturesOnTable[index];
                        cl.AttackCreature(targetCreature);
                    }
                    else {
                        Debug.Log("Attacking enemy with taunt");
                        int index = Random.Range(0, creaturesWithTaunt.Count);
                        CreatureLogic targetCreature = p.otherPlayer.table.CreaturesOnTable[creaturesWithTaunt[index]];
                        cl.AttackCreature(targetCreature);
                    }

                }                    
                else
                    cl.GoFace();
                
                InsertDelay(1f);
                //Debug.Log("AI attacked with creature");
                return true;
            }
        }
        return false;
    }

    protected void InsertDelay(float delay)
    {
        new DelayCommand(delay).AddToQueue();
    }

}
