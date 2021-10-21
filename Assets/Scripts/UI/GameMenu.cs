using System;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
	public class GameMenu : UIElement
	{
		[SerializeField] private Button resumeButton;
		[SerializeField] private Button menuButton;
		protected override void OnAwake()
		{
			resumeButton.onClick.AddListener(ResumeGame);
			menuButton.onClick.AddListener(GoToMenu);
			GameBus.OnGamePaused += OnGamePaused;
		}

		private void OnDestroy()
		{
			GameBus.OnGamePaused -= OnGamePaused;
			Time.timeScale = 1;
		}

		private void OnGamePaused()
		{
			if (gameObject.activeSelf)
				ResumeGame();
			else
				Show();
		}

		private void ResumeGame()
		{
			Time.timeScale = 1;
			Close();
		}

		private void GoToMenu()
		{
			SceneManager.LoadScene(0);
		}

		protected override void OnShown()
		{
			Time.timeScale = 0;
		}
	}
}