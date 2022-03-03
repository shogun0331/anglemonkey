using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{

    private void CheckMapScale(float mapWidth)
    {

        mapWidth = 19.2f;
        Camera cam = Camera.main;
        var topRight = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        var topLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));

        float camWidth = Mathf.Abs(topLeft.x) + Mathf.Abs(topRight.x);

        float div = mapWidth / camWidth;
        float tmp = div - 1;
        float sz = cam.orthographicSize;
        cam.orthographicSize = sz + (sz * tmp);

    }

    private void Update()
    {
        //if (_screenWidth == Screen.width && _screenHeight == Screen.height) return;
        CheckMapScale(0);
    }


#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {

        Camera cam = Camera.main;

        var bottomLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        var topLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
        var topRight = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        var bottomRight = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.nearClipPlane));


        Gizmos.color = Color.blue;
        //RU
        Vector2 position = topRight;
        Gizmos.DrawSphere(position, 0.5f);

        //RD
        position = bottomRight;
        Gizmos.DrawSphere(position, 0.5f);

        //LU
        position = topLeft;
        Gizmos.DrawSphere(position, 0.5f);


        //LU
        position = bottomLeft;
        Gizmos.DrawSphere(position, 0.5f);

        //Gizmos.DrawLine(transform.position, new Vector2(LeftX, transform.position.y));

        //Gizmos.DrawLine(transform.position, new Vector2(RightX, transform.position.y));

    }

#endif



}
