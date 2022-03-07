using UnityEngine;
using UnityEngine.UI;

public class TextLocalize : MonoBehaviour
{
    [SerializeField] string _ID;

    void OnEnable()
    {
         GetComponent<Text>().text = LocalizingManager.Instance.GetValue(_ID);
        
    }
    
}
