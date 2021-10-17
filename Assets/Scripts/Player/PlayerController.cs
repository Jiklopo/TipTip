using System;
using System.Collections;
using Events;
using Sound;
using UnityEngine;

namespace Player
{
	public class PlayerController : MonoBehaviour, ICollisionTarget
	{
		[SerializeField] private float maxVelocity;
		[SerializeField] private float movementForce;
		[SerializeField] private float jumpForce;
		[SerializeField] private int size = 1;
		public int Size
		{
			get => size;
			set
			{
				size = value;
				RefreshSize();
			}
		}
	
		private static int totalSize;
		public static int TotalSize
		{
			get => totalSize;
			set
			{
				totalSize = value;
				OnTotalSizeChange?.Invoke(totalSize);
			}
		}

		public static PlayerController ParentPlayer { get; private set; }
		public static Action<int> OnTotalSizeChange;
		private static int priorityCounter;

		private Rigidbody2D rb;
		private PlayerInputActions inputActions;
		private Animator animator;
		private SoundPlayer soundPlayer;

		private bool isJumping;
		private bool isJoining;
		private int priority;
		private static readonly int IsRunning = Animator.StringToHash("isRunning");
		private static readonly int IsEating = Animator.StringToHash("isEating");

		private void Awake()
		{
			if (ParentPlayer == null)
			{
				ParentPlayer = this;
				TotalSize = size;
				Time.timeScale = 1;
			}

			priority = priorityCounter++;
			rb = GetComponent<Rigidbody2D>();
			animator = GetComponent<Animator>();
			soundPlayer = GetComponent<SoundPlayer>();
			inputActions = new PlayerInputActions();

			AssignInputCallbacks();
			RefreshSize();
		}
	
		private void Update()
		{
			animator.SetBool(IsRunning, rb.velocity.magnitude >= Mathf.Epsilon);
			var moveDir = isJoining
				? (Vector2) (ParentPlayer.transform.position - transform.position).normalized
				: inputActions.Player.Move.ReadValue<Vector2>();

			Move(moveDir);
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

		private void OnDestroy()
		{
			if (!Equals(ParentPlayer)) return;
			ParentPlayer = FindObjectOfType<PlayerController>();
			ParentPlayer.priority = 0;
		}

		private void Move(Vector2 direction)
		{
			rb.AddForce(direction * (movementForce * (1 - rb.velocity.magnitude / maxVelocity)));
		}

		private void Jump()
		{
			if (isJumping || isJoining)
				return;

			isJumping = true;
			rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
			soundPlayer.PlayClip("jump");
		}

		private void StartJoining()
		{
			isJoining = true;
			soundPlayer.PlayClip("joining", true);
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
			winner.soundPlayer.PlayClip("join");
		}

		private void Split()
		{
			while (Size > 1)
			{
				Size--;
				Instantiate(this, transform.position, Quaternion.identity).Size = 1;
			}
		}

		private void AssignInputCallbacks()
		{
			inputActions.Player.Jump.performed += context => Jump();
			inputActions.Player.Join.started += context => StartJoining();
			inputActions.Player.Join.canceled += context => StopJoining();
			inputActions.Player.Split.performed += context => Split();
			inputActions.Player.Pause.performed += context => GameBus.OnGamePaused.Invoke();
		}

		public void ChangeSize(int amount)
		{
			if (amount > 0)
				StartCoroutine(EatingRoutine());
			else
				soundPlayer.PlayClip("damage");

			Size += amount;
			TotalSize += amount;
		}

		private void RefreshSize()
		{
			if (size <= 0)
			{
				soundPlayer.PlayClip("dead");
				Destroy(gameObject);
				return;
			}

			transform.localScale = Vector3.one * size;
		}

		private IEnumerator EatingRoutine()
		{
			soundPlayer.PlayClip("eat");
			animator.SetBool(IsEating, true);
			yield return new WaitForSeconds(2);
			animator.SetBool(IsEating, false);
		}
	}
}