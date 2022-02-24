using UnityEngine;
using UnityEngine.UI;

public class ResultPopup : UIScreen
{
    [SerializeField] Text _textTime = null;
    [SerializeField] Text _textScore = null;
    [SerializeField] Text _textMonkey = null;
    [SerializeField] Text _textTotalScore = null;

    public void Init(string time,int total)
    {
        SetTime(time);
        //SetScore(score);
        //SetMonkey(monkeyScore);
        SetTotalScore(total);
    }

    public void SetTime(string time)
    {
        _textTime.text = time;
    }

    public void SetScore(int score)
    {
        _textTime.text = score.ToString("N0");
    }

    public void SetMonkey(int monkey)
    {
        _textMonkey.text = monkey.ToString("N0");
    }

    public void SetTotalScore(int score)
    {
        _textTotalScore.text = score.ToString("N0");
    }

    public void ResetButton()
    {
        Game.I.ResetScene();
    }
}
