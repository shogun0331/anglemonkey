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
            SoundManager.Instance.Play(SoundManager.SOUND_TRACK.BONUS);
            Game.I.AddScore(transform.position, _score,Game.SCORE_TYPE.TOKKEN);
            GB.ObjectPooling.I.Destroy(gameObject);
        }


    }

}
