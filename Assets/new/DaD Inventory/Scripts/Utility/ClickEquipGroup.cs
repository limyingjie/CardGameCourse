using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Place item into specified stack group on double click.
/// </summary>
public class ClickEquipGroup : MonoBehaviour
{
	[Tooltip("The first stack group for items transactions on click")]
	public StackGroup firstGroup;
	[Tooltip("The second stack group for items transactions on click")]
	public StackGroup secondGroup;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		Debug.Assert(firstGroup && secondGroup, "Wrong settings");
	}

	/// <summary>
	/// Raises the item click event.
	/// </summary>
	/// <param name="item">Item.</param>
	public void OnItemUse(GameObject item)
	{
		if (item != null)
		{
			StackItem stackItem = item.GetComponent<StackItem>();
			if (stackItem != null)
			{
				StackGroup sourceGroup = Gets.GetComponentInParent<StackGroup>(item.transform);
				if (sourceGroup != null && (sourceGroup == firstGroup || sourceGroup == secondGroup))
				{
					StackGroup destinationGroup = sourceGroup == firstGroup ? secondGroup : firstGroup;
					// Try to place item into free space of specified stack group
					if (destinationGroup.AddItem(stackItem, stackItem.GetStack()) <= 0)
					{
						// If group have no free space for item
						// Get similar items in that group
						List<StackItem> similarItems = destinationGroup.GetSimilarStackItems(stackItem);
						if (similarItems.Count > 0)
						{
							// Try to replace with first similar item
							destinationGroup.ReplaceItems(similarItems[0], stackItem, sourceGroup);
						}
					}
				}
			}
		}
	}
}
