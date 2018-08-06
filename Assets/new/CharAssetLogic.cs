using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAssetLogic : MonoBehaviour {

	public CharacterAsset playerChar;
	private static bool created = false;

	// Use this for initialization
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
