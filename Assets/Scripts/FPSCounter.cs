using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _fpsText;

    private float _deltaTime;
    private void Awake()
    {
//        PlayerPrefs.DeleteAll();
    }
    private void Update()
    {
        // Update deltaTime
        _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;

        // Calculate FPS
        float fps = 1.0f / _deltaTime;

        // Update text
        _fpsText.text = "FPS: " + Mathf.RoundToInt(fps).ToString();
    }
}
