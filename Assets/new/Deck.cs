using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour {

    private static bool created = false;
    public List<CardAsset> cards = new List<CardAsset>();
    public int cardsNo = 25; 

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            Debug.Log("Awake: " + this.gameObject);
        }
    }

    public void shuffle(){
        cards.Shuffle();
    }
}
