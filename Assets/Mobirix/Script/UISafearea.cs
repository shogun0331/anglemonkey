using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

#if !UNITY_EDITOR && UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace Mobirix
{
    public class UISafearea : MonoBehaviour
    {

        [SerializeField]
        private CanvasScaler _canvasScaler = null;

        [SerializeField]
        private RectTransform _panel = null;

        [SerializeField]
        private RectTransform[] _bg;

        private static Vector2 _defaultResolution = Vector2.zero;
        private static Vector2 _scaledResolution = Vector2.zero;
        private static Vector2 _shiftValue = new Vector2(-1, -1);

        // 마지막 적용 해상도 
        private static Vector2 _lastResolution = Vector2.zero;

        /// <summary>
        /// Scaler Resolution 이 변경되었을경우 호출
        /// </summary>
        public static void resetDefaultResolution()
        {
            _defaultResolution = Vector2.zero;
        }

        private Vector2 _orgPanelOffsetMin;
        private Vector2 _orgPanelOffsetMax;

        private Vector2[] _orgBgOffsetMin;
        private Vector2[] _orgBgOffsetMax;

        private bool _init = false;




        /// <summary>
        /// 스케일 되기 전  Canvas의 Reference Resolution
        /// </summary>
        /// <value>The default reference resolution.</value>
        public static Vector2 defaultReferenceResolution
        {
            get
            {
                return _defaultResolution;
            }
        }

        /// <summary>
        /// 스케일 된 후의 Canvas의 Reference Resolution
        /// </summary>
        /// <value>The scaled resolution.</value>
        public static Vector2 scaledResolution
        {
            get
            {
                CanvasScaler canvasScaler = GameObject.FindObjectOfType<CanvasScaler>();

                if (canvasScaler == null)
                    return _scaledResolution;

                if (_scaledResolution.Equals(Vector2.zero))
                {
                    // 실제 화면 해상도를 가져옴
                    float width = Screen.width;
                    float height = Screen.height;

                    // 비워야되는 safearea 배율 
                    float marginX = screenScale.x;
                    float marginY = screenScale.y;

                    // 스케일 해야하는 실제 배율 
                    float scaleX = 1 / marginX;
                    float scaleY = 1 / marginY;

                    // 스케일 해야하는 값 중 큰 값으로 사용해야 함 
                    float scale = scaleX > scaleY ? scaleX : scaleY;

                    // 캔버스 해상도 
                    if (_defaultResolution.Equals(Vector2.zero))
                        _defaultResolution = canvasScaler.referenceResolution;
                    Vector2 designedResolution = _defaultResolution;

                    // 캔버스 해상도 비율 및 실제 화면 비율 계산
                    float designedRatio = designedResolution.y / designedResolution.x;
                    float currentRatio = Screen.height / (float)Screen.width;

                    // 스케일 되는 크기 계산 
                    float scaleRes = 1;
                    _scaledResolution = designedResolution;

                    // 실제회면이 가로 또는 세로로 큰 경우의 처리 
                    if (currentRatio > designedRatio)
                    {   // 세로로 더 긴 경우 
                        scaleRes = currentRatio / designedRatio;

                        _scaledResolution.y = designedResolution.y * scaleRes * scale;
                    }
                    else
                    {   // 가로로 더 긴 경우
                        scaleRes = designedRatio / currentRatio;

                        _scaledResolution.x = designedResolution.x * scaleRes * scale;
                    }
                }

                return _scaledResolution;
            }
        }

        /// <summary>
        /// 비워야하는 상,하  좌,우의 값 
        /// </summary>
        /// <value>The shift value.</value>
        public static Vector2 shiftValue
        {
            get
            {
                if (_shiftValue.Equals(new Vector2(-1, -1)))
                {
                    // 비워야되는 safearea 배율 
                    float marginX = screenScale.x;
                    float marginY = screenScale.y;

                    // 각 앵커의 위치 이동값을 계산 
                    _shiftValue = scaledResolution;
                    _shiftValue.x = (_shiftValue.x * 0.5f) * (1 - marginX);
                    _shiftValue.y = (_shiftValue.y * 0.5f) * (1 - marginY);
                }

                return _shiftValue;
            }
        }

        public void updateScaleCanvas()
        {
            if(_init==false)
            {
                //아직 초기화 되지 않았다면 Awake에서 자동 업데이트
                return;
            }
            // 해상도 재 계산을 위해 리셋 
            _scaledResolution = Vector2.zero;

            _lastResolution.x = Screen.width;
            _lastResolution.y = Screen.height;

            setScaleCanvas();
        }

        void Awake()
        {
            if(_panel!=null)
            {
                _orgPanelOffsetMin = _panel.offsetMin;
                _orgPanelOffsetMax = _panel.offsetMax;
            }
            if(_bg!=null&& _bg.Length>0)
            {
                _orgBgOffsetMin = new Vector2[_bg.Length];
                _orgBgOffsetMax = new Vector2[_bg.Length];

                for (int i = 0; i < _bg.Length; i++)
                {
                    _orgBgOffsetMin[i] = _bg[i].offsetMin;
                    _orgBgOffsetMax[i] = _bg[i].offsetMax;
                }
            }
            _init = true;

            updateScaleCanvas();
        }

        private void Update()
        {
            // 해상도가 변경 된 경우 스케일 재 설정 
            if (_lastResolution.x != Screen.width || _lastResolution.y != Screen.height)
            {
                updateScaleCanvas();
            }
        }

        private void setScaleCanvas()
        {
            // 지정한 캔버스 스케일러가 없는 경우 게임오브젝트 중 하나를 찾음 
            if (_canvasScaler == null)
                _canvasScaler = GameObject.FindObjectOfType<CanvasScaler>();

            // 캔버스 스케일러가 없는경우 스케일 할 수 없음 
            if (_canvasScaler == null)
                return;

            // 스케일 펙터 조절을 위해 캔버스 해상도 변경
            if (!scaledResolution.Equals(_canvasScaler.referenceResolution))
            {
                if (!scaledResolution.Equals(Vector2.zero))
                    _canvasScaler.referenceResolution = scaledResolution;

            }

            // shiftValue 계산이 안된경우 또는 -1인경우 작업하지 않음
            if (shiftValue.Equals(new Vector2(-1, -1)))
                return;

            if (_panel != null)
            {
                Vector2 offsetMinSelf = _orgPanelOffsetMin;
                offsetMinSelf.x += shiftValue.x;
                offsetMinSelf.y += shiftValue.y;

                Vector2 offsetMaxSelf = _orgPanelOffsetMax;
                offsetMaxSelf.x -= shiftValue.x;
                offsetMaxSelf.y -= shiftValue.y;

                _panel.offsetMin = offsetMinSelf;
                _panel.offsetMax = offsetMaxSelf;
            }

            if (_bg.Length > 0)
            {
                for (int i = 0; i < _bg.Length; ++i)
                {
                    if (_bg[i] != null)
                    {
                        Vector2 offsetMin = _orgBgOffsetMin[i];
                        offsetMin.x -= shiftValue.x;
                        offsetMin.y -= shiftValue.y;

                        _bg[i].offsetMin = offsetMin;

                        Vector2 offsetMax = _orgBgOffsetMax[i];
                        offsetMax.x += shiftValue.x;
                        offsetMax.y += shiftValue.y;

                        _bg[i].offsetMax = offsetMax;
                    }
                }

            }
        }

#if !UNITY_EDITOR && UNITY_IOS
        [DllImport ("__Internal")]
        private static extern string _sb_safearea_getScreenScale();
        public static string getScreenScale()
        {
            return _sb_safearea_getScreenScale();
        }

#else

        public static string getScreenScale()
        {
            return "1.0|1.0";
        }
#endif


        public static Vector2 getScreenScaleVector()
        {
            Vector2 scale = Vector2.one;

            string scaleData = getScreenScale();

            string[] splitData = scaleData.Split('|');

            if (splitData.Length >= 2)
            {
                float scaleX = float.Parse(splitData[0]);
                float scaleY = float.Parse(splitData[1]);

                if (scaleX != 0)
                    scale.x = scaleX;


                if (scaleY != 0)
                    scale.y = scaleY;
            }


            return scale;
        }

        /// <summary>
        /// Safearea 처리를 위한 스크린 스케일 값
        /// </summary>
        private static Vector2 _screenScale = Vector2.zero;
        public static Vector2 screenScale
        {
            get
            {
                if (_screenScale == Vector2.zero)
                    _screenScale = getScreenScaleVector();

                if (_screenScale == Vector2.zero)
                    return Vector2.one;

                return _screenScale;
            }
            set
            {
                _screenScale = value;
            }
        }
    }
}