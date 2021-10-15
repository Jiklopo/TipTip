using System;
using UnityEngine;


public class JumpPad : MonoBehaviour, ICollisionTarget
{
	private Animation animation;
	[SerializeField] private float forceMultiplier = 10f;

	private void Awake()
	{
		animation = GetComponent<Animation>();
	}

	public void OnCollision(GameObject other)
	{
		other.GetComponent<Rigidbody2D>()?.AddForce(Vector2.up * forceMultiplier, ForceMode2D.Impulse);
		animation.Play();
	}
}