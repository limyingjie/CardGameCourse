using UnityEngine;
using System.Collections;

//this class will take all decisions for AI. 

public class BossAI2TurnMaker: AITurnMaker {

    public CardAsset ca1;
    public CardAsset ca2;
    int bossState = 0;

    public override void OnTurnStart()
    {
        //Trigger boss ability at 1st health threshold
        if (bossState == 0 && p.Health <= 20) {

            new ShowMessageCommand("LIMIT BREAK! \n Diligence", 2.0f).AddToQueue();
            p.GetACardNotFromDeck(ca1);

            //CardLogic cl1 = new CardLogic(ca1);
            //p.PlayASpellFromHand(cl1, null);
            //InsertDelay(1.5f);

            bossState++;
        }

        //Trigger boss ability at 2nd health threshold
        if (bossState == 1 && p.Health <= 10) {
            new ShowMessageCommand("LIMIT BREAK! \n Diligence & Rigor", 2.0f).AddToQueue();
            p.GetACardNotFromDeck(ca1);
            p.GetACardNotFromDeck(ca2);

            //CardLogic cl1 = new CardLogic(ca1);
            ////CardLogic cl2 = new CardLogic(ca2);
            //p.PlayASpellFromHand(cl1, null);
            //InsertDelay(1.5f);
            //p.PlayASpellFromHand(cl2, null);
            //InsertDelay(1.5f);

            bossState++;
        }

        base.OnTurnStart();
    }

}
