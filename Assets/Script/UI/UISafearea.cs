using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISafearea : MonoBehaviour
{

    int _width = -1, _height = -1;

    public Rect rect;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_width == Screen.width && _height == Screen.height) return;



        _width = Screen.width;
        _height = Screen.height;

        Rect safe = Screen.safeArea;

        rect = safe;


    }
}
