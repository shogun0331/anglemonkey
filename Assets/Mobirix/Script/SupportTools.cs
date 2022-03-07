using UnityEngine;

#if !UNITY_EDITOR && UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace Mobirix
{
    public class SupportTools : MonoBehaviour
    {
        #region Const
        public const string KEY_UUID = "MBX_UUID";
        #endregion

#if UNITY_EDITOR

        #region National Code

        public static string GetNationalCode()
        {
            return "GR";
        }

        #endregion


        #region Language Code

        public static bool IsKorLan
        {
            get
            {
                return string.Compare(GetLanguageCode(), 0, "ko", 0, 2) == 0;
            }
        }

        public static string GetLanguageCode()
        {
            return "en";
        }



        #endregion


        #region Share

        public static void Share(string title, string packageName)
        {
            Debug.Log("share application");
        }

        #endregion


        #region Zendesk

        public static void Zendesk()
        {
            if (IsKorLan)
            {
                Debug.Log("Open Zendesk KR");
            }
            else
            {
                Debug.Log("Open Zendesk EN");
            }
        }

        #endregion


        #region Facebook Page

        public static void OpenFacebookPage()
        {
            Debug.Log("open Facebook Page");
        }

        #endregion


        #region Youtube Page

        public static void OpenYoutubePage()
        {
            Debug.Log("open youtube page");
        }

        #endregion


        #region UUID

        public static string UUID_LONG()
        {
            string uuidLong = PlayerPrefs.GetString(KEY_UUID);

            if (string.IsNullOrEmpty(uuidLong))
            {
                uuidLong = UUID.M_Create_UUID();
                PlayerPrefs.SetString(KEY_UUID, uuidLong);
                PlayerPrefs.Save();
            }

            return uuidLong;
        }

        public static string UUID_SHORT()
        {
            string uuidShort = UUID_LONG();

            if (uuidShort.Length > 12)
            {
                return uuidShort.Substring(uuidShort.Length - 12);
            }

            return uuidShort;
        }

        #endregion


#elif UNITY_ANDROID

        #region NationalCode

        public static string GetNationalCode()
        {
            string nationalCode = "";

            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getBaseContext"))
            {
                // get TELEPHONY_SERVICE
                string TELEPHONY_SERVICE = "";
                using (AndroidJavaClass activity = new AndroidJavaClass("android.content.Context"))
                {
                    TELEPHONY_SERVICE = activity.GetStatic<string>("TELEPHONY_SERVICE");
                }

                // get national code
                using (AndroidJavaObject telephonyManager = context.Call<AndroidJavaObject>("getSystemService", TELEPHONY_SERVICE))
                {
                    nationalCode = telephonyManager.Call<string>("getNetworkCountryIso");
                }
            }


            return nationalCode;
        }

        #endregion

        
        #region Language Code

        public static bool IsKorLan
        {
            get
            {
                return string.Compare(GetLanguageCode(), 0, "ko", 0, 2) == 0;
            }
        }

        public static string GetLanguageCode()
        {
            string languageCode = "en";

            using (AndroidJavaClass localeClass = new AndroidJavaClass("java.util.Locale"))
            using (AndroidJavaObject defaultLocale = localeClass.CallStatic<AndroidJavaObject>("getDefault"))
            {
                languageCode = defaultLocale.Call<string>("getLanguage");
            }

            return languageCode;
        }



        #endregion

    
        #region Share
        
        public static void Share(string title, string packageName)
        {

            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent"))
            {
                string text = string.Format("https://play.google.com/store/apps/details?id={0}", packageName);

                string ACTION_SEND = intentClass.GetStatic<string>("ACTION_SEND");
                string EXTRA_SUBJECT = intentClass.GetStatic<string>("EXTRA_SUBJECT");
                string EXTRA_TEXT = intentClass.GetStatic<string>("EXTRA_TEXT");

                // 공유 인텐트 설정
                using (AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", ACTION_SEND))
                {
                    intent.Call<AndroidJavaObject>("setType", "text/plain");
                    intent.Call<AndroidJavaObject>("putExtra", EXTRA_SUBJECT, title);
                    intent.Call<AndroidJavaObject>("putExtra", EXTRA_TEXT, text);

                    // 선택 인텐트 생성
                    using (AndroidJavaObject shareIntent = intent.CallStatic<AndroidJavaObject>("createChooser", intent, "Share via"))
                    {
                        currentActivity.Call("startActivity", shareIntent);
                    }
                }
            }
        }

        #endregion
        

        #region Zendesk

        public static void Zendesk()
        {
            if (IsKorLan)
            {
                Application.OpenURL("https://mobirix.zendesk.com/hc/ko");
            }
            else
            {
                Application.OpenURL("https://mobirix.zendesk.com/hc/en-us");
            }
        }

        #endregion
        

        #region Facebook Page

        public static void OpenFacebookPage()
        {
            Application.OpenURL("https://www.facebook.com/mobirixplayen");
        }

        #endregion

        
        #region Youtube Page

        public static void OpenYoutubePage()
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager"))
            using (AndroidJavaObject uriClass = new AndroidJavaClass("android.net.Uri"))
            using (AndroidJavaObject uri = uriClass.CallStatic<AndroidJavaObject>("parse", "https://www.youtube.com/user/mobirix1"))
            using (AndroidJavaObject intentClass = new AndroidJavaClass("android.content.Intent"))
            {
                string ACTION_VIEW = intentClass.GetStatic<string>("ACTION_VIEW");

                // youtube intent 가져오기 
                using (AndroidJavaObject youtubeIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", "com.google.android.youtube"))
                {
                    if (youtubeIntent != null)
                    {
                        try
                        {
                            using (AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", ACTION_VIEW))
                            {
                                intent.Call<AndroidJavaObject>("setPackage", "com.google.android.youtube");
                                intent.Call<AndroidJavaObject>("setData", uri);

                                currentActivity.Call("startActivity", intent);
                            }
                        }
                        catch(System.Exception e)
                        {
                            using (AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", ACTION_VIEW, uri))
                            {
                                currentActivity.Call("startActivity", intent);
                            }
                        }
                    }
                    else
                    {
                        using (AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", ACTION_VIEW, uri))
                        {
                            currentActivity.Call("startActivity", intent);
                        }
                    }
                }
            }
        }

        #endregion
        

        #region UUID

        public static string UUID_LONG()
        {
            string uuidLong = PlayerPrefs.GetString(KEY_UUID);

            if (string.IsNullOrEmpty(uuidLong))
            {
                uuidLong = UUID.M_Create_UUID();
                PlayerPrefs.SetString(KEY_UUID, uuidLong);
                PlayerPrefs.Save();
            }

            return uuidLong;
        }

        public static string UUID_SHORT()
        {
            string uuidShort = UUID_LONG();

            if (uuidShort.Length > 12)
            {
                return uuidShort.Substring(uuidShort.Length - 12);
            }

            return uuidShort;
        }

        #endregion


#elif UNITY_IOS
        

        #region National Code
        
        
        [DllImport ("__Internal")]
        private static extern string _sb_support_getNationalCode();
        public static string GetNationalCode()
        {
            string nationalCode = "none";

            string receiveNationalCode = _sb_support_getNationalCode();
            if(!string.IsNullOrEmpty(receiveNationalCode))
                nationalCode = receiveNationalCode;

            return nationalCode;
        }

        #endregion

                       
        #region Language Code

        public static bool IsKorLan
        {
            get
            {
                return string.Compare(GetLanguageCode(), 0, "ko", 0, 2) == 0;
            }
        }

        [DllImport ("__Internal")]
        private static extern string _sb_support_getLanguageCode();
        public static string GetLanguageCode()
        {
            string languageCode = "en";

            string receiveLanguageCode = _sb_support_getLanguageCode();
            if(!string.IsNullOrEmpty(receiveLanguageCode))
                languageCode = receiveLanguageCode;

            return languageCode;
        }


        #endregion

        
        #region Share
        
        public static void Share(string title, string packageName)
        {
            Debug.Log("share application");
        }

        #endregion
       
        
        #region Zendesk

        public static void Zendesk()
        {
            if (IsKorLan)
            {
                Application.OpenURL("https://mobirix.zendesk.com/hc/ko");
            }
            else
            {
                Application.OpenURL("https://mobirix.zendesk.com/hc/en-us");
            }
        }

        #endregion

        
        #region Facebook Page

        public static void OpenFacebookPage()
        {
            Application.OpenURL("https://www.facebook.com/mobirixplayen");
        }

        #endregion

        
        #region Youtube Page
        
        [DllImport ("__Internal")]
        private static extern void _sb_support_openYoutubePage();
        public static void OpenYoutubePage()
        {
            _sb_support_openYoutubePage();
        }


        #endregion
        

        #region UUID

        public static string UUID_LONG()
        {
            string uuidLong = PlayerPrefs.GetString(KEY_UUID);

            if (string.IsNullOrEmpty(uuidLong))
            {
                uuidLong = UUID.M_Create_UUID();
                PlayerPrefs.SetString(KEY_UUID, uuidLong);
                PlayerPrefs.Save();
            }

            return uuidLong;
        }

        public static string UUID_SHORT()
        {
            string uuidShort = UUID_LONG();

            if (uuidShort.Length > 12)
            {
                return uuidShort.Substring(uuidShort.Length - 12);
            }

            return uuidShort;
        }

        #endregion

#endif

    }
}