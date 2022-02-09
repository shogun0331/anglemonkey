using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
        Rigidbody2D rg = coll.gameObject.GetComponent<Rigidbody2D>();
        if (rg == null)   return;
        float _power = rg.mass * (rg.velocity.magnitude * 0.5f);
        //Action
    }


}
