using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : UIScreen
{
    [Header("Bgm")]
    [SerializeField] Image _imgBGM = null;
    [SerializeField] Sprite _sprBgmOn;
    [SerializeField] Sprite _sprBgmOff;

    [Header("Effect")]
    [SerializeField] Image _imgEFF = null;
    [SerializeField] Sprite _sprEffOn;
    [SerializeField] Sprite _sprEffOff;


    [SerializeField] RectTransform _panel = null;
    bool _closeAble = false;
    

    void OnEnable()
    {
        Time.timeScale = 0.0f;

        reflash();
        StartCoroutine(slideIn());
    }

    void reflash()
    {
        _imgBGM.sprite = SoundManager.Instance.IsBGM_Silence ? _sprBgmOff : _sprBgmOn;
        _imgEFF.sprite = SoundManager.Instance.IsEFF_Silence ? _sprEffOff : _sprEffOn;

    }

    void OnDisable()
    {
        Time.timeScale = 1.0f;
    }
    public void ButtonBgm()
    {
        SoundManager.Instance.SetSilence(SoundManager.AUDIO_TYPE.BGM, !SoundManager.Instance.IsBGM_Silence);
        reflash();
    }

    public void ButtonEffect()
    {
        SoundManager.Instance.SetSilence(SoundManager.AUDIO_TYPE.EFFECT, !SoundManager.Instance.IsEFF_Silence);
        reflash();
    }


    public void ButtonHowToPlay()
    {
        SoundManager.Instance.Play(SoundManager.SOUND_TRACK.BUTTON);

    }

    public void ButtonExit()
    {
        SoundManager.Instance.Play(SoundManager.SOUND_TRACK.BUTTON);

    }

    public void ButtonContinue()
    {
        if (!_closeAble)
            return;

        SoundManager.Instance.Play(SoundManager.SOUND_TRACK.BUTTON);
        StartCoroutine(slideOut());
    }

    private IEnumerator slideIn()
    {
        float time = 0;
        const float TOTAL_TIME = 0.2f;
        float percent = 0;
        SoundManager.Instance.Play(SoundManager.SOUND_TRACK.SLIDE);
        Vector2 position = _panel.anchoredPosition;

        while (time <= TOTAL_TIME)
        {
            time += Time.unscaledDeltaTime;

            percent = time / TOTAL_TIME;
            if (percent > 1)
                percent = 1;

            position.x = Mathf.Lerp(_panel.rect.width, 0, percent);

            _panel.anchoredPosition = position;

            yield return new WaitForEndOfFrame();

        }

        // 슬라이드 액션이 끝남 
        _closeAble = true;

        yield break;
    }



    private IEnumerator slideOut()
    {
        float time = 0;
        const float TOTAL_TIME = 0.2f;
        float percent = 0;
        SoundManager.Instance.Play(SoundManager.SOUND_TRACK.SLIDE);
        Vector2 position = _panel.anchoredPosition;

        while (time <= TOTAL_TIME)
        {
            time += Time.unscaledDeltaTime;

            percent = time / TOTAL_TIME;
            if (percent > 1)
                percent = 1;

            position.x = Mathf.Lerp(0, _panel.rect.width, percent);

            _panel.anchoredPosition = position;

            yield return new WaitForEndOfFrame();
        }

        _closeAble = false;

        close();

        yield break;
    }



}
