using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerExtended : SingletonComponent<AudioManagerExtended>
{

    #region Fields
    private AudioSource musicSource = null;
    private AudioSource musicSource2 = null;
    private AudioSource sfxSource = null;

    [SerializeField]
    private AudioMixer mixer;

    [SerializeField, MinMaxRange(-80.0f, 20.0f)]
    private RangedFloat decibelsRange = new RangedFloat( 20.0f, -80.0f);

    private bool firstMusicSourceIsPlaying = false;
    
    #endregion

    //Create needed components
    public override void Awake()
    {
        base.Awake();

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource2 = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
        musicSource2.loop = true;

        if(mixer != null)
        {
            musicSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];
            musicSource2.outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];
            sfxSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        }
        else
        {
            Debug.LogWarning("No se ha encontrado ningún AudioMixer asociado al AudioManager.");
        }
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

    public void PlaySFX(AudioEvent sfxEvent)
    {
        sfxEvent.Play(sfxSource);
    }
    public void PlaySFX(AudioClip sfxClip)
    {
        sfxSource.PlayOneShot(sfxClip);
    }
    public void PlaySFX(AudioClip sfxClip, float volume)
    {
        sfxSource.PlayOneShot(sfxClip, volume);
    }
    public void PlayLocatedSFX(AudioEvent sfxEvent, AudioSource source)
    {
        sfxEvent.Play(source);
    }
    public void PlayDelayedSFX(AudioClip sfxClip, float timeDelay)
    {
        sfxSource.clip = sfxClip;
        sfxSource.PlayDelayed(timeDelay);
    }

    public void PauseSound()
    {
        AudioListener.pause = true;
    }
    public void ResumeSound()
    {
        AudioListener.pause = false;
    }
    
    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVolume", Remap(Mathf.Clamp01(volume), 0, 1, decibelsRange.minValue, decibelsRange.maxValue));
    }
    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("SFXVolume", Remap(Mathf.Clamp01(volume), 0, 1, decibelsRange.minValue, decibelsRange.maxValue));
    }
    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("MasterVolume", Remap(Mathf.Clamp01(volume), 0, 1, decibelsRange.minValue, decibelsRange.maxValue));
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

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
