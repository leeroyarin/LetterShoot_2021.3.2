using System;
using UnityEngine;

public static class SettingsData
{

    //Visual
    public static float Contrast { get => PlayerPrefs.GetFloat("ContrastValue", 0); set => PlayerPrefs.SetFloat("ContrastValue", value); }
    public static float Brightness { get => PlayerPrefs.GetFloat("BrightnessValue", 0); set => PlayerPrefs.SetFloat("BrightnessValue", value); }
    public static float Saturation { get => PlayerPrefs.GetFloat("SaturationValue", 0); set => PlayerPrefs.SetFloat("SaturationValue", value); }

    //Audio
    public static float MusicVolume { get => PlayerPrefs.GetFloat("MusicVolume", 10); set => PlayerPrefs.SetFloat("MusicVolume", value); }
    public static float SfxVolume { get => PlayerPrefs.GetFloat("AudioVolume", 10); set => PlayerPrefs.SetFloat("AudioVolume", value); }

    // gets bool value from out and sets into int value, and returns bool according to int 
    public static bool AllowMusic { get => Convert.ToBoolean(PlayerPrefs.GetInt("MusicPlayable", 1)); set => PlayerPrefs.SetInt("MusicPlayable", Convert.ToUInt16(value)); }
    public static bool AllowSfx { get => Convert.ToBoolean(PlayerPrefs.GetInt("SoundPlayable", 1)); set => PlayerPrefs.SetInt("SoundPlayable", Convert.ToUInt16(value)); }

    public static bool IsMobileDevice { get => Convert.ToBoolean(PlayerPrefs.GetInt("IsMobileDevice",0)); set => PlayerPrefs.SetInt("IsMobileDevice", Convert.ToUInt16(value)); }

}