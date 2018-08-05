using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class HandVisual : MonoBehaviour
{
    // PUBLIC FIELDS
    public AreaPosition owner;
    public bool TakeCardsOpenly = true;
    public SameDistanceChildren slots;

    [Header("Transform References")]
    public Transform DrawPreviewSpot;
    public Transform DeckTransform;
    public Transform OtherCardDrawSourceTransform;
    public Transform PlayPreviewSpot;

    // PRIVATE : a list of all card visual representations as GameObjects
    private List<GameObject> CardsInHand = new List<GameObject>();

    // ADDING OR REMOVING CARDS FROM HAND

    // add a new card GameObject to hand
    public void AddCard(GameObject card)
    {
        // we allways insert a new card as 0th element in CardsInHand List 
        CardsInHand.Insert(0, card);

        // parent this card to our Slots GameObject
        card.transform.SetParent(slots.transform);

        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    // remove a card GameObject from hand
    public void RemoveCard(GameObject card)
    {
        // remove a card from the list
        CardsInHand.Remove(card);

        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    // remove card with a given index from hand
    public void RemoveCardAtIndex(int index)
    {
        CardsInHand.RemoveAt(index);
        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    // get a card GameObject with a given index in hand
    public GameObject GetCardAtIndex(int index)
    {
        return CardsInHand[index];
    }
        
    // MANAGING CARDS AND SLOTS

    // move Slots GameObject according to the number of cards in hand
    void UpdatePlacementOfSlots()
    {
        float posX;
        if (CardsInHand.Count > 0)
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[CardsInHand.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        // tween Slots GameObject to new position in 0.3 seconds
        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);  
    }

    // shift all cards to their new slots
    void PlaceCardsOnNewSlots()
    {
        foreach (GameObject g in CardsInHand)
        {
            // tween this card to a new Slot
            g.transform.DOLocalMoveX(slots.Children[CardsInHand.IndexOf(g)].transform.localPosition.x, 0.3f);

            // apply correct sorting order and HandSlot value for later 
            WhereIsTheCardOrCreature w = g.GetComponent<WhereIsTheCardOrCreature>();
            w.Slot = CardsInHand.IndexOf(g);
            w.SetHandSortingOrder();
        }
    }

    // CARD DRAW METHODS

    // creates a card and returns a new card as a GameObject
    GameObject CreateACardAtPosition(CardAsset c, Vector3 position, Vector3 eulerAngles)
    {
        // Instantiate a card depending on its type
        GameObject card;
        if (c.MaxHealth > 0)
        {
            // this card is a creature card
            card = GameObject.Instantiate(GlobalSettings.Instance.CreatureCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
        }
        else
        {
            // this is a spell: checking for targeted or non-targeted spell
            if (c.Targets == TargetingOptions.NoTarget)
                card = GameObject.Instantiate(GlobalSettings.Instance.NoTargetSpellCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
            else
            {
                card = GameObject.Instantiate(GlobalSettings.Instance.TargetedSpellCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
                // pass targeting options to DraggingActions
                DragSpellOnTarget dragSpell = card.GetComponentInChildren<DragSpellOnTarget>();
                dragSpell.Targets = c.Targets;
            }

        }

        // apply the look of the card based on the info from CardAsset
        OneCardManager manager = card.GetComponent<OneCardManager>();
        manager.cardAsset = c;
        manager.ReadCardFromAsset();

        return card;
    }

    // gives player a new card from a given position
    public void GivePlayerACard(CardAsset c, int UniqueID, bool fast = false, bool fromDeck = true)
    {
        GameObject card;
        if (fromDeck)
            card = CreateACardAtPosition(c, DeckTransform.position, new Vector3(0f, -179f, 0f));
        else
            card = CreateACardAtPosition(c, OtherCardDrawSourceTransform.position, new Vector3(0f, -179f, 0f));

        // Set a tag to reflect where this card is
        foreach (Transform t in card.GetComponentsInChildren<Transform>())
            t.tag = owner.ToString()+"Card";
        // pass this card to HandVisual class
        AddCard(card);

        // Bring card to front while it travels from draw spot to hand
        WhereIsTheCardOrCreature w = card.GetComponent<WhereIsTheCardOrCreature>();
        w.BringToFront();
        w.Slot = 0; 
        w.VisualState = VisualStates.Transition;

        // pass a unique ID to this card.
        IDHolder id = card.AddComponent<IDHolder>();
        id.UniqueID = UniqueID;

        // move card to the hand;
        Sequence s = DOTween.Sequence();
        if (!fast)
        {
            // Debug.Log ("Not fast!!!");
            s.Append(card.transform.DOMove(DrawPreviewSpot.position, GlobalSettings.Instance.CardTransitionTime));
            if (TakeCardsOpenly)
                s.Insert(0f, card.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTime)); 
            //else 
                //s.Insert(0f, card.transform.DORotate(new Vector3(0f, -179f, 0f), GlobalSettings.Instance.CardTransitionTime)); 
            s.AppendInterval(GlobalSettings.Instance.CardPreviewTime);
            // displace the card so that we can select it in the scene easier.
            s.Append(card.transform.DOLocalMove(slots.Children[0].transform.localPosition, GlobalSettings.Instance.CardTransitionTime));
        }
        else
        {
            // displace the card so that we can select it in the scene easier.
            s.Append(card.transform.DOLocalMove(slots.Children[0].transform.localPosition, GlobalSettings.Instance.CardTransitionTimeFast));
            if (TakeCardsOpenly)    
                s.Insert(0f,card.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTimeFast)); 
        }

        s.OnComplete(()=>ChangeLastCardStatusToInHand(card, w));
    }

    // this method will be called when the card arrived to hand 
    void ChangeLastCardStatusToInHand(GameObject card, WhereIsTheCardOrCreature w)
    {
        //Debug.Log("Changing state to Hand for card: " + card.gameObject.name);
        if (owner == AreaPosition.Low)
            w.VisualState = VisualStates.LowHand;
        else
            w.VisualState = VisualStates.TopHand;

        // set correct sorting order
        w.SetHandSortingOrder();
        // end command execution for DrawACArdCommand
        Command.CommandExecutionComplete();
    }

   
    // PLAYING SPELLS

    // 2 Overloaded method to show a spell played from hand
    public void PlayASpellFromHand(int CardID)
    {
        GameObject card = IDHolder.GetGameObjectWithID(CardID);
        PlayASpellFromHand(card);
    }

    public void PlayASpellFromHand(GameObject CardVisual)
    {
        Command.CommandExecutionComplete();
        CardVisual.GetComponent<WhereIsTheCardOrCreature>().VisualState = VisualStates.Transition;
        RemoveCard(CardVisual);

        CardVisual.transform.SetParent(null);

        Sequence s = DOTween.Sequence();
        s.Append(CardVisual.transform.DOMove(PlayPreviewSpot.position, 1f));
        s.Insert(0f, CardVisual.transform.DORotate(Vector3.zero, 1f));
        s.AppendInterval(2f);
        s.OnComplete(()=>
            {
                //Command.CommandExecutionComplete();
                Destroy(CardVisual);
            });
    }


}
