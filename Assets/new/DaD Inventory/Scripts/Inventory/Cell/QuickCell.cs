using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Cell for fake quick item.
/// </summary>
public class QuickCell : MonoBehaviour
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
			GameObject item = desc.sourceCell.GetItem();
			if (item != null)
			{
				// Create quick item from original item
				CreateQuickItem(item);
			}
			break;
		case DadCell.TriggerType.EmptyDrop:
			QuickItem myQuickItem = GetComponentInChildren<QuickItem>();
			if (myQuickItem != null)
			{
				// Clear cell on drop quick item out of all cells
				myQuickItem.Remove();
			}
			break;
		}
	}

	/// <summary>
	/// Creates the quick item.
	/// </summary>
	/// <returns>The quick item.</returns>
	/// <param name="item">Item.</param>
	public QuickItem CreateQuickItem(GameObject item)
	{
		QuickItem res = null;
		DadCell dadCell = GetComponent<DadCell>();
		if (item != null && dadCell != null)
		{
			ClickItem clickItem = item.GetComponent<ClickItem>();
			if (clickItem != null)
			{
				// Remove old quick item
				RemoveQuickItem();
				// Load quick item template
				GameObject newQuickItem = Instantiate(Resources.Load<GameObject>("QuickItemPrefab"));
				newQuickItem.name = item.name;
				res = newQuickItem.GetComponent<QuickItem>();
				res.itemSource = clickItem;
				Image myImage = newQuickItem.GetComponent<Image>();
				Image sourceImage = item.GetComponent<Image>();
				if (myImage != null && sourceImage != null)
				{
					myImage.sprite = sourceImage.sprite;
					myImage.color = sourceImage.color;
				}
				// Place quick item to quick cell
				dadCell.AddItem(newQuickItem.gameObject);
			}
		}
		return res;
	}

	/// <summary>
	/// Gets the quick item.
	/// </summary>
	/// <returns>The quick item.</returns>
	public QuickItem GetQuickItem()
	{
		return GetComponentInChildren<QuickItem>(true);
	}

	/// <summary>
	/// Removes the quick item.
	/// </summary>
	public void RemoveQuickItem()
	{
		QuickItem quickItem = GetComponentInChildren<QuickItem>();
		if (quickItem != null)
		{
			quickItem.Remove();
		}
	}
}
