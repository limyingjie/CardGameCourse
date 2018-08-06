using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Dummy inventory control for demo scene.
/// </summary>
public class DummyInventoryControl : MonoBehaviour
{
	[Tooltip("Equipments cells sheet")]
	public GameObject equipment;											// Equipments cells sheet
	[Tooltip("Inventory cells sheet")]
	public GameObject inventory;											// Inventory cells sheet
	[Tooltip("Skills cells sheet")]
	public GameObject skills;
	[Tooltip("Vendor cells sheet")]
	public GameObject vendor;												// Vendor cells sheet
	[Tooltip("Inventory stack group")]
	public StackGroup inventoryStackGroup;									// Inventory stack group

	private PriceGroup priceGroup;											// Player's price group

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		priceGroup = GetComponent<PriceGroup>();
		Debug.Assert(equipment && inventory && vendor && skills && inventoryStackGroup && priceGroup, "Wrong settings");
		priceGroup.ShowPrices(vendor.activeSelf);
	}

	/// <summary>
	/// Show/Hide the equipments.
	/// </summary>
	public void ToggleEquipment(Toggle toggle)
	{
		CloseAllSheets();
		if (toggle.isOn == true)
		{
			equipment.SetActive(true);
			inventory.SetActive(true);
		}
	}

	/// <summary>
	/// Show/Hide the skills.
	/// </summary>
	public void ToggleSkills(Toggle toggle)
	{
		CloseAllSheets();
		if (toggle.isOn == true)
		{
			skills.SetActive(true);
		}
	}

	/// <summary>
	/// Show/Hide the vendor.
	/// </summary>
	public void ToggleVendor(Toggle toggle)
	{
		CloseAllSheets();
		if (toggle.isOn == true)
		{
			inventory.SetActive(true);
			vendor.SetActive(true);
			priceGroup.ShowPrices(true);
		}
	}

	private void CloseAllSheets()
	{
		equipment.SetActive(false);
		inventory.SetActive(false);
		skills.SetActive(false);
		vendor.SetActive(false);
		priceGroup.ShowPrices(false);
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		// On click
		if (Input.GetMouseButtonDown(0) == true)
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = Input.mousePosition;
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerEventData, results);
			// If clicked not on UI
			if (results.Count <= 0)
			{
				DummyItemPickUp dummyItemPickUp = null;
				// Raycast to colliders2d
				RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward);
				if (hit2D.collider != null)
				{
					dummyItemPickUp = hit2D.collider.gameObject.GetComponent<DummyItemPickUp>();
				}
				if (dummyItemPickUp == null)
				{
					// Raycast to colliders3d
					RaycastHit[] hit3D = Physics.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward);
					if (hit3D.Length > 0)
					{
						dummyItemPickUp = hit3D[0].collider.gameObject.GetComponent<DummyItemPickUp>();
					}
				}
				if (dummyItemPickUp != null)
				{
					// Hitted on DummyItemPickUp item
					// Get stack item from DummyItemPickUp item
					StackItem stackItem = dummyItemPickUp.PickUp(inventoryStackGroup.GetAllowedSpace(dummyItemPickUp.itemPrefab));
					if (stackItem != null)
					{
						// Try to place item into inventory
						dummyItemPickUp.stack -= inventoryStackGroup.AddItem(stackItem, stackItem.GetStack());
						// Show item price if vendor is active
						priceGroup.ShowPrices(vendor.activeSelf);
					}
				}
			}
		}
	}
}
