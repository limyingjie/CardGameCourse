using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows to GetComponent on inactive gameobjects.
/// </summary>
public static class Gets
{
	/// <summary>
	/// Gets component in parent.
	/// </summary>
	/// <returns>The component in parent.</returns>
	/// <param name="child">Child.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T GetComponentInParent<T>(Transform child) where T : Component
	{
		T res = null;
		Transform transform = child;
		while (true)
		{
			transform = transform.parent;
			if (transform == null)
			{
				break;
			}
			res = transform.GetComponent<T>();
			if (res != null)
			{
				break;
			}
		}
		return res;
	}
}
