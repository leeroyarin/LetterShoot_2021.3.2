using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    Coroutine m_Coroutine;
    private void OnEnable()
    {
        m_Coroutine = StartCoroutine(DeactivateBullet(1.5f));

    }
    IEnumerator DeactivateBullet( float timer)
    {

        yield return new WaitForSeconds(timer);
/*       gameObject.GetComponent<TrailRenderer>().enabled = false;
*/        gameObject.GetComponent<TrailRenderer>().Clear();
        gameObject.SetActive(false);

        StopCoroutine(m_Coroutine);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
//        Debug.Log(collision.gameObject.name + " " + Time.time);
        StartCoroutine(DeactivateBullet(0f));
    }


}

