using System;
using TMPro;
using UnityEngine;

namespace UI
{
	public class PlayerSize : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI playerSizeText;

		private void Awake()
		{
			PlayerController.OnTotalSizeChange += UpdateText;
		}

		private void OnDestroy()
		{
			PlayerController.OnTotalSizeChange -= UpdateText;
		}

		private void UpdateText(int size)
		{
			playerSizeText.SetText($"Total Size: {size}");
		}
	}
}