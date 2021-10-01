using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICollisionTarget
{
	[SerializeField] private float movementForce;
	[SerializeField] private float jumpForce;
	[SerializeField] private int size = 1;

	public static PlayerController parentPlayer { get; private set; }
	private static int priorityCounter;

	private Rigidbody2D rb;
	private PlayerInputActions inputActions;
	private Animator animator;

	private bool isJumping;
	private bool isJoining;
	private int priority;

	private int Size
	{
		get => size;
		set
		{
			size = value;
			RefreshSize();
		}
	}

	private void RefreshSize()
	{
		if (size <= 0)
		{
			Debug.Log("Player Dead :c");
			Destroy(gameObject);
			return;
		}
			
		transform.localScale = Vector3.one * size;
	}

	private void Awake()
	{
		if (parentPlayer == null)
			parentPlayer = this;

		priority = priorityCounter++;
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		inputActions = new PlayerInputActions();

		AssignInputCallbacks();
		RefreshSize();
	}


	private void Update()
	{
		animator.SetBool("isRunning", rb.velocity.magnitude >= Mathf.Epsilon);
		var moveDir = isJoining
			? (Vector2) (parentPlayer.transform.position - transform.position).normalized
			: inputActions.Player.Move.ReadValue<Vector2>();

		Move(moveDir);
	}

	private void Move(Vector2 direction)
	{
		rb.AddForce(direction * movementForce);
	}

	private void Jump()
	{
		if (isJumping || isJoining)
			return;

		isJumping = true;
		rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
	}

	private void StartJoining()
	{
		isJoining = true;
	}

	private void StopJoining()
	{
		isJoining = false;
	}

	private static void JoinPlayers(PlayerController first, PlayerController second)
	{
		if (!first.gameObject.activeSelf || !second.gameObject.activeSelf)
			return;

		var winner = first.priority < second.priority ? first : second;
		var loser = first.priority > second.priority ? first : second;

		loser.gameObject.SetActive(false);
		winner.Size += loser.Size;
	}

	private void Split()
	{
		while (Size > 1)
		{
			Size--;
			Instantiate(this, transform.position, Quaternion.identity).Size = 1;
		}
	}

	private void OnEnable()
	{
		inputActions.Enable();
	}

	private void OnDisable()
	{
		inputActions.Disable();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Ground"))
			isJumping = false;
		
		other.gameObject.GetComponent<ICollisionTarget>()?.OnCollision(gameObject);
	}

	public void OnCollision(GameObject other)
	{
		if (isJoining && other.CompareTag("Player"))
		{
			JoinPlayers(this, other.GetComponent<PlayerController>());
		}
	}

	private void AssignInputCallbacks()
	{
		inputActions.Player.Jump.performed += context => Jump();
		inputActions.Player.Join.started += context => StartJoining();
		inputActions.Player.Join.canceled += context => StopJoining();
		inputActions.Player.Split.performed += context => Split();
	}

	public void ChangeSize(int amount)
	{
		if (amount > 0)
		{
			animator.SetBool("isEating", true);
			StartCoroutine(EatingRoutine());
		}

		Size += amount;
	}

	private IEnumerator EatingRoutine()
	{
		yield return new WaitForSeconds(2);
		animator.SetBool("isEating", false);
	}

	private void OnDestroy()
	{
		if (Equals(parentPlayer))
		{
			parentPlayer = FindObjectOfType<PlayerController>();
			parentPlayer.priority = 0;
		}
	}
}