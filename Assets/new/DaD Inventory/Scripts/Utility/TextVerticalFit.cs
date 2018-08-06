using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Fit text by paren't vertical dimension.
/// </summary>
[ExecuteInEditMode]
public class TextVerticalFit : MonoBehaviour
{
	[Tooltip("Empty space between 0 and 1")]
	public float indent = 0.2f;												// Empty space between 0 and 1

	/// <summary>
	/// Fit the text.
	/// </summary>
	private void Fit()
	{
		Text myText = GetComponent<Text>();
		RectTransform parentRectTransform = transform.parent.GetComponent<RectTransform>();
		if (parentRectTransform != null)
		{
			myText.fontSize = (int)(parentRectTransform.rect.height * (1f - indent));
		}
	}

	/// <summary>
	/// Raises the rect transform dimensions change event.
	/// </summary>
	void OnRectTransformDimensionsChange()
	{
		Fit();
	}
}
