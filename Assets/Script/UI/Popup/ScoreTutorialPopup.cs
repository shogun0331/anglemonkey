using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTutorialPopup : UIScreen
{
    [SerializeField] GameObject[] _pages = null;

    int _pageIndex = -1;

    public void SetPage(int index)
    {
        _pageIndex = -1;
        changePage(index);
    }

    private void changePage(int index)
    {

        if (_pageIndex == index) return;
        _pageIndex = index;

        for (int i = 0; i < _pages.Length; ++i)
        {
            if (i == index)
                _pages[i].SetActive(true);
            else
                _pages[i].SetActive(false);
        }
        
    }

    public void ButtonNextPage()
    {
        if (_pageIndex + 1 >= _pages.Length) return;
        
        changePage(_pageIndex + 1);

    }

    public void ButtonPrevPage()
    {
        if (_pageIndex - 1 < 0) return;

        changePage(_pageIndex - 1);

    }



}
