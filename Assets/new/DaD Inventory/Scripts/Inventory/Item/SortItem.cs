using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Item has specified type (sort).
/// </summary>
public class SortItem : MonoBehaviour
{
	[Tooltip("Item's type")]
	public string itemType = "";											// Item's type

	/// <summary>
	/// Gets the sort cell of tjis item.
	/// </summary>
	/// <returns>The sort cell.</returns>
	public SortCell GetSortCell()
	{
		return Gets.GetComponentInParent<SortCell>(transform);
	}
}
