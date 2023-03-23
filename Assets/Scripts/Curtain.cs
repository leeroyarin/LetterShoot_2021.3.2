using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Curtain : MonoBehaviour
{
    #region SINGLETON
    private static Curtain _curtain;
    public static Curtain CurtainInstance
    {
        get
        {
            if(_curtain == null) _curtain = FindObjectOfType<Curtain>();
            return _curtain;
        }
    }
    #endregion

    [SerializeField] Image image;
    [SerializeField] AnimationCurve curveSpeed;


    private void Awake()
    {
        _curtain = this;
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void ResetCurtains() => StartCoroutine(FadeOutAndIn());

    public void SetPermanentCurtain()
    {
        StopAllCoroutines();
        StartCoroutine(PermanentFadeOut());
    }

    public void SetPermanentCurtain(bool complete)
    {
        StopAllCoroutines();
        StartCoroutine(PermanentFadeOut());
    }

    IEnumerator FadeIn()
    {
        float t = 1f;
        image.gameObject.SetActive(true);
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = curveSpeed.Evaluate(t);
            image.color = new Color(0f, 0f, 0f, a);
            if (a <= 0) image.gameObject.SetActive(false);
            yield return 0;
        }
    }
    IEnumerator FadeOutAndIn()
    {
        float t = 0f;
        image.gameObject.SetActive(true);
        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = curveSpeed.Evaluate(t);
            image.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
        LetterManager.LetterManagerInstance.RemoveAllTrainPartsFromTheScene();
        GameManager.Instance.WaitForWhileAndChangeQuestion();
        yield return new WaitForSeconds(1f);

        StartCoroutine(FadeIn());
    }

 

    IEnumerator PermanentFadeOut()
    {
        float t = 0f;
        image.gameObject.SetActive(true);
        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = curveSpeed.Evaluate(t);
            image.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }


}
