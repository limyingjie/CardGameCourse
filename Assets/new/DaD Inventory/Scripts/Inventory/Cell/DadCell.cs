using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Drag and Drop cell.
/// </summary>
public class DadCell : MonoBehaviour, IDropHandler
{
    public enum TriggerType                                                 // Types of drag and drop events
    {
		EmptyDrop,															// Item dropped anywere outside cells
		DragGroupRequest,													// Request for group with source cell
		DropGroupRequest,                                                   // Request for group with destination cell
		DragCellRequest,													// Source cell request
		DropCellRequest,													// Destination cell request
		DragEnd,															// Drag event completed
        DropEnd,                                                       		// Drop event completed
        ItemAdded,                                                          // Item manualy added into cell
        ItemWillBeDestroyed                                               	// Called just before item will be destroyed
    }

    public class DadEventDescriptor                                        // Info about item's drag and drop event
    {
        public TriggerType triggerType;                                     // Type of drag and drop trigger
		public DadCell sourceCell;                                  		// From this cell item was dragged
		public DadCell destinationCell;                             		// Into this cell item was dropped
		public bool cellPermission;                                 		// Cell's decision need to be made on request
		public bool groupPermission;                               			// Group's decision need to be made on request
    }

	[Tooltip("Sprite color for empty cell")]
	public Color empty = Color.white;                                       // Sprite color for empty cell
	[Tooltip("Sprite color for filled cell")]
	public Color full = Color.white;                                       	// Sprite color for filled cell

	private DadItem myDadItem;												// Item of this DaD cell
	private bool awaked = false;											// Gameobject was awaked and initialized

    void OnEnable()
    {
        DadItem.OnItemDragStartEvent += OnAnyItemDragStart;         		// Handle any item drag start
        DadItem.OnItemDragEndEvent += OnAnyItemDragEnd;             		// Handle any item drag end

		myDadItem = GetDadItem();
		UpdateBackgroundState();
    }

    void OnDisable()
    {
        DadItem.OnItemDragStartEvent -= OnAnyItemDragStart;
        DadItem.OnItemDragEndEvent -= OnAnyItemDragEnd;

        StopAllCoroutines();                                                // Stop all coroutines if there is any
    }

    /// <summary>
    /// On any item drag start need to disable all items raycast for correct drop operation.
    /// </summary>
    /// <param name="item"> dragged item </param>
	private void OnAnyItemDragStart(GameObject item)
    {
		if (myDadItem != null)
        {
			myDadItem.MakeRaycast(false);                                 	// Disable item's raycast for correct drop handling
        }
    }

    /// <summary>
    /// On any item drag end enable all items raycast.
    /// </summary>
    /// <param name="item"> dragged item </param>
	private void OnAnyItemDragEnd(GameObject item)
    {
		if (myDadItem != null)
        {
			myDadItem.MakeRaycast(true);                        			// Enable item's raycast
        }
        UpdateBackgroundState();
    }

    /// <summary>
    /// Item is dropped into this cell.
    /// </summary>
    /// <param name="data"></param>
    public void OnDrop(PointerEventData data)
    {
        if (DadItem.icon != null)
        {
			DadCell sourceCell = DadItem.sourceCell;
			if (sourceCell != this)
            {
                DadEventDescriptor desc = new DadEventDescriptor();
                desc.sourceCell = sourceCell;
				desc.destinationCell = this;
				SendGroupRequest(desc);										// Send group request
				SendCellRequest(desc);										// Send cell request
                StartCoroutine(NotifyOnDragEnd(desc));  					// Send notification after drop will be finished
				if (desc.cellPermission == true && desc.groupPermission == true) // If drop permitted
                {
					SwapItems(sourceCell, this);                			// Swap items between cells
                }
            }
			UpdateMyItem();
			UpdateBackgroundState();
			sourceCell.UpdateMyItem();
			sourceCell.UpdateBackgroundState();
        }
    }

