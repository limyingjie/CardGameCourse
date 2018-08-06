using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CardUI : MonoBehaviour
{
    public Image icon;
    public CardAsset current;
    public Deck Deck;
    public PlayerCollection collection;

    void Start(){
   		Deck = GameObject.Find("DeckLogic").GetComponent<Deck>();
   		collection = GameObject.Find("CollectionLogic").GetComponent<PlayerCollection>();
   		// Button button = gameObject.GetComponent<Button>();
   		// if(!collection.cards.Contains(current)){
   		// 	button.interactable = false;
   		// }
   		// if (current!=null){
   		// 	icon.sprite = current.CardImage;
   		// }
	}

	public void Remove(){
		print("Remove");
		DeckUI cells = GameObject.Find ("CellsGroup").GetComponent<DeckUI>();
		Deck.cards.Remove(current);
		cells.display();
	}

	public void Add(){
		DeckUI cells = GameObject.Find ("CellsGroup").GetComponent<DeckUI>();
		List<CardAsset> results = Deck.cards.FindAll(s => s.Equals(current));
		if (results.Count<=1){
			Deck.cards.Add(current);
		}
		if(Deck.cards.Count>Deck.cardsNo){
			Deck.cards.RemoveAt(0);
		}
		cells.display();
	}

	public void showDetail(){
		print("showDetail");
		GameObject canvas = GameObject.Find ("DeckCanvas");
		GameObject CardDetail = canvas.transform.Find("CardDetail").gameObject;
		CardDetail.SetActive(true);
		OneCardManager ocm = CardDetail.transform.Find ("Card").GetComponent<OneCardManager>();
		print(current.Description);
		ocm.cardAsset = current;
		ocm.ReadCardFromAsset();
	}


}
