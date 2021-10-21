using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sound
{
	public class MusicPlayer : MonoBehaviour
	{
		[SerializeField] private bool isRandom;
		[SerializeField] private List<AudioClip> tracks;
		private AudioSource audioSource;
		private int currentTrack = -1;
		private static MusicPlayer instance;

		private void Awake()
		{
			if (instance != null)
			{
				Destroy(gameObject);
				return;
			}

			instance = this;
			DontDestroyOnLoad(this);
			audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
			PlayNextTrack();
		}

		private void PlayNextTrack()
		{
			currentTrack = isRandom ? Random.Range(0, tracks.Count - 1) : (currentTrack + 1) % tracks.Count;
			audioSource.PlayOneShot(tracks[currentTrack]);
			DOVirtual.DelayedCall(tracks[currentTrack].length, PlayNextTrack);
		}
	}
}