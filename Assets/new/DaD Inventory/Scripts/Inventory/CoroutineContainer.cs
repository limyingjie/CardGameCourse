using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coroutine container. Allows to use coroutines on other inactive gameobjects.
/// </summary>
public class CoroutineContainer : MonoBehaviour
{
	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		StopAllCoroutines();
	}
}
