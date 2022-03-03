using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayPopup : UIScreen
{

    [Header("Buttons")]
    [SerializeField] Image[] _imgBtns = null;
    [SerializeField] Sprite[] _sprBtnOns = null;
    [SerializeField] Sprite[] _sprBtnOffs = null;


    int _pageIndex = 0;
    int _monkeyIndex = -1;

    [Header("Infomation Sprite Array")]
    [SerializeField] Sprite[] _sprMonkey1 = null;
    [SerializeField] Sprite[] _sprMonkey2 = null;
    [SerializeField] Sprite[] _sprMonkey3 = null;
    [SerializeField] Sprite[] _sprMonkey4 = null;
    [SerializeField] Sprite[] _sprMonkey5 = null;
    [SerializeField] Sprite[] _sprMonkey6 = null;
    [SerializeField] Sprite[] _sprMonkey7 = null;
    [SerializeField] Sprite[] _sprMonkey8 = null;


    [Header("Infomation")]
    [SerializeField] Image _imgInfomation = null;
    [SerializeField] Text _textPageIndex = null;

    Dictionary<int, Sprite[]> _dicMonkeyInfo = new Dictionary<int, Sprite[]>();

    bool _isInit = false;

    private void OnEnable()
    {
        _monkeyIndex = -1;
        init();
        changeMonkey(0);

    }

    private void init()
    {
        if (_isInit) return;

        _dicMonkeyInfo.Clear();
        //{ Baby = 0,Gold, Papio, Macaca, Lemur, Nasalis, Hylobatidae, Gorilla }
        _dicMonkeyInfo.Add((int)Def.Monkey.Baby, _sprMonkey1);
        _dicMonkeyInfo.Add((int)Def.Monkey.Gold, _sprMonkey2);
        _dicMonkeyInfo.Add((int)Def.Monkey.Papio, _sprMonkey3);
        _dicMonkeyInfo.Add((int)Def.Monkey.Macaca, _sprMonkey4);
        _dicMonkeyInfo.Add((int)Def.Monkey.Lemur, _sprMonkey5);
        _dicMonkeyInfo.Add((int)Def.Monkey.Nasalis, _sprMonkey6);
        _dicMonkeyInfo.Add((int)Def.Monkey.Hylobatidae, _sprMonkey7);
        _dicMonkeyInfo.Add((int)Def.Monkey.Gorilla, _sprMonkey8);

        _isInit = true;


    }


    public void ClickMonkeyIndex(int index)
    {
        changeMonkey(index);
    }


    public void ClickNextPage()
    {
        if (_pageIndex + 1 >= _dicMonkeyInfo[_monkeyIndex].Length) return;
        _pageIndex++;
        _imgInfomation.sprite = _dicMonkeyInfo[_monkeyIndex][_pageIndex];
        _textPageIndex.text = string.Format("{0} / {1}", _pageIndex + 1, _dicMonkeyInfo[_monkeyIndex].Length);

    }

    public void ClickPrevPage()
    {
        if (_pageIndex - 1 < 0) return;
        _pageIndex--;
        _imgInfomation.sprite = _dicMonkeyInfo[_monkeyIndex][_pageIndex];
        _textPageIndex.text = string.Format("{0} / {1}", _pageIndex + 1, _dicMonkeyInfo[_monkeyIndex].Length);
    }


    private void changeMonkey(int index)
    {
        if (_monkeyIndex == index) return;

        _monkeyIndex = index;
        _pageIndex = 0;

        for (int i = 0; i < _imgBtns.Length; ++i)
        {
            if (i == index)
                _imgBtns[i].sprite = _sprBtnOns[i];
            else
                _imgBtns[i].sprite = _sprBtnOffs[i];
        }

        _imgInfomation.sprite= _dicMonkeyInfo[_monkeyIndex][_pageIndex];

        _textPageIndex.text = string.Format("{0} / {1}", 1, _dicMonkeyInfo[_monkeyIndex].Length);

    }




}
