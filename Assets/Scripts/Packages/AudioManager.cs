using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System;
using System.Xml.Serialization;
using Unity.Mathematics;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _audioManager;
    public static AudioManager Instance
    {
        get
        {
            if (_audioManager == null)
            {
                _audioManager = FindObjectOfType<AudioManager>();
                if (_audioManager == null) Debug.Log("The ColorAdjuster Component is not available in the scene");
            }
            return _audioManager;
        }
    }

    [SerializeField] Sound[] sounds;
    [SerializeField] Sound[] loopingSounds;
    [Space(20)]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource singleSFXSource;
    [SerializeField] AudioSource loopingSFXSource;


    private void Awake()
    {
        if (_audioManager == null)
        {
            _audioManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        CheckForSettings();

        void CheckForSettings()
        {
            singleSFXSource.mute = !SettingsData.AllowSfx;
            musicSource.mute = !SettingsData.AllowMusic;
            singleSFXSource.volume = SettingsData.SfxVolume;
            musicSource.volume = SettingsData.MusicVolume;
        }
    }
    public void PlaySound(string p_soundName)
    {
        /*
         * Gets Sound According to the name of the sound
         * Checks if the sfx List's elements have finished playing or not
         * if not, a new AudioSource is added to the gameObject and at the list at the same time
        */
        Sound l_sound = Array.Find<Sound>(sounds, clip => clip.soundName == p_soundName);
        singleSFXSource.PlayOneShot(l_sound.clip);
    }

    public void MuteMusic(bool play)=> musicSource.mute = play;

    public void MuteAudio(bool play) => singleSFXSource.mute = play;

    public void SetMusicVolume(float p_volume)
    {
        musicSource.volume = p_volume;
        SettingsData.MusicVolume = p_volume;
    }
    public void SetSFXVolume(float p_volume)
    {
        singleSFXSource.volume = p_volume; 
        SettingsData.SfxVolume = p_volume;
    }

    public void PlaySound(string p_soundName,float waitTime)
    {
        StartCoroutine(PlaySoundAfterWait(p_soundName,waitTime));
    }

    IEnumerator PlaySoundAfterWait(string p_soundName, float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        /*
         * Gets Sound According to the name of the sound
         * Checks if the sfx List's elements have finished playing or not
         * if not, a new AudioSource is added to the gameObject and at the list at the same time
        */
        Sound l_sound = Array.Find<Sound>(sounds, clip => clip.soundName == p_soundName);
        singleSFXSource.PlayOneShot(l_sound.clip);
    }

    bool IsLoopable = true;
    Coroutine loopSFX;
    
    public void StartPlayingSFXOnLoop(string sfxToPlay)=> loopSFX = StartCoroutine(PlaySoundOnLoop(sfxToPlay));
    public void StartPlayingSFXOnLoop(string sfxToPlay, float waitTime) => loopSFX = StartCoroutine(PlaySoundOnLoop(sfxToPlay, waitTime));
    public void PlayEndingSfxAfterLoopingSfx(string sfxToPlayOnLoop, string sfxToPlayAtEnd) => StartCoroutine(PlaySoundOnLoop(sfxToPlayOnLoop,sfxToPlayAtEnd));
    public void PlayEndingSfxAfterLoopingSfx(string sfxToPlayOnLoop, string sfxToPlayAtEnd, float waitTime) =>  StartCoroutine(PlaySoundOnLoop(sfxToPlayOnLoop, waitTime, sfxToPlayAtEnd));


    IEnumerator PlaySoundOnLoop(string sfxName)
    {
        Sound l_sound = Array.Find<Sound>(sounds, clip => clip.soundName == sfxName);
        float timeInterval = l_sound.clip.length;
        IsLoopable = true;
        while (IsLoopable)
        {
            singleSFXSource.PlayOneShot(l_sound.clip);

            yield return new WaitForSeconds(timeInterval);
        }
    }
    IEnumerator PlaySoundOnLoop(string sfxName,string sfxNameToPlayAtEnd)
    {
        Sound l_sound = Array.Find<Sound>(sounds, clip => clip.soundName == sfxName);
        float timeInterval = l_sound.clip.length;
        IsLoopable = true;

        while (IsLoopable)
        {
            singleSFXSource.PlayOneShot(l_sound.clip);

            yield return new WaitForSeconds(timeInterval);
        }
        l_sound = Array.Find<Sound>(sounds, clip => clip.soundName == sfxName);
        singleSFXSource.PlayOneShot(l_sound.clip);

    }

    IEnumerator PlaySoundOnLoop(string sfxName,float waitTime)
    {
        Sound l_sound = Array.Find<Sound>(sounds, clip => clip.soundName == sfxName);
        float timeInterval = l_sound.clip.length;
        yield return new WaitForSeconds(waitTime);
        IsLoopable = true;

        while (IsLoopable)
        {
            singleSFXSource.PlayOneShot(l_sound.clip);

            yield return new WaitForSeconds(timeInterval);
        }
    }
    IEnumerator PlaySoundOnLoop(string sfxName, float waitTime, string sfxNameToPlayAtEnd)
    {
        Sound l_sound = Array.Find<Sound>(sounds, clip => clip.soundName == sfxName);
        float timeInterval = l_sound.clip.length;
        yield return new WaitForSeconds(waitTime);
        IsLoopable = true;

        while (IsLoopable)
        {
            singleSFXSource.PlayOneShot(l_sound.clip);

            yield return new WaitForSeconds(timeInterval);
        }
        l_sound = Array.Find<Sound>(sounds, clip => clip.soundName == sfxNameToPlayAtEnd);
        singleSFXSource.PlayOneShot(l_sound.clip);

    }
    public void StopSoundLoopAndShiftToAnotherSound() => IsLoopable = false;
    public void StopSoundLoopCoroutine() => StopCoroutine(loopSFX);

    public void StopAllSoundAtOnce()
    {
        StopAllCoroutines();
        singleSFXSource.Stop();
    }

    public void OnPause(bool pause)
    {
        if(pause) singleSFXSource.Pause();
        else singleSFXSource.UnPause();
    }
    #region oldScript
    /*
    public List<AudioSource> sfxSource;
    public void PlaySound(Sound p_sound)
    {
        
        CheckIfAnySourceIsEmpty(p_sound);
        void CheckIfAnySourceIsEmpty(Sound p_sound)
        {
            for (int i = 0; i < sfxSource.Count; i++)
            {
                if (!sfxSource[i].isPlaying)
                {
                    SetAudioSource(sfxSource[i], p_sound);
                    sfxSource[i].Play();
                    break;
                }
                else
                {
                    AddAudioSource(p_sound);
                }
            }
        }
    }

       private void CheckIfAnySoundIsOnAwake()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].playOnAwake)
            {
                PlaySound(sounds[i]);
            }
        }
    }

    void SetAudioSource(AudioSource p_AudioSource, Sound p_sound)
    {
        //Sets audioSource's values according to requirement
        p_AudioSource.volume = p_sound.volume;
        p_AudioSource.clip = p_sound.clip;
        p_AudioSource.loop = p_sound.loop;
        p_AudioSource.playOnAwake = p_sound.playOnAwake;
        p_AudioSource.pitch = p_sound.pitch;
        p_AudioSource.mute = p_sound.mute;
        p_AudioSource.outputAudioMixerGroup = p_sound.mixerGroup;
    }

    void AddAudioSource(Sound p_sound)
    {
        //Adds AudioSource to the gameObject
        //And sets the value accordind to the sound instance's requirement
        //Then adds the audioSource to the list
        AudioSource l_audio = gameObject.AddComponent<AudioSource>();
        SetAudioSource(l_audio, p_sound);
        sfxSource.Add(l_audio);
    }
    */
    #endregion 
}
