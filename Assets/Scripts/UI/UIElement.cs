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

		private void Start()
		{
			gameObject.SetActive(showAtStart);
		}

		protected virtual void OnAwake(){}

		public void Show()
		{
			transform.localScale = Vector3.zero;
			gameObject.SetActive(true);
			transform.DOScale(Vector3.one, animationTime).OnComplete(OnShown);
		}

		protected virtual void OnShown()
		{
		}

		public void Show(UIElement caller, Action onClose)
		{
			this.onClose = onClose;
			lastCaller = caller;
			caller.Close();
			Show();
		}

		public virtual void Close()
		{
			transform
				.DOScale(Vector3.zero, animationTime)
				.OnComplete(() => gameObject.SetActive(false));
			
			lastCaller?.Show();
			onClose?.Invoke();

			lastCaller = null;
			onClose = null;
		}
	}
}