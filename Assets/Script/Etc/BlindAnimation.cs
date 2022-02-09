using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindAnimation : MonoBehaviour
{

    bool _isAction = false;
    bool _isOpen = false;
    Vector3 _targetScale = Vector3.one;


    public void Open()
    {
        if (_isAction) return;
        
        _isAction = true;
        _isOpen = true;
        _targetScale.x = 1.0f;
        StartCoroutine(playAction());
    }

   

    public void Close()
    {
        if (_isAction) return;
        _isAction = true;
        _isOpen = false;
        _targetScale.x = 0.0f;
        StartCoroutine(playAction());


    }

    IEnumerator playAction()
    {
        float time = 0.0f;
        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);

            time += Time.deltaTime;
            transform.localScale = Vector3.MoveTowards(transform.localScale, _targetScale, Time.deltaTime);


            if (time > 1.0f)
            {
                _isAction = false;

                yield break;
            }
         
        }
    }







}
