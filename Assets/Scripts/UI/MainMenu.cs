using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
	public class MainMenu : UIElement
	{
		[SerializeField] private LevelSelectScreen levelSelectScreen;
		[SerializeField] private Button playButton;
		[SerializeField] private Button levelsButton;
		[SerializeField] private Button exitButton;

		protected override void OnAwake()
		{
			playButton.onClick.AddListener(StartFirstLevel);
			levelsButton.onClick.AddListener(ShowLevelsPanel);
			exitButton.onClick.AddListener(Quit);
		}

		private void StartFirstLevel()
		{
			SceneManager.LoadScene(1);
		}

		private void ShowLevelsPanel()
		{
			levelSelectScreen.Show(this);
		}

		private void Quit()
		{
			Application.Quit();
		}
	}
}