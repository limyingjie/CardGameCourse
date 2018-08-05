using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCollection : MonoBehaviour {

    private static bool created = false;
    public List<CardAsset> cards = new List<CardAsset>();

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            Debug.Log("Awake: " + this.gameObject);
        }
    }

}
