using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FootStepSounds : MonoBehaviour
{

	public List<AudioClip> soundsList;
	public float stepDistance;
	public float distance;

	public float randomPitch = 0.2f;

	AudioSource[] audioSrc;
	int nextSource = 0;


	private void Awake()
	{
		audioSrc = GetComponents<AudioSource>();
	}


	private void Update()
	{
		if (distance > stepDistance)
		{
			PlaySound();
			distance = 0;
		}
	}

	void PlaySound()
	{
		//passe un sound dans l'audiosource, son al�atoire dans la liste des sons
		audioSrc[nextSource].clip = soundsList[Random.Range(0, soundsList.Count)];

		audioSrc[nextSource].pitch = Random.Range(1f - randomPitch, 1f + randomPitch);

		audioSrc[nextSource].Play();

		nextSource++;

		if (nextSource > 1)
		{
			nextSource = 0;
		}

	}
}