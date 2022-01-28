using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoroyObejct : MonoBehaviour
{
    public float EndTime = 2.0f;
    float time = 0.0f;


    private void Update()
    {

        time += Time.deltaTime;

        if (time > EndTime)
        {
            time = 0.0f;


            GB.ObjectPooling.I.Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }


        
    }

}
