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
     
     ?????? ??? ????????? ????????? ?????? ????????? 
     ios9??? ?????? ?????? ?????? ios9 Application.systemLanguage??? ????????? ??????, ????????? ??????, ????????? ?????? ?????? ???????????? ????????? 
     ????????? ?????? ??? ?????? ?????? ??? ??? ???????????? ????????? 
   
     IOS 7 
     ???????????? zh - ?????? 
     ??????-Hant?????? zh 
     
     IOS 8.1
     
     ????????? ???????????? zh - ?????? ChineseSimplified 
     ????????? ?????? (??????)?????? zh-HK ChineseTraditional 
     ????????? ?????? (??????)?????? zh-Hant ChineseTraditional 
     
 
     
     IOS 9.1 
     
     ????????? ???????????? zh - ??????-CN ????????? 
     ?????? (??????)?????? zh-HK ChineseTraditional 
     ????????? ?????? (??????) ZH ?????? ?????? ????????? TW 
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
        //????????? ??????????????? ??????????????? ?????? ???????????? ???????????? ??????
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
        //             ????????? ?????? = )??? CurIOSLang(;
        //             ??????(name.StartsWith(" ?????? zh - ?????? ")) {
        //                 return SystemLanguage.ChineseSimplified;
        //             }
        //             return SystemLanguage.ChineseTraditional;
        //         }
        //     }

        _sysLanguage = language;

        return _sysLanguage;
    }



}