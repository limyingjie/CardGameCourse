using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Dummy example of health bar realization.
/// </summary>
public class DummyHealthBar : MonoBehaviour
{
	[Tooltip("UI image of health bar")]
	public Image healthBarImage;										// UI image of health bar
	[Tooltip("Health bar's text amount")]
	public Text healthBarText;											// Health bar's text amount
	[Tooltip("Health bar's text max amount")]
	public Text maxHealthBarText;										// Health bar's text max amount
	[Tooltip("Audio source for SFX")]
	public AudioSource audioSource;										// Audio source for SFX

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		Debug.Assert(healthBarImage && healthBarText && maxHealthBarText, "Wrong settings");
		SetHealth(GetHealth());
	}

	/// <summary>
	/// Updates the health bar image.
	/// </summary>
	/// <param name="fillAmount">Fill amount.</param>
	private void UpdateHealthBarImage(float fillAmount)
	{
		healthBarImage.fillAmount = fillAmount;
	}

	/// <summary>
	/// Gets the health amount.
	/// </summary>
	/// <returns>The health.</returns>
	public int GetHealth()
	{
		int health;
		int.TryParse(healthBarText.text, out health);
		return health;
	}

	/// <summary>
	/// Gets the max health amount.
	/// </summary>
	/// <returns>The max health.</returns>
	public int GetMaxHealth()
	{
		int maxHealth;
		int.TryParse(maxHealthBarText.text, out maxHealth);
		return maxHealth;
	}

	/// <summary>
	/// Sets the health amounth.
	/// </summary>
	/// <param name="health">Health.</param>
	public void SetHealth(int health)
	{
		int maxHealth = GetMaxHealth();
		int res = Mathf.Min(health, maxHealth);
		res = Mathf.Max(res, 0);
		healthBarText.text = res.ToString();
		UpdateHealthBarImage((float)res / maxHealth);
	}

	/// <summary>
	/// Sets the max health amounth.
	/// </summary>
	/// <param name="maxHealth">Max health.</param>
	public void SetMaxHealth(int maxHealth)
	{
		int health = Mathf.Min(GetHealth(), maxHealth);
		maxHealthBarText.text = maxHealth.ToString();
		SetHealth(health);
	}

	/// <summary>
	/// Adds health.
	/// </summary>
	/// <param name="health">Health.</param>
	public void AddHealth(int health)
	{
		SetHealth(GetHealth() + health);
	}

	/// <summary>
	/// Reduces health.
	/// </summary>
	/// <param name="health">Health.</param>
	public void ReduceHealth(int health)
	{
		SetHealth(GetHealth() - health);
	}

	/// <summary>
	/// Raises the item click event.
	/// </summary>
	/// <param name="item">Item.</param>
	public void OnItemUse(GameObject item)
	{
		if (item != null)
		{
			// Heal on potion use
			DummyHealthPotion healthPotion = item.GetComponent<DummyHealthPotion>();
			ClickItem clickItem = item.GetComponent<ClickItem>();
			if (healthPotion != null)
			{
				if (GetHealth() < GetMaxHealth())
				{
					// Add health if it is not full
					AddHealth(healthPotion.healAmount);
					if (audioSource != null && clickItem != null && clickItem.audioClip != null)
					{
						audioSource.PlayOneShot(clickItem.audioClip);
					}
					StackItem stackItem = item.GetComponent<StackItem>();
					if (stackItem != null)
					{
						// Reduce potion's stack
						stackItem.ReduceStack(1);
					}
				}
			}
		}
	}
}
