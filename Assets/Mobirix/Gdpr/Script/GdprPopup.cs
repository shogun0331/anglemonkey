using UnityEngine;

namespace Mobirix.GDPR
{
    public class GdprPopup : MonoBehaviour
    {
#pragma warning disable 0414
        [SerializeField]
        private GameObject _closeBtn = null;
#pragma warning restore 0414

        void OnEnable()
        {
            // iOS는 닫기 버튼이 없음 
#if UNITY_IOS
            _closeBtn.SetActive(false);
#else
            _closeBtn.SetActive(true);
#endif
        }

        private void Update()
        {
            // 안드로이드 백키 감지를 위해 추가 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                setDisagree();
            }
        }

        public void setAgree()
        {
            GdprManager.Instance.SetAgree();
            UIManager.Instance.changeScene(Def.SCENE_MAIN);
        }

        public void setDisagree()
        {
            // 어플리케이션 종료
            GdprManager.Instance.SetDisAgree(true);
        }

        public void viewDetail()
        {
            GdprManager.Instance.GoDetailView();
        }


    }
}