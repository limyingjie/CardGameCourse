using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dummy quick slots group.
/// </summary>
public class DummyQuickSlotsGroup : MonoBehaviour
{
	/// <summary>
	/// Raises the DaD group event.
	/// </summary>
	/// <param name="desc">Desc.</param>
	private void OnDadGroupEvent(DadCell.DadEventDescriptor desc)
	{
		switch (desc.triggerType)
		{
		case DadCell.TriggerType.DropEnd:
			RemoveUnresolvedItems();
			break;
		}
	}

	/// <summary>
	/// Raises the stack group event.
	/// </summary>
	private void OnStackGroupEvent()
	{
		// Remove unresolved items after any DaD event
		RemoveUnresolvedItems();
	}

	/// <summary>
	/// Removes the unresolved items
	/// </summary>
	private void RemoveUnresolvedItems()
	{
		foreach (QuickItem quickItem in GetComponentsInChildren<QuickItem>(true))
		{
			bool hit = false;
			foreach (Transform parentTransform in quickItem.itemSource.GetComponentsInParent<Transform>())
			{
				if (parentTransform == transform)
				{
					hit = true;
					break;
				}
			}
			if (hit == false)
			{
				quickItem.Remove();
			}
		}
	}
}
