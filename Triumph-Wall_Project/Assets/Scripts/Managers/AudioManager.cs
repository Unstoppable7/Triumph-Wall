using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton region
    private static AudioManager instance = null;
    public static AudioManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if(instance == null)
                {
                    instance = new GameObject("AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
                
            }
            return instance;
        }

        set
        {
            instance = value;
        }
    }
    #endregion

    #region Fields
    private AudioSource musicSource = null;
    private AudioSource musicSource2 = null;
    private AudioSource sfxSource = null;

    private bool firstMusicSourceIsPlaying = false;
    #endregion

    //Create needed components
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource2 = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
        musicSource2.loop = true;
    }

    public void PlayMusic(AudioClip musicClip)
    {
        AudioSource activeSource = getActiveMusicAudioSource();

        activeSource.clip = musicClip;
        activeSource.volume = 1.0f;
        activeSource.Play();
    }
    public void PlayMusicWithFade(AudioClip newClip, float transitionTime = 1.0f)
    {
        StartCoroutine(UpdateMusicWithFade(getActiveMusicAudioSource(), newClip, transitionTime));
    }
    public void PlayMusicWithCrossFade(AudioClip newClip, float transitionTime = 1.0f)
    {
        AudioSource activeSource = getActiveMusicAudioSource();
        AudioSource newSource = getInactiveMusicAudioSource();

        firstMusicSourceIsPlaying = !firstMusicSourceIsPlaying;

        newSource.clip = newClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, transitionTime));
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        sfxSource.PlayOneShot(sfxClip);
    }
    public void PlaySFX(AudioClip sfxClip, float volume)
    {
        sfxSource.PlayOneShot(sfxClip, volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        musicSource2.volume = volume;
    }
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    private AudioSource getActiveMusicAudioSource()
    {
        return (firstMusicSourceIsPlaying) ? musicSource : musicSource2;
    }
    private AudioSource getInactiveMusicAudioSource()
    {
        return (firstMusicSourceIsPlaying) ? musicSource2 : musicSource;
    }

    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip newClip, float transitionTime)
    {
        if (!activeSource.isPlaying)
            activeSource.Play();

        float t = 0.0f;

        //Fade out
        for (t = 0.0f; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = (1 - (t / transitionTime));
            yield return null;
        }

        activeSource.Stop();
        activeSource.clip = newClip;
        activeSource.Play();

        //Fade in
        for (t = 0.0f; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = (t / transitionTime);
            yield return null;
        }
    }
    private IEnumerator UpdateMusicWithCrossFade(AudioSource original, AudioSource newSource, float transitionTime)
    {
        float t = 0.0f;

        for (t = 0.0f; t < transitionTime; t += Time.deltaTime)
        {
            original.volume = (1 - (t / transitionTime));
            newSource.volume = (t / transitionTime);
            yield return null;
        }

        original.Stop();
    }
}
