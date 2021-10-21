using System;
using Events;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
	public class LevelCompletedScreen : UIElement
	{
		[SerializeField] private Button nextLevelButton;
		[SerializeField] private Button menuButton;
		protected override void OnAwake()
		{
			nextLevelButton.onClick.AddListener(NextLevel);
			menuButton.onClick.AddListener(GoToMenu);
			GameBus.OnLevelCompleted += Show;
		}

		private void OnEnable()
		{
			var areLevelsLeft = SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1;
			nextLevelButton.gameObject.SetActive(areLevelsLeft);
		}

		private void NextLevel()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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