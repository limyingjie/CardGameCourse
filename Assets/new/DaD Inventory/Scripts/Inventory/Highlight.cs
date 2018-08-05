using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Highlight UI or render.
/// </summary>
public class Highlight : MonoBehaviour
{
	[Tooltip("Blink period on highlighting")]
	public float blinkPeriod = 1f;
	[Tooltip("Blink color")]
	public Color highlightColor = Color.black;
	[Tooltip("Target graphic when use UI component")]
	public MaskableGraphic highlightTargetUI;
	[Tooltip("Target graphic when use render component")]
	public Renderer highlightTargetRenderer;

	private Color originColor;									// Original target color
	private Coroutine highlightCoroutine;
	private static CoroutineContainer coroutineContainer;
	private bool inited = false;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		if (inited == false)
		{
			Init();
		}
	}

	/// <summary>
	/// Init this instance.
	/// </summary>
	void Init()
	{
		Debug.Assert(highlightTargetUI || highlightTargetRenderer, "Wrong initial settings");
		if (coroutineContainer == null)
		{
			// Create container for all coroutines
			coroutineContainer = FindObjectOfType<CoroutineContainer>();
			if (coroutineContainer == null)
			{
				coroutineContainer = new GameObject().AddComponent<CoroutineContainer>();
				coroutineContainer.name = "CoroutineContainer";
			}
		}
		if (highlightTargetUI != null)
		{
			originColor = highlightTargetUI.color;
		}
		else
		{
			originColor = highlightTargetRenderer.material.color;
		}
		inited = true;
	}

	/// <summary>
	/// Starts the highlight.
	/// </summary>
	public void StartHighlight()
	{
		if (inited == false)
		{
			Init();
		}
		highlightCoroutine = coroutineContainer.StartCoroutine(HighlightCoroutine());
	}

	/// <summary>
	/// Stops the highlight.
	/// </summary>
	public void StopHighlight()
	{
		if (inited == false)
		{
			Init();
		}
		if (highlightCoroutine != null)
		{
			coroutineContainer.StopCoroutine(highlightCoroutine);
			highlightCoroutine = null;
		}
		if (highlightTargetUI != null)
		{
			highlightTargetUI.color = originColor;
		}
		else
		{
			highlightTargetRenderer.material.color = originColor;
		}
	}

	/// <summary>
	/// Highlights coroutine.
	/// </summary>
	/// <returns>The coroutine.</returns>
	private IEnumerator HighlightCoroutine()
	{
		while (true)
		{
			// Set color to highlightColor an back to originColor with blinkPeriod
			if (highlightTargetUI != null)
			{
				highlightTargetUI.color = Color.Lerp(originColor, highlightColor, Mathf.PingPong(Time.time, blinkPeriod / 2f));
			}
			if (highlightTargetRenderer != null)
			{
				highlightTargetRenderer.material.color = Color.Lerp(originColor, highlightColor, Mathf.PingPong(Time.time, blinkPeriod / 2f));
			}
			yield return new WaitForEndOfFrame();
		}
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy()
	{
		if (inited == true && coroutineContainer != null && highlightCoroutine != null)
		{
			coroutineContainer.StopCoroutine(highlightCoroutine);
		}
	}
}
