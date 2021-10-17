using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
	public class LevelSelectScreen : UIElement
	{
		[SerializeField] private Button closeButton;
		[SerializeField] private RectTransform levelsGrid;
		[SerializeField] private Button levelButtonPrefab;

		protected override void OnAwake()
		{
			closeButton.onClick.AddListener(Close);
			GenerateLevelsButton();
		}

		private void GenerateLevelsButton()
		{
			for (var i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
			{
				var button = Instantiate(levelButtonPrefab, levelsGrid);
				var sceneIndex = i;
				button.onClick.AddListener(() => LoadLevel(sceneIndex));
				button.GetComponentInChildren<TextMeshProUGUI>()?.SetText(i.ToString());
			}
		}

		private void LoadLevel(int buildIndex)
		{
			SceneManager.LoadScene(buildIndex);
		}
	}
}