using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Text_Json
{
    [System.Serializable]
    public class TextModel
    {
        public string ID;
        public string Korean;
        public string English;
        public string Japanese;
        public string ChineseSimplified;
        public string ChineseTraditional;
        public string German;
        public string Spanish;
        public string French;
        public string Vietnamese;
        public string Thai;
        public string Russian;
        public string Italian;
        public string Portuguese;
        public string Turkish;
        public string Indonesian;

    }
    public TextModel[] Data;
    

}

public class LocalizingManager : MonoBehaviour
{
    [SerializeField] TextAsset _jsonFile = null;
    [SerializeField] SystemLanguage _sysLanguage;

    [SerializeField] Text_Json _data;

    Dictionary<string, Dictionary<SystemLanguage, string>> _dicData = new Dictionary<string, Dictionary<SystemLanguage, string>>();


    #region Singleton
    public static LocalizingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LocalizingManager>();
                if (_instance == null)
                    _instance = new GameObject("LocalizingManager").AddComponent<LocalizingManager>();
            }

            if (_instance._jsonFile == null)
            {
                _instance._jsonFile = Resources.Load<TextAsset>("LocalizeText/LocalizeMonkey");
                _instance.PaserData();
                _instance.getSystemLanguage();
            }

            return _instance;

        }

    }

    static LocalizingManager _instance = null;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else if (_instance != this) Destroy(gameObject);

        if (_instance._jsonFile == null)
            _instance._jsonFile = Resources.Load<TextAsset>("LocalizeText/LocalizeMonkey");

        _instance.PaserData();


        DontDestroyOnLoad(this.gameObject);
    }

    #endregion
    /*
     
     보정 후 시스템 언어를 얻을 수있다 
     ios9는 모든 중국 획득 ios9 Application.systemLanguage의 시스템 언어, 중국어 간체, 중국어 번체 값을 조정하기 때문에 
     중국어 간체 및 번체 구별 할 수 없습니다 중국어 
   
     IOS 7 
     간체에서 zh - 한스 
     전통-Hant에서 zh 
     
     IOS 8.1
     
     중국어 간체에서 zh - 한스 ChineseSimplified 
     중국어 번체 (홍콩)에서 zh-HK ChineseTraditional 
     중국어 번체 (대만)에서 zh-Hant ChineseTraditional 
     
 
     
     IOS 9.1 
     
     중국어 간체에서 zh - 한스-CN 중국어 
     번체 (홍콩)에서 zh-HK ChineseTraditional 
     중국어 번체 (대만) ZH 버전 다름 중국어 TW 
    */

    public void PaserData()
    {
        _data = JsonUtility.FromJson<Text_Json>(_jsonFile.text);
        _dicData.Clear();

        for (int i = 0; i < _data.Data.Length; ++i)
        {
            Dictionary<SystemLanguage, string> dicLanguage = new Dictionary<SystemLanguage, string>();
            dicLanguage.Add(SystemLanguage.Korean, _data.Data[i].Korean);
            dicLanguage.Add(SystemLanguage.English, _data.Data[i].English);
            dicLanguage.Add(SystemLanguage.Japanese, _data.Data[i].Japanese);
            dicLanguage.Add(SystemLanguage.ChineseSimplified, _data.Data[i].ChineseSimplified);
            dicLanguage.Add(SystemLanguage.ChineseTraditional, _data.Data[i].ChineseTraditional);
            dicLanguage.Add(SystemLanguage.German, _data.Data[i].German);
            dicLanguage.Add(SystemLanguage.Spanish, _data.Data[i].Spanish);
            dicLanguage.Add(SystemLanguage.French, _data.Data[i].French);
            dicLanguage.Add(SystemLanguage.Vietnamese, _data.Data[i].Vietnamese);
            dicLanguage.Add(SystemLanguage.Thai, _data.Data[i].Thai);
            dicLanguage.Add(SystemLanguage.Russian, _data.Data[i].Russian);
            dicLanguage.Add(SystemLanguage.Italian, _data.Data[i].Italian);
            dicLanguage.Add(SystemLanguage.Portuguese, _data.Data[i].Portuguese);
            dicLanguage.Add(SystemLanguage.Turkish, _data.Data[i].Turkish);
            dicLanguage.Add(SystemLanguage.Indonesian, _data.Data[i].Indonesian);
            
            if (_dicData.ContainsKey(_data.Data[i].ID))
                Debug.LogWarning("SameKey : " + _data.Data[i].ID);

            _dicData.Add(_data.Data[i].ID, dicLanguage);
        }
    }

    public string GetValue(string id)
    {
        //유니티 에디터이면 내마음대로 언어 선택해서 사용하기 위함
#if !UNITY_EDITOR
        getSystemLanguage();
#endif

        if (!_dicData.ContainsKey(id))
            return string.Empty;
        if (!_dicData[id].ContainsKey(_sysLanguage))
            _sysLanguage = SystemLanguage.English;
            

        return _dicData[id][_sysLanguage];
    }



    private SystemLanguage getSystemLanguage()
    {
        SystemLanguage language = Application.systemLanguage;

        //     if(Application.platform == RuntimePlatform.IPhonePlayer)
        //  {
        //         if(language == SystemLanguage.Chinese)
        //          {
        //             문자열 이름 = )를 CurIOSLang(;
        //             경우(name.StartsWith(" 에서 zh - 한스 ")) {
        //                 return SystemLanguage.ChineseSimplified;
        //             }
        //             return SystemLanguage.ChineseTraditional;
        //         }
        //     }

        _sysLanguage = language;

        return _sysLanguage;
    }



}