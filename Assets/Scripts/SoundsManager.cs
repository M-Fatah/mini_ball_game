using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
	public AudioSource musicAudioSource;
    public AudioSource SFXAudioSource;

	public static SoundsManager instance = null;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
        else if (instance != this)
        {
            Destroy(gameObject);
        }

		DontDestroyOnLoad(gameObject);
	}

	public void PlaySound(AudioClip clip, Vector3 position)
	{
		if(clip)
		{
			AudioSource.PlayClipAtPoint(clip, position);
		}
	}

	public void PlaySFX(AudioClip clip)
	{
		if(clip)
		{
			SFXAudioSource.clip = clip;
			SFXAudioSource.Play();
		}
	}
}
