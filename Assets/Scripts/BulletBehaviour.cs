using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform _hookSpawnPointTransform;
    [SerializeField] Rigidbody2D _hookRigidBody;
    Coroutine m_Coroutine;

   
    IEnumerator DeactivateBullet(float timer)
    {
        lineRenderer.positionCount =2;
        float timePassed = 0;
        lineRenderer.enabled = true;
        while (timePassed > timer)
        {
            print("beingCalled");
            lineRenderer.SetPosition(1, _hookSpawnPointTransform.position);
            lineRenderer.SetPosition(0, this.transform.position);
            timePassed+= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (Vector2.Distance(_hookSpawnPointTransform.transform.position, this.transform.position) >= 0.1)
        {
            this.transform.Translate(_hookSpawnPointTransform.transform.position.normalized*20f);
            lineRenderer.SetPosition(1, _hookSpawnPointTransform.position);
            lineRenderer.SetPosition(0, this.transform.position);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        lineRenderer.enabled = false;   
        StopAllCoroutines();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    { 
        //if the hook gets collided with the shooter then the bullet stops moving 
        if (collision.gameObject.CompareTag("Shooter"))
        {
            StopCoroutine(m_Coroutine);
            return;
        }

        //else wise the coroutine of returning the hook is activated after stopping the currently running coroutine
        StopAllCoroutines();
        m_Coroutine = StartCoroutine(DeactivateBullet(0f));
    }

    //if the coroutine is currently working then the function returns true
    public bool CheckIfBulletIsLaunched() => m_Coroutine != null;


    public void LauchHook( float p_firingPower)
    {
        this.transform.position = _hookSpawnPointTransform.transform.position;
        this.gameObject.SetActive(true);
        this._hookRigidBody.velocity = _hookSpawnPointTransform.right * p_firingPower;

        m_Coroutine = StartCoroutine(DeactivateBullet(1.5f));

    }
}

