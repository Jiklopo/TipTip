using System;
using Player;
using Sound;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
	public class GameOverScreen : UIElement
	{
		[SerializeField] private Button restartButton;
		[SerializeField] private Button menuButton;
		private SoundPlayer soundPlayer;
		protected override void OnAwake()
		{
			restartButton.onClick.AddListener(RestartLevel);
			menuButton.onClick.AddListener(GoToMenu);
			PlayerController.OnTotalSizeChange += CheckPlayer;
			soundPlayer = GetComponent<SoundPlayer>();
		}

		private void OnDestroy()
		{
			PlayerController.OnTotalSizeChange -= CheckPlayer;
		}

		private void CheckPlayer(int totalSize)
		{
			if (totalSize > 0) 
				return;
			Show();
		}

		private void RestartLevel()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		private void GoToMenu()
		{
			SceneManager.LoadScene(0);
		}

		protected override void OnShown()
		{
			soundPlayer.PlayClip("game over");
			Time.timeScale = 0;
		}
	}
}