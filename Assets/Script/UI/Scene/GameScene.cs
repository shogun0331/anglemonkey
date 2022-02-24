using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : UIScreen
{
    [Header("Score")]
    [SerializeField] Text _textScore = null;

    [Header("InfomationMonkey")]
    [SerializeField] Image _imgMonkeyInfo = null;
    [SerializeField] Sprite[] _sprMonkeyInfos = new Sprite[8];

    [Header("Stage - State")]
    [SerializeField] Image[] _imgStageFrames = null;
    [SerializeField] Transform _stageMonkey = null;

    [SerializeField] Transform _scores = null;


    [SerializeField] CardControl _cardControl = null;

    [SerializeField] Text _textTimer = null;


    [SerializeField] Text[] _textMokeyCnts = null;
    [SerializeField] Button[] _mokeyCards = null;

    private void Awake()
    {
        UIManager.Instance.registScreen(Def.SCENE_GAME, this);
    }




    public void SetTimer(int time)
    {
        System.TimeSpan span = new System.TimeSpan(0, 0, time);
        _textTimer.text = span.ToString(@"mm\:ss");
    }

    public void AddMonkey(int index)
    {
        _cardControl.Add(index);
    }

    public void DeleteMonkey(int index)
    {
        //_cardControl.Delete(index);
    }

    public void UseMonkeyText(int index,int count)
    {
        _textMokeyCnts[index].text = count.ToString();
    }

    public void ActiveMonkey(int index,bool active)
    {
        _mokeyCards[index].interactable = active;
    }
    
    public void InitCards()
    {
        _cardControl.InitCards();
    }



    public void ChiseCard(int index)
    {
        _cardControl.ChiseCard(index);
    }



    public void AddScore(Vector2 position, int score)
    {
        GameObject oj = loadPoolingObject(Def.PATH_EFFECT_SCORE, Def.EFFECT_SCORE);
        oj.GetComponent<Animation>().Play("AddScore");
        oj.GetComponent<TextScore>().SetScore(score);
        oj.transform.SetParent(_scores);
        oj.transform.position = position;

    }
    public void SetScore(int score)
    {
        _textScore.text = score.ToString("N0");
    }

    public void SetMonkeyInfo(int index)
    {
        _imgMonkeyInfo.sprite = _sprMonkeyInfos[index];
    }

    public void SetStage(int index)
    {
        _stageMonkey.position  = _imgStageFrames[index].transform.position;
    }

    private GameObject loadPoolingObject(string path, string key)
    {
        GameObject oj = null;
        if (GB.ObjectPooling.I.GetRemainingUses(key) > 0)
        {
            oj = GB.ObjectPooling.I.Import(key);
        }
        else
        {
            GameObject resources = null;

            if (GB.ObjectPooling.I.CheckModel(key))
            {
                resources = GB.ObjectPooling.I.GetModel(key);
                oj = Instantiate(resources);
            }
            else
            {
                resources = Resources.Load<GameObject>(path);
                GB.ObjectPooling.I.RegistModel(key, resources);
                oj = Instantiate(resources);
            }

            GB.ObjectPooling.I.Registration(key, oj, true);
        }

        return oj;
    }

}