    /// <summary>
    /// Change cell's sprite color on item put/remove.
    /// </summary>
    /// <param name="condition"> true - filled, false - empty </param>
	public void UpdateBackgroundState()
    {
		Image bg = GetComponent<Image>();
		if (bg != null)
		{
			bg.color = myDadItem != null ? full : empty;
		}
    }

	/// <summary>
	/// Updates my item
	/// </summary>
	public void UpdateMyItem()
	{
		myDadItem = GetComponentInChildren<DadItem>(true);
	}

	/// <summary>
	/// Gets DaD item from this cell.
	/// </summary>
	/// <returns>The dad item.</returns>
	public DadItem GetDadItem()
	{
		if (awaked == false)
		{
			UpdateMyItem();
			awaked = true;
		}
		return myDadItem;
	}

	/// <summary>
	/// Get item from this cell.
	/// </summary>
	/// <returns> Item </returns>
	public GameObject GetItem()
	{
		GameObject res = null;
		DadItem dadItem = GetDadItem();
		if (dadItem != null)
		{
			res = dadItem.gameObject;
		}
		return res;
	}

	/// <summary>
	/// Put item into this cell.
	/// </summary>
	/// <param name="item">Item.</param>
	/// <param name="destroyOldItem">If set to <c>true</c> destroy old item.</param>
	private void PlaceItem(GameObject item, bool destroyOldItem)
	{
		if (item != null)
		{
			if (destroyOldItem == true)
			{
				RemoveItem();                                            	// Remove current item from this cell
				myDadItem = null;
			}
			DadItem dadItem = item.GetComponent<DadItem>();
			if (dadItem != null)
			{
				// Put new item into this cell
				item.transform.SetParent(transform, false);
				item.transform.SetAsFirstSibling();
				item.transform.localPosition = Vector3.zero;
				dadItem.MakeRaycast(true);
				myDadItem = dadItem;
			}
		}
		UpdateBackgroundState();
	}

	/// <summary>
	/// Swaps DaD items between cells.
	/// </summary>
	/// <param name="firstDadCell">First DaD cell.</param>
	/// <param name="secondDadCell">Second DaD cell.</param>
	public static void SwapItems(DadCell firstDadCell, DadCell secondDadCell)
	{
		if ((firstDadCell != null) && (secondDadCell != null))
		{
			GameObject firstItem = firstDadCell.GetItem();                // Get item from first cell
			GameObject secondItem = secondDadCell.GetItem();              // Get item from second cell
			// Swap items
			firstDadCell.PlaceItem(secondItem, false);
			secondDadCell.PlaceItem(firstItem, false);
			// Update states
			firstDadCell.UpdateMyItem();
			secondDadCell.UpdateMyItem();
			firstDadCell.UpdateBackgroundState();
			secondDadCell.UpdateBackgroundState();
		}
	}

	/// <summary>
	/// Swap items between two cells.
	/// </summary>
	/// <param name="firstCell"> Cell </param>
	/// <param name="secondCell"> Cell </param>
	public static void SwapItems(GameObject firstCell, GameObject secondCell)
	{
		if ((firstCell != null) && (secondCell != null))
		{
			DadCell firstDadCell = firstCell.GetComponent<DadCell>();
			DadCell secondDadCell = secondCell.GetComponent<DadCell>();
			SwapItems(firstDadCell, secondDadCell);
		}
	}

	/// <summary>
	/// Manualy add item into this cell.
	/// </summary>
	/// <param name="item"> New item </param>
	public void AddItem(GameObject item)
	{
		if (item != null)
		{
			PlaceItem(item, true);
			DadEventDescriptor desc = new DadEventDescriptor();
			// Fill event descriptor
			desc.triggerType = TriggerType.ItemAdded;
			desc.sourceCell = this;
			desc.destinationCell = this;
			// Send notification about item adding
			SendGroupNotification(desc.destinationCell.gameObject, desc);
		}
	}

