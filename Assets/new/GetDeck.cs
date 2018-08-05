using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDeck : MonoBehaviour {

	public Deck Deck;

	// Use this for initialization
	void Start () {
		Deck = GameObject.Find("DeckLogic").GetComponent<Deck>();
		Deck.cards.Shuffle();
		
	}
	
}
