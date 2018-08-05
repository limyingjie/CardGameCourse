using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This items may unite into stack.
/// </summary>
public class StackItem : MonoBehaviour
{
	[Tooltip("Stack of this item on scene start")]
	public int defaultStack = 1;											// Stack of this item on scene start
	[Tooltip("Max stack amount for this item")]
	public int maxStack = 10;												// Max stack amount for this item
	[Tooltip("Game object with stack text field")]
	public GameObject stackObject;											// GO with stack text field
	[Tooltip("Stack text field")]
	public Text stackText;													// Stack text field
	[Tooltip("SFX when stack operations")]
	public AudioClip sound;													// SFX when stack operations

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		Debug.Assert(stackObject && stackText, "Wrong settings");
		SetStack(defaultStack);
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		UpdateCondition();
	}

	/// <summary>
	/// Updates stack's condition.
	/// </summary>
	private void UpdateCondition()
	{
		int stack = GetStack();
		if (stack > 1)
		{
			ShowStack();
		}
		else if (stack == 1)
		{
			// Hide stack text if stack == 0
			HideStack();
		}
		else
		{
			// Stack <= 0
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
	}

	/// <summary>
	/// Gets the stack cell of this item.
	/// </summary>
	/// <returns>The stack cell.</returns>
	public StackCell GetStackCell()
	{
		return Gets.GetComponentInParent<StackCell>(transform);
	}

	/// <summary>
	/// Gets the stack of this item.
	/// </summary>
	/// <returns>The stack.</returns>
	public int GetStack()
	{
		return defaultStack;
	}

	/// <summary>
	/// Sets the item's stack.
	/// </summary>
	/// <param name="stack">Stack.</param>
	public void SetStack(int stack)
	{
		defaultStack = stack;
		stackText.text = defaultStack.ToString();
		UpdateCondition();
	}

	/// <summary>
	/// Adds the stack.
	/// </summary>
	/// <param name="stack">Stack.</param>
	public void AddStack(int stack)
	{
		SetStack(GetStack() + stack);
	}

	/// <summary>
	/// Reduces the stack.
	/// </summary>
	/// <param name="stack">Stack.</param>
	public void ReduceStack(int stack)
	{
		SetStack(GetStack() - stack);
	}

	/// <summary>
	/// Shows the stack.
	/// </summary>
	public void ShowStack()
	{
		stackObject.SetActive(true);
	}

	/// <summary>
	/// Hides the stack.
	/// </summary>
	public void HideStack()
	{
		stackObject.SetActive(false);
	}
}