	/// <summary>
	/// Manualy add item into cell.
	/// </summary>
	/// <param name="cell">Cell.</param>
	/// <param name="item">Item.</param>
	public static void AddItem(GameObject cell, GameObject item)
	{
		DadCell dadCell = cell.GetComponent<DadCell>();
		if (dadCell != null)
		{
			dadCell.AddItem(item);
		}
	}

	/// <summary>
	/// Manualy delete item from this cell
	/// </summary>
	public void RemoveItem()
	{
		UpdateMyItem();
		GameObject item = GetItem();
		if (item != null)
		{
			DadEventDescriptor desc = new DadEventDescriptor();
			// Fill event descriptor
			desc.triggerType = TriggerType.ItemWillBeDestroyed;
			desc.sourceCell = this;
			desc.destinationCell = this;
			// Send notification about item destruction
			SendGroupNotification(desc.destinationCell.gameObject, desc);
			if (item != null)
			{
				Destroy(item);
			}
		}
		myDadItem = null;
		UpdateBackgroundState();
	}

	/// <summary>
	/// Send drag and drop group request to application
	/// </summary>
	/// <param name="desc"> drag and drop event descriptor </param>
	/// <returns> result from desc.permission </returns>
	private bool SendGroupRequest(DadEventDescriptor desc)
	{
		bool result = false;
		if (desc != null)
		{
			desc.cellPermission = true;
			desc.groupPermission = true;
			// To source cell
			desc.triggerType = TriggerType.DragGroupRequest;
			SendGroupNotification(desc.sourceCell.gameObject, desc);
			// To destination cell
			desc.triggerType = TriggerType.DropGroupRequest;
			SendGroupNotification(desc.destinationCell.gameObject, desc);

			result = desc.groupPermission;
		}
		return result;
	}

	/// <summary>
	/// Send drag and drop cell request to application
	/// </summary>
	/// <returns><c>true</c>, if confirm is successfull, <c>false</c> otherwise.</returns>
	/// <param name="desc">Desc.</param>
	private bool SendCellRequest(DadEventDescriptor desc)
	{
		bool result = false;
		if (desc != null)
		{
			// To source cell
			desc.triggerType = TriggerType.DragCellRequest;
			SendCellNotification(desc.sourceCell.gameObject, desc);
			// To destination cell
			desc.triggerType = TriggerType.DropCellRequest;
			SendCellNotification(desc.destinationCell.gameObject, desc);

			result = desc.cellPermission;
		}
		return result;
	}

    /// <summary>
    /// Send drag and drop information to group
    /// </summary>
    /// <param name="desc"> drag and drop event descriptor </param>
	private void SendGroupNotification(GameObject source, DadEventDescriptor desc)
    {
		if (source != null && desc != null)
        {
            // Send message with DragAndDrop info to parents GameObjects
			source.SendMessageUpwards("OnDadGroupEvent", desc, SendMessageOptions.DontRequireReceiver);
        }
    }

	/// <summary>
	/// Send drag and drop information to cell
	/// </summary>
	/// <param name="desc"> drag and drop event descriptor </param>
	private void SendCellNotification(GameObject source, DadEventDescriptor desc)
	{
		if (source != null && desc != null)
		{
			// Send message with DragAndDrop info to GameObject
			source.SendMessage("OnDadCellEvent", desc, SendMessageOptions.DontRequireReceiver);
		}
	}

    /// <summary>
    /// Wait for event end and send notification to application
    /// </summary>
    /// <param name="desc"> drag and drop event descriptor </param>
    /// <returns></returns>
    private IEnumerator NotifyOnDragEnd(DadEventDescriptor desc)
    {
        // Wait end of drag operation
        while (DadItem.draggedItem != null)
        {
            yield return new WaitForEndOfFrame();
        }
		desc.triggerType = TriggerType.DragEnd;
		SendGroupNotification(desc.sourceCell.gameObject, desc);
		desc.triggerType = TriggerType.DropEnd;
		SendGroupNotification(desc.destinationCell.gameObject, desc);
    }
}
