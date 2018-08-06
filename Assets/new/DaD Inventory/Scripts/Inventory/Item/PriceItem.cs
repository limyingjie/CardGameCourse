using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Item will have price in cash.
/// </summary>
public class PriceItem : MonoBehaviour
{
	[Tooltip("Default price of this item (without modifier)")]
	public int defaultPrice;														// Default price of this item (without modifier)
	[Tooltip("Game object that contains text field with price amount")]
	public GameObject priceObject;													// GO that contains text field with price amount
	[Tooltip("Text field with price amount")]
	public Text priceText;															// Text field with price amount

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		Debug.Assert(priceObject & priceText, "Wrong settings");
	}

	/// <summary>
	/// Gets the price of this item.
	/// </summary>
	/// <returns>The price.</returns>
	public int GetPrice()
	{
		int price;
		int.TryParse(priceText.text, out price);
		return price;
	}

	/// <summary>
	/// Sets the price of this item.
	/// </summary>
	/// <param name="price">Price.</param>
	public void SetPrice(int price)
	{
		priceText.text = price.ToString();
	}

	/// <summary>
	/// Show/Hide the price.
	/// </summary>
	/// <param name="condition">If set to <c>true</c> condition.</param>
	public void ShowPrice(bool condition)
	{
		priceObject.SetActive(condition);
	}
}
