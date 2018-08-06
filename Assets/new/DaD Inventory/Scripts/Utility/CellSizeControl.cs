using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Automatic cell size control when use GrigLayautGroup in fixed rows/lines count mode.
/// </summary>
[ExecuteInEditMode]
public class CellSizeControl : MonoBehaviour
{
	private RectTransform rectTransform;											// My rect transform
	private GridLayoutGroup gridLayoutGroup;										// My grid layout group

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		rectTransform = GetComponent<RectTransform>();
		gridLayoutGroup = GetComponent<GridLayoutGroup>();
		Debug.Assert(rectTransform && gridLayoutGroup, "Wrong settings");
		UpdateCellsSize();
	}

	/// <summary>
	/// Updates the size of the cells depending of parent's RectTransform size.
	/// </summary>
	private void UpdateCellsSize()
	{
		if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
		{
			int cellSize = ((int)rectTransform.rect.width - gridLayoutGroup.padding.horizontal - (int)gridLayoutGroup.spacing.x * (gridLayoutGroup.constraintCount - 1)) / gridLayoutGroup.constraintCount;
			gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
		}
		else if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedRowCount)
		{
			int cellSize = ((int)rectTransform.rect.height - gridLayoutGroup.padding.vertical * 2 - (int)gridLayoutGroup.spacing.y * (gridLayoutGroup.constraintCount - 1)) / gridLayoutGroup.constraintCount;
			gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
		}
	}

#if UNITY_EDITOR
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		UpdateCellsSize();
	}
#endif
}
