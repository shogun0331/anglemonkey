using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<Brick>() != null)
        {
            if (coll.gameObject.GetComponent<Brick>().IsDestroy) return;
        }

        Rigidbody2D rg = coll.gameObject.GetComponent<Rigidbody2D>();
        if (rg == null)   return;
        float _power = rg.mass * (rg.velocity.magnitude * 0.5f);
        //Action
        rg.velocity = transform.up * _power;
        GetComponent<Animation>().Play("Spring_Pong");



    }


}
