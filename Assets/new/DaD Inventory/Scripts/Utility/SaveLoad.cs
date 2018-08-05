using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Save and load inventory state.
/// </summary>
public class SaveLoad : MonoBehaviour
{
	[Tooltip("The inventory state will save on game close and will load on game start")]
	public bool autoSave = false;
	[Tooltip("Delete data with saved inventory state (on game start)")]
	public bool deleteSavedData = false;

	private PriceGroup priceGroup;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		priceGroup = GetComponentInChildren<PriceGroup>(true);
		if (deleteSavedData == true)
		{
			DeleteSavedData();
		}
		if (autoSave == true)
		{
			// Load data on game start
			Load();
		}
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		if (autoSave == true)
		{
			// Save data on game stop
			Save();
		}
	}

	/// <summary>
	/// Save inventory data.
	/// </summary>
	public void Save()
	{
		DadCell[] dadCells = GetComponentsInChildren<DadCell>(true);
		QuickCell[] quickCells = GetComponentsInChildren<QuickCell>(true);

		PlayerPrefs.SetString("DaD Inventory", "0.2.0"); // Stored data format version

		PlayerPrefs.SetInt("Cells count", dadCells.Length); // Stored count of dad cells

		PlayerPrefs.SetInt("Quick count", quickCells.Length); // Stored count of quick cells

		if (priceGroup != null)
		{
			PlayerPrefs.SetInt("Cash amount", priceGroup.GetCash()); // Stored player's cash
		}

		for (int i = 0; i < dadCells.Length; ++i)
		{
			GameObject item = dadCells[i].GetItem();
			if (item != null)
			{
				PlayerPrefs.SetString("Cell_" + i + " item", item.name); // Stored item's name
				StackItem stackItem = item.GetComponent<StackItem>();
				if (stackItem != null)
				{
					PlayerPrefs.SetInt("Cell_" + i + " stack", stackItem.GetStack()); // Stored item's stack
				}
				else
				{
					PlayerPrefs.SetInt("Cell_" + i + " stack", 0); // Stored item's stack
				}
			}
			else
			{
				PlayerPrefs.SetString("Cell_" + i + " item", "null"); // Stored item's name
			}
		}

		for (int i = 0; i < quickCells.Length; ++i)
		{
			QuickItem quickitem = quickCells[i].GetQuickItem();
			if (quickitem != null && quickitem.itemSource != null)
			{
				for (int k = 0; k < dadCells.Length; ++k)
				{
					if (dadCells[k].GetItem() == quickitem.itemSource.gameObject)
					{
						PlayerPrefs.SetInt("Quick_" + i + " link", k); // Stored item's link
						break;
					}
				}
			}
			else
			{
				PlayerPrefs.SetInt("Quick_" + i + " link", -1); // Stored item's link
			}
		}

		Debug.Log("Inventory saved");
	}

	/// <summary>
	/// Load inventory data.
	/// </summary>
	public void Load()
	{
		if (PlayerPrefs.HasKey("DaD Inventory") == true) // If any stored data exist
		{
			DadCell[] dadCells = GetComponentsInChildren<DadCell>(true);
			QuickCell[] quickCells = GetComponentsInChildren<QuickCell>(true);

			if (PlayerPrefs.GetInt("Cells count") != dadCells.Length && PlayerPrefs.GetInt("Quick count") != quickCells.Length) // If cells amount were changed
			{
				DeleteSavedData();
			}
			else // If cells amount the same as in stored data
			{
				if (priceGroup != null)
				{
					int cash = PlayerPrefs.GetInt("Cash amount");
					if (cash < 0)
					{
						cash = 0;
					}
					priceGroup.SetCash(cash); // Set player's cash from stored data
				}

				ClearInventory(); // Clear all current items from inventory

				// Add items from prefabs by it's stored names
				for (int i = 0; i < dadCells.Length; ++i)
				{
					string itemName = PlayerPrefs.GetString("Cell_" + i + " item");
					if (itemName != null && itemName != "" && itemName != "null")
					{
						int stack = PlayerPrefs.GetInt("Cell_" + i + " stack");
						if (stack > 0)
						{
							GameObject itemPrefab = Resources.Load<GameObject>("Items/" + itemName);
							if (itemPrefab != null)
							{
								GameObject item = Instantiate(itemPrefab);
								if (item != null)
								{
									item.name = itemName;
									dadCells[i].AddItem(item);
									StackItem stackItem = item.GetComponent<StackItem>();
									if (stackItem != null)
									{
										stackItem.SetStack(stack); // Set item's stack
									}
								}
							}
						}
						else
						{
							GameObject skillPrefab = Resources.Load<GameObject>("Skills/" + itemName);
							if (skillPrefab != null)
							{
								GameObject skill = Instantiate(skillPrefab);
								if (skill != null)
								{
									skill.name = itemName;
									dadCells[i].AddItem(skill);
								}
							}
						}
					}
				}

				for (int i = 0; i < quickCells.Length; ++i)
				{
					int link = PlayerPrefs.GetInt("Quick_" + i + " link");
					if (link >= 0)
					{
						quickCells[i].CreateQuickItem(dadCells[link].GetItem());
					}
				}

				Debug.Log("Inventory loaded");
			}
		}
	}

	/// <summary>
	/// Clears all inventory items.
	/// </summary>
	public void ClearInventory()
	{
		DadCell[] dadCells = GetComponentsInChildren<DadCell>(true);

		foreach (DadCell dadCell in dadCells)
		{
			dadCell.RemoveItem();
		}
	}

	/// <summary>
	/// Deletes the saved data.
	/// </summary>
	public void DeleteSavedData()
	{
		PlayerPrefs.DeleteAll();

		Debug.Log("Inventory saved data deleted");
	}
}
