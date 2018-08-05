using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DragSpellNoTarget: DraggingActions{

    private int savedHandSlot;
    private WhereIsTheCardOrCreature whereIsCard;
    private OneCardManager manager;

    public override bool CanDrag
    {
        get
        { 
            // TEST LINE: this is just to test playing creatures before the game is complete 
            // return true;

            // TODO : include full field check
            return base.CanDrag && manager.CanBePlayedNow;
        }
    }

    void Awake()
    {
        whereIsCard = GetComponent<WhereIsTheCardOrCreature>();
        manager = GetComponent<OneCardManager>();
    }

    public override void OnStartDrag()
    {
        savedHandSlot = whereIsCard.Slot;

        whereIsCard.VisualState = VisualStates.Dragging;
        whereIsCard.BringToFront();

    }

    public override void OnDraggingInUpdate()
    {
        
    }

    public override void OnEndDrag()
    {
        // 1) Check if we are holding a card over the table
        if (DragSuccessful())
        {
            // play this card
            playerOwner.PlayASpellFromHand(GetComponent<IDHolder>().UniqueID, -1);
        }
        else
        {
            // Set old sorting order 
            whereIsCard.Slot = savedHandSlot;
            if (tag.Contains("Low"))
                whereIsCard.VisualState = VisualStates.LowHand;
            else
                whereIsCard.VisualState = VisualStates.TopHand;
            // Move this card back to its slot position
            HandVisual PlayerHand = playerOwner.PArea.handVisual;
            Vector3 oldCardPos = PlayerHand.slots.Children[savedHandSlot].transform.localPosition;
            transform.DOLocalMove(oldCardPos, 1f);
        } 
    }

    protected override bool DragSuccessful()
    {
        //bool TableNotFull = (TurnManager.Instance.whoseTurn.table.CreaturesOnTable.Count < 8);

        return TableVisual.CursorOverSomeTable; //&& TableNotFull;
    }


}
