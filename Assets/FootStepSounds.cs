using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSounds : MonoBehaviour
{

    public List<AudioClip> soundsList;
	public float stepDistance;
	public float distance;

	public float randomPitch = 0.2f;

	AudioSource audioSrc;

	private void Awake()
	{
        audioSrc = GetComponent<AudioSource>();
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
		//passe un sound dans l'audiosource, son aléatoire dans la liste des sons
		audioSrc.clip = soundsList[Random.Range(0, soundsList.Count)];

		audioSrc.pitch = Random.Range(1f - randomPitch, 1f + randomPitch);

		audioSrc.Play();
	}
}
