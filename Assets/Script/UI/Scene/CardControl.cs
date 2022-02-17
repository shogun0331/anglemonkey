using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class CardControl : MonoBehaviour
{
    //카드 사이즈
    const float CARD_SIZE_X = 200.0f;

    //정렬시 카드 겹치는 갭
    const float OVERLAB_GAP = 0.3f;

    //오른쪽 정렬 
    [SerializeField] bool _sortRight = true;

    List<Card> _cards = new List<Card>();

    private int _targetIdx = -1;





    public void Add(int index)
    {
        GameObject card = loadPoolingObject(Def.PATH_CARD + index, Def.CARD + index);
        card.GetComponent<Card>().Idle();
        card.transform.SetParent(transform);
        _cards.Add(card.GetComponent<Card>());
        sorting(_sortRight);
    }

    public void Delete(int index)
    {
        GB.ObjectPooling.I.Destroy(_cards[index].gameObject);
        _cards.RemoveAt(index);
         sorting(_sortRight);
    }

    public void Use()
    {
        GB.ObjectPooling.I.Destroy(_cards[_targetIdx].gameObject);
        _cards.RemoveAt(_targetIdx);
        sorting(_sortRight);
    }

    public void InitCards()
    {
        for (int i = 0; i < _cards.Count; ++i)
        {
                _cards[i].Idle();
                _cards[i].SetActiveButton(true);
 
        }
    }

    public void Clear()
    {

        for (int i = 0; i < _cards.Count; ++i)
            GB.ObjectPooling.I.Destroy(_cards[i].gameObject);

        _cards.Clear();

    }

    public void ChiseCard(int index)
    {
        if(index < _cards.Count)
        _cards[index].Click();
    }

    

    public void SelectCard(int index)
    {
        _targetIdx = index;
        //선택된 카드애니메이션 _버튼 끄기 , 나머지 반대 
        for (int i = 0; i < _cards.Count; ++i)
        {
            if (i == index)
            {
                _cards[i].Shoot();
                _cards[i].SetActiveButton(false);
            }
            else
            {
                _cards[i].Idle();
                //_cards[i].GetComponent<RectTransform>().SetSiblingIndex (i);
                _cards[i].SetActiveButton(true);
            }
                
        }

        //_cards[index].GetComponent<RectTransform>().SetAsLastSibling();



    }

    public void Show()
    {

    }

    public void Hide()
    {

    }


    private void sorting(bool right)
    {
        for (int i = 0; i < _cards.Count; ++i)
            _cards[i].SetBtnIndex(i);

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
