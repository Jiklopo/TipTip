using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
	public abstract class UIElement : MonoBehaviour
	{
		protected Action onClose;
		protected UIElement lastCaller;
		[SerializeField] private float animationTime = 0.3f;

		private void Awake()
		{
			OnAwake();
		}
		
		protected virtual void OnAwake(){}

		public void Show()
		{
			transform.localScale = Vector3.zero;
			gameObject.SetActive(true);
			transform.DOScale(Vector3.one, animationTime);
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
			gameObject.SetActive(false);
			lastCaller?.Show();
			onClose?.Invoke();

			lastCaller = null;
			onClose = null;
		}
	}
}