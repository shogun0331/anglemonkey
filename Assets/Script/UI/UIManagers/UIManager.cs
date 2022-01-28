using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    #region Singleton


    private static UIManager _instance = null;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UIManager>();
            }

            if (_instance == null)
            {
                GameObject go = new GameObject("UIManager");
                _instance = go.AddComponent<UIManager>();
            }

            return _instance;
        }
    }

    #endregion

    private Dictionary<string, UIScreen> _screenList = new Dictionary<string, UIScreen>();

    private Stack<UIScreen> _screenStack = new Stack<UIScreen>();
    private Stack<UIScreen> _popupStack = new Stack<UIScreen>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    ///  오브젝트 풀링 된 모든 UI 를 제거
    /// </summary>
    public void clear()
    {
        _screenList.Clear();
        _screenStack.Clear();
        _popupStack.Clear();
    }

    public void changeScene(string sceneName)
    {
        clear();
        SceneManager.LoadScene(sceneName);
    }

    public GameObject findScreen(string name)
    {
        if (_screenList.ContainsKey(name))
            return _screenList[name].gameObject;
        else
            return null;
    }

    public void showScreen(string name, int extraValue = 0)
    {
        
        // 띄워주기 전 이전 UI 를 닫음
        _screenStack.Peek().gameObject.SetActive(false);

        if (_screenList.ContainsKey(name))
        {
            _screenList[name].gameObject.SetActive(true);       // UI 활성화
            _screenStack.Push(_screenList[name]);
        }
        else
        {
            loadFromResources(name, extraValue);
        }
    }

    public void showPopup(string name, int extraValue = 0, bool isSwap = true)
    {

        if (_popupStack.Count > 0)
        {
            if (_popupStack.Peek() == null)
            {
                _popupStack.Clear();
                showPopup(name);
                return;
            }


            if (_popupStack.Peek().name.Equals(name))
            {
                _popupStack.Peek().setExtraValue(extraValue);
                return;
            }

            if (isSwap)
                _popupStack.Peek().gameObject.SetActive(false);
        }

        if (_screenList.ContainsKey(name))
        {
            _screenList[name].gameObject.SetActive(true);   // UI 활성화
            _screenList[name].setExtraValue(extraValue);
            _popupStack.Push(_screenList[name]);
            _screenList[name].GetComponent<RectTransform>().SetAsLastSibling();
        }
        else
        {
            loadFromResources(name, extraValue, true);
        }
    }

    private void loadFromResources(string name, int extraValue, bool isPopup = false)
    {
        const string SCENE_PATH = "Prefab/Scene";
        const string POPUP_PATH = "Prefab/Popup";

        GameObject screen = null;

        if (isPopup)
            screen = Resources.Load<GameObject>(string.Format("{0}/{1}", POPUP_PATH, name));
        else
            screen = Resources.Load<GameObject>(string.Format("{0}/{1}", SCENE_PATH, name));

        // 로드에 실패한 경우 아무것도 하지 않음
        if (screen == null)
        {
            Debug.LogError(string.Format("can not load UI '{0}'", name));
            return;
        }

        screen = Instantiate(screen);
        screen.name = name;

        if (isPopup)
            screen.transform.SetParent(GameObject.Find("UIPopup").transform);
        else
            screen.transform.SetParent(GameObject.Find("UIScreen").transform);

        // reset transform info
        screen.GetComponent<RectTransform>().localScale = Vector3.one;
        screen.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        screen.GetComponent<RectTransform>().offsetMin = Vector2.zero;

        // set extra value
        screen.GetComponent<UIScreen>().setExtraValue(extraValue);

        _screenList.Add(name, screen.GetComponent<UIScreen>());
        if (isPopup)
            _popupStack.Push(screen.GetComponent<UIScreen>());
        else
            _screenStack.Push(screen.GetComponent<UIScreen>());

    }

    public void registScreen(string screenName, UIScreen screen)
    {
        // 이미 등록된 경우 더이상 등록하지 않음
        if (_screenList.ContainsKey(screenName))
            return;

        _screenList.Add(screenName, screen);
        _screenStack.Push(screen);
    }

    public void closeScene()
    {
        UIScreen screen = _screenStack.Pop();
        screen.gameObject.SetActive(false);

        if (_screenStack.Count > 0)
            _screenStack.Peek().gameObject.SetActive(true);
    }

    public void closePopup()
    {
        if (_popupStack.Count <= 0) return;

        UIScreen popup = _popupStack.Pop();
        popup.gameObject.SetActive(false);

        if (_popupStack.Count > 0)
            _popupStack.Peek().gameObject.SetActive(true);
    }

    public void OnBackKey()
    {
        if (_popupStack.Count > 0)
        {
            if (_popupStack.Peek().backkey())
                return;
        }

        if (_screenStack.Count > 0)
        {
            _screenStack.Peek().backkey();
        }
    }

    public bool isAlreadyPopup()
    {
        return _popupStack.Count > 0;
    }

}
