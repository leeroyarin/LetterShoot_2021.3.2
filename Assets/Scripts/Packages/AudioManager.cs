using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System;
using System.Xml.Serialization;

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
    [Space(20)]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource singleSFXSource;
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
