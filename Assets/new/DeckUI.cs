using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DeckUI : MonoBehaviour
{

    [SerializeField]
    private GameObject cardTemplate;
    [SerializeField]
    private Deck Deck;




    void Start(){
    	if (Deck == null){
    		Deck = GameObject.Find("DeckLogic").GetComponent<Deck>();
    	}

        foreach (CardUI item in this.GetComponentsInChildren<CardUI>())
        {
            Destroy(item.gameObject);
        }

        foreach (CardAsset card in Deck.cards)
        {
            GameObject obj = Instantiate(this.cardTemplate);
            obj.transform.SetParent(this.transform);
            CardUI sample = obj.transform.gameObject.GetComponent<CardUI>();
            sample.transform.localScale = new Vector3(1,1,1);
            sample.current = card;
            sample.icon.sprite = card.CardImage;
        }

    }

    
    public void display()
    {	
    	Deck = GameObject.Find("DeckLogic").GetComponent<Deck>();

        foreach (CardUI item in this.GetComponentsInChildren<CardUI>())
        {	
            Destroy(item.gameObject);
        }


        foreach (CardAsset card in Deck.cards)
        {
            GameObject obj = Instantiate(this.cardTemplate);
            obj.transform.SetParent(this.transform);
            CardUI sample = obj.transform.gameObject.GetComponent<CardUI>();
            sample.transform.localScale = new Vector3(1,1,1);
            sample.current = card;
            sample.icon.sprite = card.CardImage;
        }
    }
}