using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

	static AudioManager instance;

	public AudioClip bubbles;
	public AudioClip wrong;
	public AudioClip general;
	public AudioClip right;
	public AudioClip uiSound;
	public AudioClip[] music;
	public AudioClip mainMusic;

	public AudioSource mySource;
	public AudioSource musicSource;
	public AudioSource musicSourceB;

	int currentDeck;

	public enum AudioType
	{
		WRONG,
		GENERAL,
		RIGHT,
		UISOUND,
		BUBBLESOUND
	}

	public static AudioManager Manager
	{
		get
		{
			return instance;
		}
	}

	void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		currentDeck = 1;
	}

	public void PlayFX(AudioType audioToPlay)
	{
		switch (audioToPlay)
		{
			case AudioType.WRONG:
				mySource.clip = wrong;
				break;
			case AudioType.GENERAL:
				mySource.clip = general;
				break;
			case AudioType.RIGHT:
				mySource.clip = right;
				break;
			case AudioType.UISOUND:
				mySource.clip = uiSound;
				break;
			case AudioType.BUBBLESOUND:
				mySource.clip = bubbles;
				break;
		}

		mySource.Play();
	}

	public void StopMusic()
	{
		if (currentDeck == 1)
			musicSource.Stop();
		else
			musicSourceB.Stop();
	}

	public void PlayMusic(bool mainTheme)
	{
		if (PlayerPrefs.GetInt("Music", 1) == 0)
		{
			return;
		}

		if (mainTheme)
		{
			if (currentDeck == 2)
			{
				musicSource.clip = mainMusic;
				musicSource.Play();
				currentDeck = 1;
			}
			else
			{
				musicSourceB.clip = mainMusic;
				musicSourceB.Play();
				currentDeck = 2;
			}
			return;
		}

		int trackId = Random.Range(0, music.Length);

		if (currentDeck == 2)
		{
			musicSource.clip = music[trackId];
			musicSource.Play();
			currentDeck = 1;
		}
		else
		{
			musicSourceB.clip = music[trackId];
			musicSourceB.Play();
			currentDeck = 2;
		}
	}
	
}
