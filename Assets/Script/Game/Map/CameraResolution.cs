using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    //const float SIZE_X = 2.055f;
    //const float SIZE_Y = 1.0f;



    private void Awake()
    {

        
    }


    public void SetSize(Vector2 size)
    {

        float screenX = Screen.width / Screen.height;
        //9.6
        float x = size.x / screenX;

        Camera.main.orthographicSize = x;

        //4.97 
    }



    //public Vector2 GetSize()
    //{


    //    Camera cam = GetComponent<Camera>();
    //    float x = cam.orthographicSize * SIZE_X;
    //    float y = cam.orthographicSize * SIZE_Y;

    //    return new Vector2(x, y);

    //}


#if UNITY_EDITOR



    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.blue;
        //Gizmos.DrawLine(transform.position, new Vector2(LeftX, transform.position.y));

        //Gizmos.DrawLine(transform.position, new Vector2(RightX, transform.position.y));

    }

#endif



}
