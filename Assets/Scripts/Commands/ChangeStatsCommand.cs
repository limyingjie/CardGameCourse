using UnityEngine;
using System.Collections;

public class ChangeStatsCommand : Command {

    private int targetID;
    private int attackAmount;
    private int healthAmount;
    private int attackAfter;
    private int healthAfter;

    public ChangeStatsCommand( int targetID, int attackAmount, int healthAmount, int attackAfter, int healthAfter)
    {
        this.targetID = targetID;
        this.attackAmount = attackAmount;
        this.healthAmount = healthAmount;
        this.attackAfter = attackAfter;
        this.healthAfter = healthAfter;
    }

    public override void StartCommandExecution()
    {
        Debug.Log("In change stats command!");

        GameObject target = IDHolder.GetGameObjectWithID(targetID);

        // target is a creature
        target.GetComponent<OneCreatureManager>().ChangeStats(attackAmount, healthAmount, attackAfter, healthAfter);

        CommandExecutionComplete();
    }
}
