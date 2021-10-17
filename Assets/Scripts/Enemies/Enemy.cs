using System.Collections;
using Player;
using Sound;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour, ICollisionTarget
{
	[SerializeField] protected int hp;
	[SerializeField] protected int strength;
	[SerializeField] protected float speed;
	[SerializeField] protected float viewRadius;
	[SerializeField] protected float roamRange;
	[SerializeField] protected float attackInterval = 3f;

	protected Vector3 currentRoamPoint;
	protected Rigidbody2D rb;
	protected Animator animator;
	protected SoundPlayer soundPlayer;
	protected Transform player;
	protected float lastAttackTime = Mathf.NegativeInfinity;
	protected bool IsChasing => player != null;
	protected bool CanAttack => Time.time - lastAttackTime >= attackInterval;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		soundPlayer = GetComponent<SoundPlayer>();
		
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
		if (Vector3.Distance(transform.position, currentRoamPoint) <= 1)
			currentRoamPoint = GetRandomRoamPoint();
		
		Move(currentRoamPoint - transform.position);
	}

	protected virtual void Move(Vector2 direction)
	{
		animator.SetBool("walk", direction.magnitude != 0);
		var localScale = transform.localScale;
		var absX = Mathf.Abs(localScale.x);
		transform.localScale = new Vector3(direction.x < 0? absX: absX * -1, localScale.y, localScale.z);
		rb.AddForce(direction.normalized * speed);
	}

	private Vector3 GetRandomRoamPoint()
	{
		return transform.position + Vector3.right * Random.Range(-roamRange, roamRange);
	}

	public void OnCollision(GameObject other)
	{
		if(!CanAttack)
			return;
		
		var playerController = other.GetComponent<PlayerController>();
		if (playerController != null)
		{
			playerController.ChangeSize(-strength);
			soundPlayer.PlayClip("attack");
			animator.SetTrigger("attack");
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !IsChasing)
		{
			player = other.transform;
			soundPlayer.PlayClip("notice");			
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (!other.CompareTag("Player") || !player.Equals(other.transform)) 
			return;
		
		soundPlayer.PlayClip("leave");
		player = null;
		currentRoamPoint = GetRandomRoamPoint();
	}
}