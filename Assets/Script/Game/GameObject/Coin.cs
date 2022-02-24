using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int _score;

    private void OnTriggerEnter2D(Collider2D coll)
    {



        if (string.Equals(coll.gameObject.tag, "Monkey"))
        {
            Game.I.AddScore(transform.position, _score);
            GB.ObjectPooling.I.Destroy(gameObject);
        }


    }

}
