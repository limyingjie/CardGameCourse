using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Dummy skills control.
/// </summary>
public class DummySkillsControl : MonoBehaviour
{
	[Tooltip("Audio source for SFX")]
	public AudioSource audioSource;
	[Tooltip("Cursor for repair skill")]
	public Texture2D repairCursor;
	[Tooltip("DummyParameters gameobject")]
	public DummyParameters dummyParameters;

	private Coroutine repairCoroutine = null;

	/// <summary>
	/// Raises the item click event.
	/// </summary>
	/// <param name="item">Item.</param>
	public void OnItemUse(GameObject item)
	{
		switch (item.name)
		{
		case "Repair":
			if (repairCoroutine == null)
			{
				repairCoroutine = StartCoroutine(RepairCoroutine(item.GetComponent<Image>()));
			}
			break;
		case "Rage":
			if (dummyParameters != null)
			{
				StartCoroutine(RageCoroutine(item.GetComponent<CooldownItem>()));
			}
			break;
		}
	}

	/// <summary>
	/// Repair coroutine.
	/// </summary>
	/// <returns>The coroutine.</returns>
	/// <param name="skillIcon">Skill icon.</param>
	private IEnumerator RepairCoroutine(Image skillIcon)
	{
		// Start highlight skill
		skillIcon.GetComponent<Highlight>().StartHighlight();
		// Change cursor
		Cursor.SetCursor(repairCursor, Vector2.zero, CursorMode.Auto);
		// Wait for mouse pressing
		while (Input.GetMouseButton(0) == true)
		{
			yield return new WaitForEndOfFrame();
		}
		while (Input.GetMouseButton(0) == false)
		{
			yield return new WaitForEndOfFrame();
		}
		// Check if any item under cursor
		PointerEventData pointerData = new PointerEventData(EventSystem.current);
		pointerData.position = Input.mousePosition;
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerData, results);
		if (results.Count > 0)
		{
			foreach (RaycastResult res in results)
			{
				StackItem stackItem = res.gameObject.GetComponent<StackItem>();
				if (stackItem != null)
				{
					// Start skill cooldown
					skillIcon.GetComponent<CooldownItem>().StartCooldown();
					ClickItem clickItem = skillIcon.GetComponent<ClickItem>();
					if (audioSource != null && clickItem.audioClip != null)
					{
						// Play SFX
						audioSource.PlayOneShot(clickItem.audioClip);
					}
					break;
				}
			}
		}
		// Return default cursor
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		// Stop highlight skill
		skillIcon.GetComponent<Highlight>().StopHighlight();
		repairCoroutine = null;
	}

	/// <summary>
	/// Rage coroutine.
	/// </summary>
	/// <returns>The coroutine.</returns>
	/// <param name="skillCooldown">Skill cooldown.</param>
	private IEnumerator RageCoroutine(CooldownItem skillCooldown)
	{
		// Add temporary attack bonus
		dummyParameters.attackBonus += 10;
		dummyParameters.UpdateParameters();
		// Start skill cooldown
		skillCooldown.StartCooldown();
		ClickItem clickItem = skillCooldown.GetComponent<ClickItem>();
		if (audioSource != null && clickItem.audioClip != null)
		{
			// Play SFX
			audioSource.PlayOneShot(clickItem.audioClip);
		}
		// Wait for cooldown end
		while (skillCooldown.timeLeft > 0f)
		{
			yield return new WaitForEndOfFrame();
		}
		// Remove attack bonus
		dummyParameters.attackBonus -= 10;
		dummyParameters.UpdateParameters();
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		StopAllCoroutines();
	}
}
