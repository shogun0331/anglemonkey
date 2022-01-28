using UnityEngine;

public class UIScreen : MonoBehaviour
{

    //	[SerializeField]
    //	private bool _isCallback = true;

    [SerializeField]
    private bool _isPopup = false;

    [SerializeField]
    protected AudioClip _soundButton = null;

    
    public void changeScene(string sceneName)
    {
        UIManager.Instance.changeScene(sceneName);
    }

    virtual public void close()
    {
        //SoundManager.PlaySound(_soundButton);

        if (_isPopup)
            UIManager.Instance.closePopup();
        else
            UIManager.Instance.closeScene();

    }

    /// <summary>
    /// 다른팝업 또는 화면에 백키를 적용 시키지 않는 경우 true 를 return
    /// </summary>
    virtual public bool backkey()
    {
        close();

        return true;
    }

    virtual public void setExtraValue(int extraValue)
    {

    }

    

}
