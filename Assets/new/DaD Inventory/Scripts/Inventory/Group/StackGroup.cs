using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unite stack cells in group and change it's stack logic.
/// </summary>
public class StackGroup : MonoBehaviour
{
	[Tooltip("Split items when drop between two different stack groups")]
	public bool splitOuter;														// Split items when drop between two different stack groups
	[Tooltip("Arrange item placement in group's cells on drop")]
	public bool arrangeMode;													// Arrange item placement in group's cells on drop
	[Tooltip("This stack group will destroy any dropped item")]
	public bool trashBinMode;													// This stack group will destroy any dropped item
	[Tooltip("SFX for item destroying")]
	public AudioClip trashBinSound;												// SFX for item destroying
	[Tooltip("Audio source for SFX")]
	public AudioSource audioSource;												// Audio source for SFX
	[Tooltip("This game objects will be notified on stack events")]
	public List<GameObject> eventAdditionalReceivers = new List<GameObject>();	// This GOs will be notified on stack events

	public static bool globalSplit;												// Split interface will be used for all item's drag till this value == true

	private enum MyState
	{
		WaitForRequest,
		WaitForEvent,
		Busy
	}

	private MyState myState = MyState.WaitForRequest;							// State machine
	private static SplitInterface splitInterface;								// Interface for items split operations

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		if (splitInterface == null)
		{
			splitInterface = FindObjectOfType<SplitInterface>();
		}
		if (splitInterface == null)
		{
			splitInterface = Instantiate(Resources.Load<SplitInterface>("SplitInterface"));
			splitInterface.gameObject.name = "SplitInterface";
			splitInterface.transform.SetAsLastSibling();
		}
		Debug.Assert(splitInterface, "Wrong initial settings");
	}

	/// <summary>
	/// Toggles the global split enabling.
	/// </summary>
	public void ToggleGlobalSplit()
	{
		globalSplit = !globalSplit;
	}

	/// <summary>
	/// Adds the item.
	/// </summary>
	/// <returns>The item.</returns>
	/// <param name="stackItem">Stack item.</param>
	/// <param name="limit">Limit.</param>
	public int AddItem(StackItem stackItem, int limit)
	{
		int res = 0;
		if (stackItem != null)
		{
			StackGroup sourceStackGroup = Gets.GetComponentInParent<StackGroup>(stackItem.transform);
			// Try to distribute item inside group's items and cells
			res += DistributeAnywhere(stackItem, limit, null);
			// Send stack event notification
			SendNotification(sourceStackGroup != null ? sourceStackGroup.gameObject : null, gameObject);
			if (res > 0)
			{
				PlaySound(stackItem.sound);
			}
		}
		return res;
	}

	/// <summary>
	/// Removes the item.
	/// </summary>
	/// <param name="stackCell">Stack cell.</param>
	/// <param name="limit">Limit.</param>
	public void RemoveItem(StackCell stackCell, int limit)
	{
		if (stackCell != null)
		{
			StackItem stackItem = stackCell.GetStackItem();
			if (stackItem != null)
			{
				RemoveItem(stackItem, limit);
			}
		}
	}

	/// <summary>
	/// Removes the item.
	/// </summary>
	/// <param name="stackItem">Stack item.</param>
	/// <param name="limit">Limit.</param>
	public void RemoveItem(StackItem stackItem, int limit)
	{
		if (stackItem != null)
		{
			stackItem.ReduceStack(limit);
			SendNotification(null, gameObject);
			PlaySound(trashBinSound);
		}
	}

	/// <summary>
	/// Swap items between groups.
	/// </summary>
	/// <returns><c>true</c>, if item was replaced, <c>false</c> otherwise.</returns>
	/// <param name="currentStackItem">Current stack item.</param>
	/// <param name="sourceStackItem">Source stack item.</param>
	/// <param name="sourceStackGroup">Source stack group.</param>
	public bool ReplaceItems(StackItem currentStackItem, StackItem sourceStackItem, StackGroup sourceStackGroup)
	{
		bool res = false;
		if (currentStackItem != null && sourceStackItem != null && sourceStackGroup != null)
		{
			StackCell currentStackCell = currentStackItem.GetStackCell();
			sourceStackGroup.DistributeAnywhere(currentStackItem, currentStackItem.GetStack(), null);
			if (currentStackCell.GetStackItem() == null)
			{
				currentStackCell.UniteStack(sourceStackItem, sourceStackItem.GetStack());
				PlaySound(sourceStackItem.sound);
				res = true;
			}
			// Send stack event notification
			SendNotification(sourceStackGroup.gameObject, gameObject);
		}
		return res;
	}

	/// <summary>
	/// Gets the allowed space for specified item.
	/// </summary>
	/// <returns>The allowed space.</returns>
	/// <param name="stackItem">Stack item.</param>
	public int GetAllowedSpace(StackItem stackItem)
	{
		double res = 0;
		if (stackItem != null)
		{
			foreach (StackCell stackCell in GetComponentsInChildren<StackCell>(true))
			{
				StackItem item = stackCell.GetStackItem();
				if (item != null)
				{
					if (stackCell.HasSameItem(stackItem) == true)
					{
						res += stackCell.GetAllowedSpace();
					}
				}
				else
				{
					res += stackCell.GetAllowedSpace();
				}
			}
		}
		if (res > int.MaxValue)
		{
			res = int.MaxValue;
		}
		return (int)res;
	}

	/// <summary>
	/// Gets the free stack cells.
	/// </summary>
	/// <returns>The free stack cells.</returns>
	/// <param name="stackItem">Stack item.</param>
	public List<StackCell> GetFreeStackCells(StackItem stackItem)
	{
		List<StackCell> res = new List<StackCell>();
		if (stackItem != null)
		{
			foreach (StackCell stackCell in GetComponentsInChildren<StackCell>(true))
			{
				if (stackCell.GetStackItem() == null)
				{
					if (SortCell.IsSortAllowed(stackCell.gameObject, stackItem.gameObject) == true)
					{
						res.Add(stackCell);
					}
				}
			}
		}
		return res;
	}

	/// <summary>
	/// Gets the similar stack items (with same sort).
	/// </summary>
	/// <returns>The similar stack items.</returns>
	/// <param name="stackItem">Stack item.</param>
	public List<StackItem> GetSimilarStackItems(StackItem stackItem)
	{
		List<StackItem> res = new List<StackItem>();
		if (stackItem != null)
		{
			foreach (StackCell stackCell in GetComponentsInChildren<StackCell>(true))
			{
				StackItem sameStackItem = stackCell.GetStackItem();
				if (sameStackItem != null)
				{
					if (SortCell.IsSortAllowed(stackCell.gameObject, stackItem.gameObject) == true)
					{
						res.Add(sameStackItem);
					}
				}
			}
		}
		return res;
	}

	/// <summary>
	/// Raises the DaD group event.
	/// </summary>
	/// <param name="desc">Desc.</param>
	private void OnDadGroupEvent(DadCell.DadEventDescriptor desc)
	{
		switch (desc.triggerType)
		{
		case DadCell.TriggerType.DragGroupRequest:
		case DadCell.TriggerType.DropGroupRequest:
			if (myState == MyState.WaitForRequest)
			{
				// Disable standard DaD logic
				desc.groupPermission = false;
				myState = MyState.WaitForEvent;
			}
			break;
		case DadCell.TriggerType.DragEnd:
			if (myState == MyState.WaitForEvent)
			{
				StackGroup sourceStackControl = Gets.GetComponentInParent<StackGroup>(desc.sourceCell.transform);
				StackGroup destStackControl = Gets.GetComponentInParent<StackGroup>(desc.destinationCell.transform);
				if (sourceStackControl != destStackControl)
				{
					// If this group is source group - do nothing
					myState = MyState.WaitForRequest;
				}
			}
			break;
		case DadCell.TriggerType.DropEnd:
			if (myState == MyState.WaitForEvent)
			{
				// If this group is destination group
				myState = MyState.Busy;
				// Operate item's drop
				StartCoroutine(EventHandler(desc));
			}
			break;
		}
	}

	/// <summary>
	/// Try to distribute item in similar items inside group.
	/// </summary>
	/// <returns>The in items.</returns>
	/// <param name="stackItem">Stack item.</param>
	/// <param name="amount">Amount.</param>
	/// <param name="reservedStackCell">Reserved stack cell, excluded from calculation.</param>
	private int DistributeInItems(StackItem stackItem, int amount, StackCell reservedStackCell)
	{
		int res = 0;

		if (stackItem != null)
		{
			if (amount > 0)
			{
				foreach (StackCell stackCell in GetComponentsInChildren<StackCell>(true))
				{
					if (stackCell != reservedStackCell)
					{
						if (amount > 0)
						{
							if (stackCell.HasSameItem(stackItem) == true)
							{
								int unitedPart = stackCell.UniteStack(stackItem, amount);
								res += unitedPart;
								amount -= unitedPart;
							}
						}
						else
						{
							break;
						}
					}
				}
			}
		}
		return res;
	}

	/// <summary>
	/// Try to distribute item in free cells inside group.
	/// </summary>
	/// <returns>The in cells.</returns>
	/// <param name="stackItem">Stack item.</param>
	/// <param name="amount">Amount.</param>
	/// <param name="reservedStackCell">Reserved stack cell, excluded from calculation.</param>
	private int DistributeInCells(StackItem stackItem, int amount, StackCell reservedStackCell)
	{
		int res = 0;

		if (stackItem != null)
		{
			if (amount > 0)
			{
				foreach (StackCell emptyStackCell in GetFreeStackCells(stackItem))
				{
					if (emptyStackCell != reservedStackCell)
					{
						int unitedPart = emptyStackCell.UniteStack(stackItem, amount);
						res += unitedPart;
						amount -= unitedPart;
						if (amount <= 0)
						{
							break;
						}
					}
				}
			}
		}
		return res;
	}

	/// <summary>
	/// Try to distribute between items than between free cells.
	/// </summary>
	/// <returns>The anywhere.</returns>
	/// <param name="stackItem">Stack item.</param>
	/// <param name="amount">Amount.</param>
	/// <param name="reservedStackCell">Reserved stack cell, excluded from calculation.</param>
	private int DistributeAnywhere(StackItem stackItem, int amount, StackCell reservedStackCell)
	{
		int res = 0;
		res += DistributeInItems(stackItem, amount, reservedStackCell);
		amount -= res;
		if (amount > 0)
		{
			res += DistributeInCells(stackItem, amount, reservedStackCell);
		}
		return res;
	}

	/// <summary>
	/// Stack event handler.
	/// </summary>
	/// <returns>The handler.</returns>
	/// <param name="desc">Desc.</param>
	private IEnumerator EventHandler(DadCell.DadEventDescriptor desc)
	{
		StackGroup sourceStackGroup = Gets.GetComponentInParent<StackGroup>(desc.sourceCell.transform);
		StackGroup destStackGroup = Gets.GetComponentInParent<StackGroup>(desc.destinationCell.transform);

		if (sourceStackGroup == null || destStackGroup == null)
		{
			desc.groupPermission = false;
			// Send stack event notification
			SendNotification(sourceStackGroup != null ? sourceStackGroup.gameObject : null, destStackGroup != null ? destStackGroup.gameObject : null);
			myState = MyState.WaitForRequest;
			yield break;
		}

		StackCell myStackCell = desc.destinationCell.GetComponent<StackCell>();
		StackCell theirStackCell = desc.sourceCell.GetComponent<StackCell>();

		if (myStackCell == null || theirStackCell == null)
		{
			desc.groupPermission = false;
			// Send stack event notification
			SendNotification(sourceStackGroup.gameObject, destStackGroup.gameObject);
			myState = MyState.WaitForRequest;
			yield break;
		}

		StackItem myStackItem = myStackCell.GetStackItem();
		StackItem theirStackItem = theirStackCell.GetStackItem();

		PriceItem priceItem = theirStackItem.GetComponent<PriceItem>();
		PriceGroup buyer = Gets.GetComponentInParent<PriceGroup>(desc.destinationCell.transform);
		PriceGroup seller = Gets.GetComponentInParent<PriceGroup>(desc.sourceCell.transform);

		AudioClip itemSound = theirStackItem.sound;									// Item's SFX

		int amount = theirStackItem.GetStack();										// Item's stack amount

		if ((globalSplit == true)
			|| (sourceStackGroup != destStackGroup && (sourceStackGroup.splitOuter == true || destStackGroup.splitOuter == true)))
		{
			// Need to use split interface
			if (splitInterface != null)
			{
				if (priceItem != null && buyer != null && seller != null && buyer != seller)
				{
					// Split with prices
					splitInterface.ShowSplitter(theirStackItem, priceItem);
				}
				else
				{
					// Split without prices
					splitInterface.ShowSplitter(theirStackItem, null);
				}
				// Show split interface and wait while it is active
				while (splitInterface.gameObject.activeSelf == true)
				{
					yield return new WaitForEndOfFrame();
				}
				// Get splitted stack amount
				amount = splitInterface.GetRightAmount();
			}
		}

		if (amount > 0)
		{
			if (sourceStackGroup != destStackGroup
				&& (destStackGroup.arrangeMode == true || sourceStackGroup.arrangeMode == true))
			{
				// Split in arrange mode between different stack groups
				if (priceItem != null && buyer != null && seller != null && buyer != seller)
				{
					// Different price groups
					if (buyer.GetCash() > priceItem.GetPrice() * amount)
					{
						// Has anough cash
						int distributed = DistributeAnywhere(theirStackItem, amount, null);
						if (distributed > 0)
						{
							int totalPrice = priceItem.GetPrice() * distributed;
							seller.AddCash(totalPrice);
							buyer.SpendCash(totalPrice);

							buyer.UpdatePrices();
						}
					}
				}
				else
				{
					// Same price group
					DistributeAnywhere(theirStackItem, amount, null);
				}
			}
			else
			{
				// Inside same stack group transactions disabled in arrange mode
				if (arrangeMode == false)
				{
					if (myStackItem != null)
					{
						// Check if items allowed for cells
						if (	SortCell.IsSortAllowed(myStackCell.gameObject, theirStackItem.gameObject) == true
							&& 	SortCell.IsSortAllowed(theirStackCell.gameObject, myStackItem.gameObject) == true)
						{
							// Destination cell already has item
							if (myStackCell.HasSameItem(theirStackItem) == true)
							{
								// Same item
								myStackCell.UniteStack(theirStackItem, amount);
							}
							else
							{
								// Different items. Try to swap items between cells
								if (myStackCell.SwapStacks(theirStackCell) == true)
								{
									// Swap successful
									theirStackItem = theirStackCell.GetStackItem();
									if (theirStackItem != null)
									{
										// Distribute item after swap
										DistributeInItems(theirStackItem, theirStackItem.GetStack(), theirStackCell);
									}
								}
								else
								{
									// Swap unsuccessful.
									// Try to distribute item between other cells to make cell free
									DistributeAnywhere(myStackItem, myStackItem.GetStack(), myStackCell);
									myStackItem = myStackCell.GetStackItem();
									if (myStackItem != null)
									{
										// Item still in cell. Try to place item in other group's cells
										sourceStackGroup.DistributeAnywhere(myStackItem, myStackItem.GetStack(), null);
										myStackItem = myStackCell.GetStackItem();
										if (myStackItem == null)
										{
											// Item was placed into other cell and now this cell is free
											// Place item into destination cell
											myStackCell.UniteStack(theirStackItem, amount);
										}
									}
									else
									{
										// Item was placed into other cell and now this cell is free
										// Place item into destination cell
										myStackCell.UniteStack(theirStackItem, amount);
									}
								}
							}
						}
					}
					else
					{
						// Destination cell has no item
						// Place item into destination cell
						myStackCell.UniteStack(theirStackItem, amount);
					}
				}
			}
		}
		// Send stack event notification
		SendNotification(sourceStackGroup.gameObject, destStackGroup.gameObject);
		if (trashBinMode == true)
		{
			// In trash bin mode just destroy item
			desc.destinationCell.RemoveItem();
			PlaySound(trashBinSound);
		}
		else
		{
			PlaySound(itemSound);
		}
		myState = MyState.WaitForRequest;
	}

	/// <summary>
	/// Sends the stack event notification.
	/// </summary>
	/// <param name="sourceGroup">Source group.</param>
	/// <param name="destinationGroup">Destination group.</param>
	private void SendNotification(GameObject sourceGroup, GameObject destinationGroup)
	{
		if (sourceGroup != null)
		{
			// Send notification to source GO
			sourceGroup.SendMessageUpwards("OnStackGroupEvent", SendMessageOptions.DontRequireReceiver);
		}
		if (destinationGroup != null)
		{
			// Send notification to destination GO
			destinationGroup.SendMessageUpwards("OnStackGroupEvent", SendMessageOptions.DontRequireReceiver);
		}
		foreach (GameObject receiver in eventAdditionalReceivers)
		{
			// Send notification to additionaly specified GOs
			receiver.SendMessage("OnStackGroupEvent", SendMessageOptions.DontRequireReceiver);
		}
	}

	/// <summary>
	/// Plaies the sound.
	/// </summary>
	/// <param name="sound">Sound.</param>
	private void PlaySound(AudioClip sound)
	{
		if (audioSource != null && sound != null)
		{
			audioSource.PlayOneShot(sound);
		}
	}
}
