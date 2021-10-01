﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour, ICollisionTarget
{
	[SerializeField] protected int hp;
	[SerializeField] protected int strength;
	[SerializeField] protected float speed;
	[SerializeField] protected float viewRadius;
	[SerializeField] protected float roamRange;

	protected Vector3 currentRoamPoint;
	protected Rigidbody2D rb;
	protected Transform player;
	protected bool IsChasing => player != null;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		
		var trigger = gameObject.AddComponent<CircleCollider2D>();
		trigger.isTrigger = true;
		trigger.radius = viewRadius;

		currentRoamPoint = GetRandomRoamPoint();

		OnAwake();
	}

	protected virtual void OnAwake()
	{
	}

	private void Update()
	{
		if (IsChasing)
			FollowPlayer();
		else
			Roam();
	}

	protected virtual void FollowPlayer()
	{
		Move(player.position - transform.position);
	}

	protected virtual void Roam()
	{
		if (Vector3.Distance(transform.position, currentRoamPoint) <= Mathf.Epsilon)
			currentRoamPoint = GetRandomRoamPoint();
		
		Move(currentRoamPoint - transform.position);
	}

	protected virtual void Move(Vector2 direction)
	{
		rb.AddForce(direction.normalized * speed);
	}

	private Vector3 GetRandomRoamPoint()
	{
		return transform.position + Vector3.right * Random.Range(0f, roamRange);
	}

	public void OnCollision(GameObject other)
	{
		other.GetComponent<PlayerController>()?.ChangeSize(-strength);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !IsChasing)
			player = other.transform;
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player") && player.Equals(other.transform))
			player = null;
	}
}