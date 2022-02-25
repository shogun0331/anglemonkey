using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System;
using System.Text;
using UnityEngine;


public static class Etc 
{
     static Etc()
    {

    }



    static public void ShuffleArray<T>(T[] array, int iBegin)
    {
        int random1;
        int random2;

        T tmp;

        for (int index = iBegin; index < array.Length; ++index)
        {

            random1 = UnityEngine.Random.Range(iBegin, array.Length);
            random2 = UnityEngine.Random.Range(iBegin, array.Length);

            tmp = array[random1];
            array[random1] = array[random2];
            array[random2] = tmp;

        }
    }

    static public void ShuffleArray<T>(T[] array)
    {
        ShuffleArray(array, 0);
    }

}
