using UnityEngine;
using System.Collections;

//this class will take all decisions for AI. 

public class BossAITurnMaker: AITurnMaker {

    public CardAsset ca1;
    int bossState = 0;

    public override void OnTurnStart()
    {
        //Trigger boss ability at 1st health threshold
        if (bossState == 0 && p.Health <= 20) {
            //p.GetACardNotFromDeck(ca1);
            //p.GetACardNotFromDeck(ca1);
            new ShowMessageCommand("LIMIT BREAK! \n eBot Army", 2.0f).AddToQueue();
            CardLogic cl1 = new CardLogic(ca1);
            CardLogic cl2 = new CardLogic(ca1);
            Debug.Log(cl1.ca.name);
            p.PlayACreatureFromHand(cl1, 0);
            InsertDelay(1.5f);
            p.PlayACreatureFromHand(cl2, 0);
            InsertDelay(1.5f);
            bossState++;
        }

        //Trigger boss ability at 2nd health threshold
        if (bossState == 1 && p.Health <= 10) {
            new ShowMessageCommand("LIMIT BREAK! \n eBot Army", 2.0f).AddToQueue();
            CardLogic cl1 = new CardLogic(ca1);
            CardLogic cl2 = new CardLogic(ca1);
            p.PlayACreatureFromHand(cl1, 0);
            InsertDelay(1.5f);
            p.PlayACreatureFromHand(cl2, 0);
            InsertDelay(1.5f);
            bossState++;
        }

        base.OnTurnStart();
    }

}
