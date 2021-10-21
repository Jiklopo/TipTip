using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
	public abstract class UIElement : MonoBehaviour
	{
		[SerializeField] private bool showAtStart;
		[SerializeField] private float animationTime = 0.3f;
		protected Action onClose;
		protected UIElement lastCaller;

		private void Awake()
		{
			OnAwake();
		}

		protected virtual void OnAwake()
		{
		}

		private void Start()
		{
			gameObject.SetActive(showAtStart);
		}

		public void Show(UIElement caller)
		{
			lastCaller = caller;
			caller.Close();
			Show();
		}

		public void Show()
		{
			gameObject.SetActive(true);
			transform.localScale = Vector3.zero;
			transform.DOScale(Vector3.one, animationTime).OnComplete(OnShown);
		}

		protected virtual void OnShown()
		{
		}

		public void Close(Action onClose = null)
		{
			transform
				.DOScale(Vector3.zero, animationTime)
				.OnComplete(() =>
				{
					gameObject.SetActive(false);
					OnClosed();
				});

			lastCaller?.Show();
			lastCaller = null;
			onClose?.Invoke();
		}

		protected virtual void OnClosed()
		{
		}
	}
}