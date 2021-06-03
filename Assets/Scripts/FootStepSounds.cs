using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FootStepSounds : MonoBehaviour
{

	public List<AudioClip> soundsListSnow;
	public List<AudioClip> soundsListGround;
	public LayerMask lmask;
	public float stepDistance;
	public float distance;
	public float snowFactor = 0.2f;

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
		if (SampleTexture() > snowFactor)
		{
			audioSrc[nextSource].clip = soundsListSnow[Random.Range(0, soundsListSnow.Count)];
		}
		else
		{
			audioSrc[nextSource].clip = soundsListGround[Random.Range(0, soundsListSnow.Count)];
		}

		//passe un sound dans l'audiosource, son aléatoire dans la liste des sons

		audioSrc[nextSource].pitch = Random.Range(1f - randomPitch, 1f + randomPitch);

		audioSrc[nextSource].Play();

		nextSource++;

		if (nextSource > 1)
		{
			nextSource = 0;
		}

	}

	float SampleTexture()
	{

		RaycastHit hit;
		if (!Physics.Raycast(transform.position, Vector3.down * 10f, out hit, lmask))
		{
			return 0f;
		}

		Debug.DrawRay(transform.position, Vector3.down * 10f, Color.red, 1f);

		Renderer rend = hit.transform.GetComponent<Renderer>();
		MeshCollider meshCollider = hit.collider as MeshCollider;

		if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
		{
			return 0f;
		}

		Texture2D tex = rend.material.mainTexture as Texture2D;
		Vector2 pixelUV = hit.textureCoord;
		pixelUV.x *= tex.width;
		pixelUV.y *= tex.height;

		Color c = tex.GetPixel((int)pixelUV.x, (int)pixelUV.y);
		return (c.r + c.g + c.b) / 3f;
	}


}
