using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	public void LoadByName(string sceneName)
	{
		print("LoadScene");
		SceneManager.LoadScene(sceneName);
	}
}
