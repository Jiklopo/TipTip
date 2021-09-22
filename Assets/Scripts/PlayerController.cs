using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float movementForce;
	[SerializeField] private float jumpForce;
	[SerializeField] private int size = 1;

	public static PlayerController parentPlayer { get; private set; }
	private static int priorityCounter = 0;

	private Rigidbody2D rb;
	private PlayerInputActions inputActions;

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
		transform.localScale = Vector3.one * size;
	}

	private void Awake()
	{
		if (parentPlayer == null)
			parentPlayer = this;

		priority = priorityCounter++;
		rb = GetComponent<Rigidbody2D>();
		inputActions = new PlayerInputActions();

		AssignInputCallbacks();
		RefreshSize();
	}


	private void Update()
	{
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

		else if (isJoining && other.gameObject.CompareTag("Player"))
		{
			var otherPlayer = other.gameObject.GetComponent<PlayerController>();
			JoinPlayers(this, otherPlayer);
		}
	}

	private void AssignInputCallbacks()
	{
		inputActions.Player.Jump.performed += context => Jump();
		inputActions.Player.Join.started += context => StartJoining();
		inputActions.Player.Join.canceled += context => StopJoining();
		inputActions.Player.Split.performed += context => Split();
	}
}