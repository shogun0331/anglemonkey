using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : UIScreen
{
    [Header("Bgm")]
    [SerializeField] Image _imgBGM = null;
    [SerializeField] Sprite _sprBgmOn;
    [SerializeField] Sprite _sprBgmOff;

    [Header("Effect")]
    [SerializeField] Image _imgEFF = null;

    [SerializeField] Sprite _sprEffOn;
    [SerializeField] Sprite _sprEffOff;

    [Header("Version")]
    [SerializeField] Text _textVersion;

    [SerializeField] RectTransform _panel = null;
    bool _closeAble = false;

    bool _isEffOn = false;
    bool _isBgOn = false;

    void OnEnable()
    {

        _textVersion.text = string.Format("Ver {0}", Application.version);
        StartCoroutine(slideIn());
        _isEffOn = !SoundManager.Instance.IsEFF_Silence;
        _isBgOn = !SoundManager.Instance.IsBGM_Silence;

        reflash();
    }
    
    public void ButtonBgm()
    {
        SoundManager.Instance.Play(SoundManager.SOUND_TRACK.BUTTON);
 
        _isBgOn = !_isBgOn;
        SoundManager.Instance.SetSilence(SoundManager.AUDIO_TYPE.BGM, !_isBgOn);
        reflash();
    }

    public void ButtonEffect()
    {
        SoundManager.Instance.Play(SoundManager.SOUND_TRACK.BUTTON);
        _isEffOn = !_isEffOn;
        SoundManager.Instance.SetSilence(SoundManager.AUDIO_TYPE.EFFECT, !_isEffOn);
        reflash();
    }

    public void ButtonHelp()
    {
        SoundManager.Instance.Play(SoundManager.SOUND_TRACK.BUTTON);

    }

    public void ButtonShare()
    {
        SoundManager.Instance.Play(SoundManager.SOUND_TRACK.BUTTON);

    }

    public void ButtonClose()
    {
        if (!_closeAble)
            return;

        SoundManager.Instance.Play(SoundManager.SOUND_TRACK.BUTTON);
        StartCoroutine(slideOut());

    }



    void reflash()
    {
        _imgBGM.sprite =_isBgOn ? _sprBgmOn : _sprBgmOff;
        _imgEFF.sprite =_isEffOn ? _sprEffOn : _sprEffOff ;
    }



    private IEnumerator slideIn()
    {
        float time = 0;
        const float TOTAL_TIME = 0.2f;
        float percent = 0;

        Vector2 position = _panel.anchoredPosition;

        SoundManager.Instance.Play(SoundManager.SOUND_TRACK.SLIDE);

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

        Vector2 position = _panel.anchoredPosition;
        SoundManager.Instance.Play(SoundManager.SOUND_TRACK.SLIDE);

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
