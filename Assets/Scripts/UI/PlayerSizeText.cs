using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
	public class PlayerSizeText : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI playerSizeText;

		private void Awake()
		{
			PlayerController.OnTotalSizeChange += UpdateText;
			UpdateText(PlayerController.TotalSize);
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