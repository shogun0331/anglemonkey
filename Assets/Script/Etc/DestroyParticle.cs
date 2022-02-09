using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    public float DestroyDelay = 1.0f;
    float _time = 0.0f;

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time > DestroyDelay)
        {
            _time = 0.0f;
            GB.ObjectPooling.I.Destroy(this.gameObject);
        }
        
    }


}
