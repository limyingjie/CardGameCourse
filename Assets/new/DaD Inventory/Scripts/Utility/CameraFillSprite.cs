using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera moving and autoscaling.
/// </summary>
public class CameraFillSprite : MonoBehaviour
{
	[Tooltip("Camera will autoscale to fit this object")]
	public SpriteRenderer focusObjectRenderer;										// Camera will autoscale to fit this object

	private Camera cam;																// Camera component from this gameobject
	private float originAspect;														// Origin camera aspect ratio

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		cam = GetComponent<Camera>();
		Debug.Assert (focusObjectRenderer && cam, "Wrong settings");
		originAspect = cam.aspect;
		UpdateCameraSize();
	}

	/// <summary>
	/// Lates update this instance.
	/// </summary>
    void LateUpdate()
    {
		// Camera aspect ratio is changed
		if (originAspect != cam.aspect)
		{
			UpdateCameraSize();
			originAspect = cam.aspect;
		}
    }

	/// <summary>
	/// Updates the size of the camera to fit focus object.
	/// </summary>
	private void UpdateCameraSize()
	{
		cam.orthographicSize = (focusObjectRenderer.bounds.max.x - focusObjectRenderer.bounds.min.x) / (2f * cam.aspect);
	}
}
