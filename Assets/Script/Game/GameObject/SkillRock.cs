using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRock : MonoBehaviour
{
    
    public void Play(Transform target)
    {
        GetComponent<SpriteRenderer>().enabled = false;

        StartCoroutine(playAction(target));
    }

    IEnumerator playAction(Transform target)
    {
        const float POWER = 7.0f;
        const float DESTROYTIME = 2.0f;
        const float ANI_DELAY = 0.3f;

        Rigidbody2D rg = GetComponent<Rigidbody2D>();
        rg.isKinematic = true;
        yield return new WaitForSeconds(ANI_DELAY);
        transform.position = target.position;
        GetComponent<SpriteRenderer>().enabled = true;
        rg.isKinematic = false;
        rg.velocity = Vector2.down * POWER;

        yield return new WaitForSeconds(DESTROYTIME);
        rg.isKinematic = false;
        GB.ObjectPooling.I.Destroy(gameObject);
    }


    
}
