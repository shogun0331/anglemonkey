using UnityEngine;

namespace Mobirix.GDPR
{
    public class GdprWithdrawPopup : MonoBehaviour
    {
        [SerializeField]
        private GameObject _firstPage = null;

        [SerializeField]
        private GameObject _secondPage = null;

        private void OnEnable()
        {
            _firstPage.SetActive(true);
            _secondPage.SetActive(false);
        }

        public void setAgree()
        {
            GdprManager.Instance.SetAgree();
            close();
        }

        public void setDisagree()
        {
#if UNITY_ANDROID || UNITY_EDITOR
            GdprManager.Instance.SetDisAgree(true);
#else
            // IOS 경우 종료하지 않는다.
            GdprManager.Instance.SetDisAgree(false);
#endif
        }

        public void viewDetail()
        {
            GdprManager.Instance.GoDetailView();
        }

        // 동의 철회 페이지로 이동
        public void viewSeconePage()
        {
            _firstPage.SetActive(false);
            _secondPage.SetActive(true);
        }


        public void close()
        {
            gameObject.SetActive(false);
        }

    }
}