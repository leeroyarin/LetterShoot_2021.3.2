using UnityEngine;
using UnityEngine.UI;

public class SettingManager: MonoBehaviour
{
    [SerializeField] Slider _brightnessSlider;
    [SerializeField] Slider _contrastSlider;
    [SerializeField] Slider _saturationSlider;
    [SerializeField] Slider _audioSlider;
    [SerializeField] Slider _musicSlider;
    [SerializeField] Toggle _musicToggle;
    [SerializeField] Toggle _audioToggle;

    [SerializeField] GameObject _settingsUI;
    
    private void Start()
    {
        SetMaxMinValue();//first sets max and min value of sliders

        SetBrightnessSliderValue();
        SetContrastSliderValue();
        SetSaturationSliderValue();
        SetMusicSliderValue();
        SetAudioSliderValue();
        SetMusicToggleValue();
        SetAudioToggleValue();
    }

    private void SetMaxMinValue()
    {
        //sets max value and min value of the slider
        _brightnessSlider.maxValue = 3;
        _brightnessSlider.minValue = -3;

        _contrastSlider.maxValue = 100;
        _contrastSlider.minValue = -100;

        _saturationSlider.maxValue = 100;
        _saturationSlider.minValue = -100;

        _audioSlider.maxValue = 0.5f;
        _audioSlider.minValue = 0;

        _musicSlider.maxValue = 0.5f;
        _musicSlider.minValue = 0;
    }

    #region SliderValue Setters
    //Sets value of the slider according to the previous data
    void SetBrightnessSliderValue() => _brightnessSlider.value = SettingsData.Brightness;
    void SetSaturationSliderValue() => _saturationSlider.value = SettingsData.Saturation;
    void SetContrastSliderValue() => _contrastSlider.value = SettingsData.Contrast;
    void SetAudioSliderValue() => _audioSlider.value = SettingsData.SfxVolume;
    void SetMusicSliderValue() => _musicSlider.value = SettingsData.MusicVolume;

    void SetAudioToggleValue()=> _audioToggle.isOn = SettingsData.AllowSfx;
    void SetMusicToggleValue() => _musicToggle.isOn = SettingsData.AllowMusic;

    #endregion

    #region SetValue from slider
    //Set value according to slider
    public void SetBrightnessFromSliderValue()
    {
        //gets color adjuster to set the color's adjustment
        ColorAdjuster.Instance.SetBrightness(_brightnessSlider.value);
    }
    public void SetSaturationFromSliderValue()
    {
        //gets color adjuster to set the color's adjustment
        ColorAdjuster.Instance.SetSaturation(_saturationSlider.value);
    }
    
    public void SetContrastFromSliderValue()
    {
        //gets color adjuster to set the color's adjustment

        ColorAdjuster.Instance.SetContrast(_contrastSlider.value);
    }
    public void SetAudioFromSliderValue()
    {
        AudioManager.Instance.SetSFXVolume(_audioSlider.value);
    }
    public void SetMusicFromSliderValue()
    {
        AudioManager.Instance.SetMusicVolume(_musicSlider.value);
    }

    public void SetMusicToggle()
    {
        AudioManager.Instance.MuteMusic(!_musicToggle.isOn);
        _musicSlider.gameObject.SetActive(_musicToggle.isOn);
        SettingsData.AllowMusic = _musicToggle.isOn;

    }

    public void SetAudioToggle()
    {
        AudioManager.Instance.MuteAudio(!_audioToggle.isOn);
        _audioSlider.gameObject.SetActive(_audioToggle.isOn);
        SettingsData.AllowSfx = _audioToggle.isOn;

    }
    #endregion

    public void ResetValues()
    {
        //resets the slider value
        _brightnessSlider.value = 0;
        _saturationSlider.value = 0;
        _contrastSlider.value = 0;
        _audioSlider.value = 0.5f;
        _musicSlider.value= 0.5f;
        _musicToggle.isOn = true;
        _audioToggle.isOn = true;
    }

    public void OnSettingCloseButtonClick(bool enable)
    {
        _settingsUI?.SetActive(enable);
    }
}