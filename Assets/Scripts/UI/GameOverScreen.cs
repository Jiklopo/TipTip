using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
	public class GameOverScreen : MonoBehaviour
	{
		[SerializeField] private Button exitButton;
		[SerializeField] private Button menuButton;
		private void Awake()
		{
			exitButton.onClick.AddListener(Exit);
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

		private void Exit()
		{
			Application.Quit();
		}

		private void GoToMenu()
		{
			SceneManager.LoadScene(0);

		}
	}
}