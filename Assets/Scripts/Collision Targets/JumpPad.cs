using System;
using Sound;
using UnityEngine;


public class JumpPad : MonoBehaviour, ICollisionTarget
{
	[SerializeField] private float forceMultiplier = 10f;
	private Animation animation;
	private SoundPlayer soundPlayer;

	private void Awake()
	{
		animation = GetComponent<Animation>();
		soundPlayer = GetComponent<SoundPlayer>();
	}

	public void OnCollision(GameObject other)
	{
		other.GetComponent<Rigidbody2D>()?.AddForce(Vector2.up * forceMultiplier, ForceMode2D.Impulse);
		soundPlayer.PlayClip("jump");
		animation.Play();
	}
}