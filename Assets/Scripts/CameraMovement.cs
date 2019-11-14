using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	[HideInInspector]
	public Transform target;
	[Min(1)]
	public float smoothing;

	[HideInInspector]
	public Vector2 maxPosition;
	[HideInInspector]
	public Vector2 minPosition;

	// Late update so that the camera moves after the player has moved in Update()
	void LateUpdate()
	{
		if (transform.position != target.position)
		{
			Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

			targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
			targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);

			transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
		}
	}
}
