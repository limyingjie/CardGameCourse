using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This cell will drop only allowed items.
/// </summary>
public class SortCell : MonoBehaviour
{
	[Tooltip("List with allowed items sorts")]
	public List<string> allowedItemTypes = new List<string>();							// List with allowed items sorts

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		DadItem.OnItemDragStartEvent += OnAnyItemDragStart;         					// Handle any item drag start
		DadItem.OnItemDragEndEvent += OnAnyItemDragEnd;             					// Handle any item drag end
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		DadItem.OnItemDragStartEvent -= OnAnyItemDragStart;
		DadItem.OnItemDragEndEvent -= OnAnyItemDragEnd;
	}

	/// <summary>
	/// Raises the any item drag start event.
	/// </summary>
	/// <param name="item">Item.</param>
	private void OnAnyItemDragStart(GameObject item)
	{
		if (item != null && IsSortAllowed(item) == true)
		{
			Highlight highlight = GetComponent<Highlight>();
			if (highlight != null)
			{
				// Highlight cell if dragged item allowed for this cell
				highlight.StartHighlight();
			}
		}
	}

	/// <summary>
	/// Raises the any item drag end event.
	/// </summary>
	/// <param name="item">Item.</param>
	private void OnAnyItemDragEnd(GameObject item)
	{
		Highlight highlight = GetComponent<Highlight>();
		if (highlight != null)
		{
			highlight.StopHighlight();
		}
	}

	/// <summary>
	/// Gets the sort item from this cell.
	/// </summary>
	/// <returns>The sort item.</returns>
	public SortItem GetSortItem()
	{
		SortItem res = null;
		GameObject item = GetComponent<DadCell>().GetItem();
		if (item != null)
		{
			res = item.GetComponent<SortItem>();
		}
		return res;
	}

	/// <summary>
	/// Check if item may be placed into this cell.
	/// </summary>
	/// <returns><c>true</c> if this instance is sort allowed the specified item; otherwise, <c>false</c>.</returns>
	/// <param name="item">Item.</param>
	public bool IsSortAllowed(GameObject item)
	{
		bool res = false;
		if (item != null)
		{
			SortItem sortItem = item.GetComponent<SortItem>();
			if (allowedItemTypes.Count <= 0)
			{
				res = true;
			}
			else
			{
				if (sortItem != null)
				{
					foreach (string itemType in allowedItemTypes)
					{
						// If item has allowed sort
						if (itemType == sortItem.itemType)
						{
							res = true;
							break;
						}
					}
				}
			}
		}
		return res;
	}

	/// <summary>
	/// Determines if is item allowed the specified sort.
	/// </summary>
	/// <returns><c>true</c> if is sort allowed the specified cell item; otherwise, <c>false</c>.</returns>
	/// <param name="cell">Cell.</param>
	/// <param name="item">Item.</param>
	public static bool IsSortAllowed(GameObject cell, GameObject item)
	{
		bool res = false;
		if (cell != null && item != null)
		{
			res = true;
			SortCell sortCell = cell.GetComponent<SortCell>();
			if (sortCell != null)
			{
				res = sortCell.IsSortAllowed(item);
			}
		}
		return res;
	}

	/// <summary>
	/// Raises the DaD cell event.
	/// </summary>
	/// <param name="desc">Desc.</param>
	public void OnDadCellEvent(DadCell.DadEventDescriptor desc)
	{
		switch (desc.triggerType)
		{
		case DadCell.TriggerType.DragCellRequest:
			GameObject swapItem = desc.destinationCell.GetItem();
			if (swapItem != null)
			{
				// Check for sort in destination cell
				if (IsSortAllowed(swapItem) == false)
				{
					desc.cellPermission = false;
				}
			}
			break;
		case DadCell.TriggerType.DropCellRequest:
			// Check for sort in source cell
			if (IsSortAllowed(desc.sourceCell.GetItem()) == false)
			{
				desc.cellPermission = false;
			}
			break;
		}
	}
}
