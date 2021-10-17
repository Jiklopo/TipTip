using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sound
{
	[RequireComponent(typeof(AudioSource))]
	public class SoundPlayer : MonoBehaviour
	{
		[SerializeField] private List<SoundData> soundData = new List<SoundData>();

		private AudioSource audioSource;

		private void Awake()
		{
			audioSource = GetComponent<AudioSource>();
		}

		public void PlayClip(string clipName, bool loop = false)
		{
			audioSource.loop = loop;
			audioSource.PlayOneShot(GetAudioClip(clipName));
		}

		private AudioClip GetAudioClip(string clipName)
		{
			var data = soundData.FirstOrDefault(data => data.name.Equals(clipName)).sound;
			if(data == null)
				Debug.LogWarning($"There is no sound data with a name {clipName}!");
			return data;
		}
		
		[Serializable]
		private struct SoundData
		{
			public string name;
			public AudioClip sound;
		}
	}
}