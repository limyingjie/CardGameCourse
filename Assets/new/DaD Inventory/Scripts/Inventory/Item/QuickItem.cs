using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Quick item.
/// </summary>
public class QuickItem : MonoBehaviour, IPointerClickHandler
{
	[Tooltip("Top image for cooldown state")]
	public Image cooldownMask;
	[Tooltip("Cooldown counter (UI Text)")]
	public Text cooldownText;
	[Tooltip("GameObject with stack amount text")]
	public GameObject stackObject;
	[Tooltip("UI Text with stack amount")]
	public Text stackText;
	[HideInInspector]
	public ClickItem itemSource; 						// Source of this quick item

	private Image myImage;								// My icon image
	private Image sourceImage;							// Source icon image
	private CooldownItem cooldownSource;				// Cooldown item source
	private StackItem stackSource;						// Stack item source
	private int myStack = 0;							// Stack amount

	private enum MyState								// Machine state
	{
		Init,
		Active,
		Cooldown
	}

	private MyState myState = MyState.Init;				// Current state for this instance

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		Debug.Assert(itemSource && cooldownMask && cooldownText && stackObject && stackText, "Wrong initial settings");
		myImage = GetComponent<Image>();
		sourceImage = itemSource.GetComponent<Image>();
		Debug.Assert(myImage && sourceImage, "Wrong initial settings");
		cooldownSource = itemSource.GetComponent<CooldownItem>();
		stackSource = itemSource.GetComponent<StackItem>();
		Update();
	}

	/// <summary>
	/// Remove quick item.
	/// </summary>
	public void Remove()
	{
		DadCell dadCell = Gets.GetComponentInParent<DadCell>(transform);
		if (dadCell != null)
		{
			dadCell.RemoveItem();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Raises the pointer click event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerClick(PointerEventData eventData)
	{
		if (itemSource != null)
		{
			bool res = true;
			if (cooldownSource != null && cooldownSource.timeLeft > 0f)
			{
				res = false;
			}
			if (res == true)
			{
				// Call original item use
				itemSource.UseItem();
			}
		}
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		if (itemSource == null)
		{
			// Remove quick item if original item destroyed
			Remove();
		}
		else
		{
			if (myImage.color != sourceImage.color)
			{
				// Update color
				myImage.color = sourceImage.color;
			}

			if (stackSource != null)
			{
				// Update stack amount
				int stack = stackSource.GetStack();
				if (myStack != stack)
				{
					myStack = stack;
					stackText.text = myStack.ToString();
				}
			}
			if (stackSource != null && stackSource.GetStack() > 1)
			{
				stackObject.SetActive(true);
			}
			else
			{
				stackObject.SetActive(false);
			}

			if (cooldownSource != null)
			{
				// Update cooldown state
				if (cooldownSource.timeLeft > 0f)
				{
					if (myState != MyState.Cooldown)
					{
						myState = MyState.Cooldown;
						cooldownText.gameObject.SetActive(true);
						cooldownMask.gameObject.SetActive(true);
					}
					cooldownText.text = ((int)Mathf.Ceil(cooldownSource.timeLeft)).ToString();
					cooldownMask.fillAmount = 1f - (cooldownSource.cooldown - cooldownSource.timeLeft) / cooldownSource.cooldown;
				}
				else
				{
					if (myState != MyState.Active)
					{
						myState = MyState.Active;
						cooldownText.gameObject.SetActive(false);
						cooldownMask.gameObject.SetActive(false);
					}
				}
			}
			else
			{
				if (myState != MyState.Active)
				{
					myState = MyState.Active;
					cooldownText.gameObject.SetActive(false);
					cooldownMask.gameObject.SetActive(false);
				}
			}
		}
	}
}
