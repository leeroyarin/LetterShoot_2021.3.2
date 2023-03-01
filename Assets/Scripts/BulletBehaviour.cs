using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    Vector2 m_OriginPosition;
    private void OnEnable()
    {
        Invoke("DeactivateBullet", 3);
    }
    void DeactivateBullet()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DeactivateBullet();
    }
}
