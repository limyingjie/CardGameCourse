using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dummy example of item pick up on click.
/// </summary>
public class DummyItemPickUp : MonoBehaviour
{
	[Tooltip("Stack item prefab")]
	public StackItem itemPrefab;										// Stack item prefab
	[Tooltip("Stack amount")]
	public int stack = 1;												// Stack amount

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		Debug.Assert(itemPrefab, "Wrong settings");
		Highlight highlight = GetComponent<Highlight>();
		if (highlight != null)
		{
			highlight.StartHighlight();
		}
	}

	/// <summary>
	/// Picks up.
	/// </summary>
	/// <returns>The up.</returns>
	/// <param name="amount">Amount.</param>
	public StackItem PickUp(int amount)
	{
		// Create stack item
		StackItem stackItem = Instantiate(itemPrefab);
		stackItem.name = itemPrefab.name;
		stackItem.SetStack(Mathf.Min(stack, amount));
		stack -= amount;
		if (stack <= 0)
		{
			Destroy(gameObject);
		}
		return stackItem;
	}
}
