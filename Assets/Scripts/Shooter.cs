using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour,IInteractableShooter
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public int bulletCapacity = 10;

    private List<GameObject> bulletPool;

    [SerializeField] Transform shooterBase;
    [SerializeField] GameObject cannonLight;

    void Start()
    {
        LoadBullet();
    }

    private void LoadBullet()
    {
        bulletPool = new List<GameObject>(bulletCapacity);
        for (int i = 0; i < bulletCapacity; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    public void CheckInPool()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].activeInHierarchy)
            {
                SummonBullet(i);
                break;
            }
        }
    }

    private void SummonBullet(int bulletIndex)
    {
        bulletPool[bulletIndex].transform.position = bulletSpawnPoint.position;
        bulletPool[bulletIndex].SetActive(true);
        bulletPool[bulletIndex].GetComponent<Rigidbody2D>().velocity = bulletSpawnPoint.right * 20;
    }

    public void Fire()
    {
        CheckInPool();
    }

    public void LookAtPosition(Vector2 targetPosition)
    {
        Vector3 aimDirection = (targetPosition - new Vector2(transform.position.x, transform.position.y));

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        shooterBase.eulerAngles = new Vector3(0, 0, angle);
    }

    public void EnableCannonLights(bool enable)
    {
        cannonLight.SetActive(enable);
    }
}
