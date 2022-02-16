using UnityEngine;
using UnityEngine.UI;

public class TextScore : MonoBehaviour
{
    [SerializeField] Text _text = null;
    
    public void SetScore(int score)
    {
        _text.text = score.ToString("N0");
        
    }

   
}
