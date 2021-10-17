using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
	public class GameOverScreen : MonoBehaviour
	{
		[SerializeField] private Button restartButton;
		[SerializeField] private Button menuButton;
		private void Awake()
		{
			restartButton.onClick.AddListener(RestartLevel);
			menuButton.onClick.AddListener(GoToMenu);
			PlayerController.OnTotalSizeChange += CheckPlayer;
			gameObject.SetActive(false);
		}

		private void OnDestroy()
		{
			PlayerController.OnTotalSizeChange -= CheckPlayer;
		}

		private void CheckPlayer(int totalSize)
		{
			if (totalSize > 0) 
				return;
			Time.timeScale = 0;
			gameObject.SetActive(true);
		}

		private void RestartLevel()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		private void GoToMenu()
		{
			SceneManager.LoadScene(0);
		}
	}
}