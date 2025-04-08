using UnityEngine;

public class FollowTargetSmooth : MonoBehaviour
{
	public Transform target;
	public float smoothSpeed = 0.125f;
	public Vector3 offset;

	private void LateUpdate()
	{
		if (target == null) return;

		Vector3 desiredPosition = target.position + offset;

		// Conservamos el valor original del eje Z de la cámara
		desiredPosition.z = transform.position.z;

		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
		transform.position = smoothedPosition;
	}
}
