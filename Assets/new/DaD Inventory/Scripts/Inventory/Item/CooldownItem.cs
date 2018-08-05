using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Cooldown item.
/// </summary>
public class CooldownItem : MonoBehaviour
{
	[Tooltip("Cooldown delay")]
	public float cooldown = 0f;
	[Tooltip("Icon color on cooldown")]
	public Color cooldownColor = Color.gray;
	[Tooltip("Cooldown icon mask")]
	public Image cooldownMask;
	[Tooltip("Cooldown countdown text")]
	public Text cooldownText;
	[HideInInspector]
	public float timeLeft = 0f;

	private Coroutine cooldownCoroutine;
	private static CoroutineContainer coroutineContainer;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		// Create contaner for all cooldon coroutines
		if (coroutineContainer == null)
		{
			coroutineContainer = FindObjectOfType<CoroutineContainer>();
			if (coroutineContainer == null)
			{
				coroutineContainer = new GameObject().AddComponent<CoroutineContainer>();
				coroutineContainer.name = "CoroutineContainer";
			}
		}
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		if (timeLeft > 0f)
		{
			cooldownCoroutine = coroutineContainer.StartCoroutine(CooldownCoroutine());
		}
	}

	/// <summary>
	/// Starts the cooldown.
	/// </summary>
	public void StartCooldown()
	{
		timeLeft = cooldown;
		if (timeLeft > 0f)
		{
			cooldownCoroutine = coroutineContainer.StartCoroutine(CooldownCoroutine());
		}
	}

	/// <summary>
	/// Stops the cooldown.
	/// </summary>
	public void StopCooldown()
	{
		timeLeft = 0f;
		if (cooldownMask != null)
		{
			// Hide mask
			cooldownMask.gameObject.SetActive(false);
		}
		if (cooldownText != null)
		{
			// Hide countdown
			cooldownText.gameObject.SetActive(false);
		}
		if (cooldownCoroutine != null)
		{
			coroutineContainer.StopCoroutine(cooldownCoroutine);
			cooldownCoroutine = null;
		}
	}

	/// <summary>
	/// Cooldowns the coroutine.
	/// </summary>
	/// <returns>The coroutine.</returns>
	private IEnumerator CooldownCoroutine()
	{
		if (cooldownMask != null)
		{
			// Show mask
			cooldownMask.gameObject.SetActive(true);
		}
		if (cooldownText != null)
		{
			// Show countdown
			cooldownText.gameObject.SetActive(true);
		}

		while (timeLeft > 0f)
		{
			if (cooldownMask != null)
			{
				// Fill mask
				cooldownMask.fillAmount = 1f - (cooldown - timeLeft) / cooldown;
			}
			if (cooldownText != null)
			{
				// Aply countdown
				cooldownText.text = ((int)Mathf.Ceil(timeLeft)).ToString();
			}
			yield return new WaitForEndOfFrame();
			timeLeft -= Time.deltaTime;
		}
		StopCooldown();
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy()
	{
		if (coroutineContainer != null && cooldownCoroutine != null)
		{
			coroutineContainer.StopCoroutine(cooldownCoroutine);
		}
	}
}
