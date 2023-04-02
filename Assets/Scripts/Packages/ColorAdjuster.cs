using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorAdjuster : MonoBehaviour
{
    private static ColorAdjuster _colorAdjuster;
    public static ColorAdjuster Instance
    {
        get
        {
            if (_colorAdjuster == null)
            {
                _colorAdjuster = FindObjectOfType<ColorAdjuster>();
                if (_colorAdjuster == null) Debug.Log("The ColorAdjuster Component is not available in the scene");
            }
            return _colorAdjuster;
        }
    }

    [SerializeField] Volume globalVolume;
    public ColorAdjustments colorAdjustments;

    private void Awake()
    {

        if(_colorAdjuster==null)
        {
            _colorAdjuster = this;
        }
        else
        {
            Destroy(this);
        }
        globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        SetColorAdjustments();
    }

    private void SetColorAdjustments()
    {
        colorAdjustments.contrast.value = SettingsData.Contrast;
        colorAdjustments.saturation.value = SettingsData.Brightness;
        colorAdjustments.postExposure.value = SettingsData.Brightness;
    }

    public void SetBrightness(float value)
    {
        SettingsData.Brightness = value;
        Instance.colorAdjustments.postExposure.value = value;
    }

    public void SetContrast(float value)
    {
        if (Instance == null) print("Null");
        if (colorAdjustments == null) print("Null2");
        Instance.colorAdjustments.contrast.value = value;
        SettingsData.Contrast = value;
    }
    public void SetSaturation(float value)
    {
        Instance.colorAdjustments.saturation.value = value;
        SettingsData.Saturation = value;
    }
}
