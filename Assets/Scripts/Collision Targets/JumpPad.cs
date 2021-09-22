using UnityEngine;


public class JumpPad : MonoBehaviour, ICollisionTarget
{
	[SerializeField] private float forceMultiplier = 10f;
	public void OnCollision(GameObject other)
	{
		other.GetComponent<Rigidbody2D>()?.AddForce(Vector2.up * forceMultiplier, ForceMode2D.Impulse);
	}
}