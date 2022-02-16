using System.Collections;

using UnityEngine.UI;
using UnityEngine;

public class Loading : MonoBehaviour
{
    [SerializeField] Image _panel = null;
    [SerializeField] Sprite[] _sprPanels = null;

    Color _targetColor;

    float _speed = 1.0f;

    private bool _isAction = false;

    public void Show()
    {
        _panel.color = Color.white;
    }

    public void OpenLoading(float speed)
    {
        if (_isAction) return;
        _isAction = true;
        _speed = speed;
        _targetColor = Color.white;
        _panel.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        int rand = Random.Range(0, _sprPanels.Length);
        _panel.sprite = _sprPanels[rand];
        StartCoroutine(playLoading());
    }

    public void CloseLoading(float speed)
    {
        if (_isAction) return;

        _isAction = true;
        _speed = speed;
        _targetColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        _panel.color = Color.white;
        StartCoroutine(playLoading());

    }

    IEnumerator playLoading()
    {
        float time = 0.0f;

        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);

            time += Time.deltaTime * _speed;
            _panel.color = Color.Lerp(_panel.color, _targetColor, time);

            if (time > 1.0f)
            {
                _isAction = false;
                _panel.color = _targetColor;
                time = 0.0f;
                yield break;
            }

        }
    }

    
}
