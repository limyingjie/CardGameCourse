using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Price group for automatical cash calculations on item drop.
/// </summary>
public class PriceGroup : MonoBehaviour
{
	[Tooltip("Text field contains cash amount")]
	public Text cashText;
	[Tooltip("Prices of this group will be multiplied with this value")]
	public float sellModifier = 1f;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		UpdatePrices();
	}

	/// <summary>
	/// Gets the current cash amount.
	/// </summary>
	/// <returns>The cash.</returns>
	public int GetCash()
	{
		int cash;
		if (cashText != null)
		{
			int.TryParse(cashText.text, out cash);
		}
		else
		{
			cash = int.MaxValue;
		}
		return cash;
	}

	/// <summary>
	/// Sets the current cash amount
	/// </summary>
	/// <param name="cash">Cash.</param>
	public void SetCash(int cash)
	{
		if (cashText != null)
		{
			cashText.text = cash.ToString();
		}
	}

	/// <summary>
	/// Adds the cash.
	/// </summary>
	/// <param name="cash">Cash.</param>
	public void AddCash(int cash)
	{
		if (cashText != null)
		{
			SetCash(GetCash() + cash);
		}
	}

	/// <summary>
	/// Spends the cash.
	/// </summary>
	/// <returns><c>true</c>, if cash was spent, <c>false</c> otherwise.</returns>
	/// <param name="amount">Amount.</param>
	public bool SpendCash(int amount)
	{
		bool res = false;
		int cash = GetCash();
		if (cash >= amount)														// If cash anough
		{
			SetCash(cash - amount);
			res = true;
		}
		return res;
	}

	/// <summary>
	/// Updates the prices using sell modifier.
	/// </summary>
	public void UpdatePrices()
	{
		foreach (PriceItem item in GetComponentsInChildren<PriceItem>(true))
		{
			item.SetPrice((int)Mathf.Ceil(item.defaultPrice * sellModifier));
		}
	}

	/// <summary>
	/// Show/Hide prices.
	/// </summary>
	/// <param name="condition">If set to <c>true</c> condition.</param>
	public void ShowPrices(bool condition)
	{
		foreach (PriceItem item in GetComponentsInChildren<PriceItem>(true))
		{
			item.ShowPrice(condition);
		}
	}
}
