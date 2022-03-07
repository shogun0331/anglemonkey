using UnityEngine;
using UnityEngine.UI;

public class ResultPopup : UIScreen
{
    [SerializeField] Text _textClearScore = null;
    [SerializeField] Text _textBananaScore = null;
    [SerializeField] Text _textTokkenScore = null;
    [SerializeField] Text _textDestroyScore = null;
    [SerializeField] Text _textMonkeyScore = null;
    [SerializeField] Text _textTimeScore = null;
    [SerializeField] Text _textTotalScore = null;

    public void Init(int[] scores)
    {
        _textClearScore.text = scores[0].ToString("N0");
        _textBananaScore.text = scores[1].ToString("N0");
        _textTokkenScore.text = scores[2].ToString("N0");
        _textDestroyScore.text = scores[3].ToString("N0");
        _textMonkeyScore.text = scores[4].ToString("N0");
        _textTimeScore.text = scores[5].ToString("N0");
        _textTotalScore.text = scores[6].ToString("N0");
    }


    // public void Init(string time,int total)
    // {
    //     SetTime(time);
    //     //SetScore(score);
    //     //SetMonkey(monkeyScore);
    //     SetTotalScore(total);
    // }

    // public void SetTime(string time)
    // {
    //     _textTime.text = time;
    // }

    // public void SetScore(int score)
    // {
    //     _textTime.text = score.ToString("N0");
    // }

    // public void SetMonkey(int monkey)
    // {
    //     _textMonkey.text = monkey.ToString("N0");
    // }

    // public void SetTotalScore(int score)
    // {
    //     _textTotalScore.text = score.ToString("N0");
    // }

    public void ResetButton()
    {
        Game.I.ResetScene();
    }
}
