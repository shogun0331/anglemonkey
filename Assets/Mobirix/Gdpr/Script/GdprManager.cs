using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_IOS

using System.Runtime.InteropServices;

#endif

namespace Mobirix.GDPR
{
    public class GdprManager : MonoBehaviour
    {
        private static GdprManager _instance = null;
        public static GdprManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "GdprManager";
                    _instance = obj.AddComponent<GdprManager>();
                    _instance.Init();
                    DontDestroyOnLoad(obj);
                }
                return _instance;
            }
        }


        private static string[] eu_country = { "AT", "BE", "BG", "CY", "CZ", "DE", "DK", "EE", "ES", "FI", "FR", "GB", "GR", "HR", "HU", "IE", "IT", "LT", "LU", "LV", "MT", "NL", "PL", "PT", "RO", "SE", "SI", "SK", "IS", "LI", "NO" };

        private const string GDPR_URL_AOS = "http://gdprinfo.mobirix.net:33364/GdprServer/aosdesc.html";
        private const string GDPR_URL_IOS = "http://gdprinfo.mobirix.net:33364/GdprServer/iosdesc.html";

        private const string GDPR_SCENE = "GdprScene";

        #region Public Variable
        /// <summary>
        /// EU 국가 인가?
        /// </summary>
        public bool IsEu { get; private set; }
        #endregion

        #region Private Variable
        private bool isNoCountryCode;
        private Action onAgree;
        private Action onDisagree;
        #endregion

        #region Public Func
        /// <summary>
        /// 동의
        /// </summary>
        public void SetAgree()
        {
            setGdprAgree(true);
            setGdprCheckOn(false);
            if(onAgree!=null)
            {
                onAgree.Invoke();
            }
            clearAction();
        }
        /// <summary>
        /// 동의 철회
        /// </summary>
        public void SetDisAgree(bool quit)
        {
            setGdprAgree(false);
            setGdprCheckOn(true);
            if (onDisagree != null)
            {
                onDisagree.Invoke();
            }
            clearAction();
            if (quit)
            {
                Application.Quit();
            }
        }
        /// <summary>
        /// 입장시 동의 팝업을 띄워야 하는가?
        /// </summary>
        public bool CheckGdprOn()
        {
            if (getGdprCheckOn()) // 기존에 gdpr동의가 되어 있지 않은경우나 철회 했을 경우
            {
                if (isNoCountryCode)
                {
                    setNoEu();

                    return false;
                }
                else if (IsEu)
                {
                    return true;
                }
                else
                {
                    setNoEu();

                    return false;
                }
            }
            else
            {
                //기존에 gdpr체크했다.

                return false;
            }
        }

        public void LoadGdprScene()
        {
            SceneManager.LoadScene(GDPR_SCENE);
        }

        /// <summary>
        /// GDPR 동의 씬으로 이동
        /// </summary>
        /// <param name="onAgree"></param>
        /// <param name="onDisagree"></param>
        public void LoadGdprScene(Action onAgree,Action onDisagree)
        {
            this.onAgree = onAgree;
            this.onDisagree = onDisagree;
            SceneManager.LoadScene(GDPR_SCENE);
        }
        public void OpenGdprWithdrawPopup(GdprWithdrawPopup popup, Action onAgree, Action onDisagree)
        {
            this.onAgree = onAgree;
            this.onDisagree = onDisagree;
            popup.gameObject.SetActive(true);
        }
        public void OpenGdprWithdrawPopup(GdprWithdrawPopup popup)
        {
            OpenGdprWithdrawPopup(popup, null, null);
        }


        /// <summary>
        /// 약관 보기
        /// </summary>
        public void GoDetailView()
        {
#if UNITY_IOS
            gdpr_openUrl(GDPR_URL_IOS);
#else
            gdpr_openUrl(GDPR_URL_AOS);
#endif
        }
        #endregion
        #region Private Func
        private void Init()
        {
            IsEu = false;
            isNoCountryCode = false;

            //string saved_country = getCountry();
            string device_country = SupportTools.GetNationalCode();

            setCountry(device_country);

            IsEu = IsEuCountry();

            if (string.Equals(device_country, "null") || string.Equals(device_country, ""))
            {
                //  CountryCode is NULL

                isNoCountryCode = true;
            }
            else
            {
                isNoCountryCode = false;
            }
        }

        private void setNoEu()
        {
            setGdprAgree(false);
            setGdprCheckOn(false);
        }

        private void gdpr_openUrl(string url)
        {
            Application.OpenURL(url);
        }
        private void clearAction()
        {
            this.onAgree = null;
            this.onDisagree = null;
        }

        #endregion

        #region Country
        private bool IsEuCountry()
        {
#if GDPR_TEST
            return true;
#else
            string gdpr_country = getCountry();

            for (int i = 0; i < eu_country.Length; i++)
            {
                if (string.Compare(gdpr_country, 0, eu_country[i], 0, 2, true) == 0)
                    return true;

            }
            return false;
#endif
        }

        private void setCountry(string value)
        {
            PlayerPrefs.SetString("GDPR_COUNTRY", value);
            PlayerPrefs.Save();
        }

        private string getCountry()
        {
            return PlayerPrefs.GetString("GDPR_COUNTRY", "");
        }
#endregion
#region GDPR Check
        /// <summary>
        /// GDPR를 띄워줘야하는가?
        /// </summary>
        private bool getGdprCheckOn()
        {
            int isCheckOn = PlayerPrefs.GetInt("GDPR_CHECK_ON", 1);

            if (isCheckOn != 0)
                return true;

            return false;
        }

        private void setGdprCheckOn(bool val)
        {
            if (val)
            {
                PlayerPrefs.SetInt("GDPR_CHECK_ON", 1);
            }
            else
            {
                PlayerPrefs.SetInt("GDPR_CHECK_ON", 0);
            }
            PlayerPrefs.Save();
        }
#endregion
#region GDPR Agree
        private bool getGdprAgree()
        {
            int isAgree = PlayerPrefs.GetInt("GDPR_AGREE", 0);

            if (isAgree != 0)
                return true;

            return false;
        }
        private void setGdprAgree(bool val)
        {
            if (val)
            {
                PlayerPrefs.SetInt("GDPR_AGREE", 1);
            }
            else
            {
                PlayerPrefs.SetInt("GDPR_AGREE", 0);
            }
            PlayerPrefs.Save();
        }
#endregion



    }
}