using UnityEngine;
using System.Collections;
using System;

public class AudioManager : MonoBehaviour
{

    #region Singleton
    private static AudioManager _audioManager;
    public static AudioManager Instance
    {
        get
        {
            if (_audioManager == null)
            {
                _audioManager = FindObjectOfType<AudioManager>();
                if (_audioManager == null) Debug.Log("The AudioManager Component is not available in the scene");
            }
            return _audioManager;
        }
    }
    #endregion

    [SerializeField] Sound[] sounds;
    [Space(20)]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource singleSFXSource;


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
    /// <summary>
    /// Gets Sound According to the name of the sound
    /// Checks if the sfx List's elements have finished playing or not
    /// if not, a new AudioSource is added to the gameObject and at the list at the same time
    /// </summary>
    /// <param name="p_soundName">Name of sound to play</param>
    public void PlaySound(string p_soundName)
    {
        Sound l_sound = Array.Find<Sound>(sounds, clip => clip.soundName == p_soundName);
        singleSFXSource.PlayOneShot(l_sound.clip);
    }

    /// <summary>
    /// mutes the music
    /// </summary>
    /// <param name="muteMusic">bool whether we have to mute the Audio</param>
    public void MuteMusic(bool muteMusic)=> musicSource.mute = muteMusic;

    /// <summary>
    /// mutes the audio
    /// </summary>
    /// <param name="muteAudio">bool whether we have to mute the Audio</param>
    public void MuteAudio(bool muteAudio) => singleSFXSource.mute = muteAudio;

    /// <summary>
    /// sets loudness to music
    /// and saves the value of current Volume
    /// </summary>
    /// <param name="p_volume">volume value to set</param>
    public void SetMusicVolume(float p_volume)
    {
        musicSource.volume = p_volume;
        SettingsData.MusicVolume = p_volume;
    }

    /// <summary>
    /// sets loudness to audio
    /// and saves the value of current Volume
    /// </summary>
    /// <param name="p_volume">volume value to set</param>
    public void SetSFXVolume(float p_volume)
    {
        singleSFXSource.volume = p_volume; 
        SettingsData.SfxVolume = p_volume;
    }

    /// <summary>
    /// Starts Coroutine after the paramtered time
    /// </summary>
    /// <param name="p_soundName">plays sound according to name</param>
    /// <param name="waitTime">plays sound after waiting for the parametered time</param>
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
}