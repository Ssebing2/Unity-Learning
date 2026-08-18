using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Gun_Implementation : MonoBehaviour
{
    [SerializeField] private int _initialSize = 50;

    private readonly Queue<GameObject> pool = new Queue<GameObject>();

    private GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _bulletPoolParent);
        return bullet;
    }

    public GameObject GetBullet()
    {
        if (pool.Count == 0)
        {
            return CreateBullet();
        }

        GameObject bullet = pool.Dequeue();
        bullet.SetActive(true);
        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        pool.Enqueue(bullet);
    }

    public void Spawn(Vector2 position, Vector2 direction)
    {
        GameObject bulletObject = GetBullet();
        bulletObject.transform.position = position;
        bulletObject.transform.rotation = Quaternion.identity;

        Gun_Implementation bullet = bulletObject.GetComponent<Gun_Implementation>();

        bullet.SetOwnerPool(_ownerPool);
        bullet.SetDirection(direction);
    }

}
