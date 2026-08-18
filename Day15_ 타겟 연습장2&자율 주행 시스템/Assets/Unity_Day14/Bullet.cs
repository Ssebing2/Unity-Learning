using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _lifeTime = 5f;
    private float _lifeTimer;

    private Gun_Implementation _ownerPool;

    private void OnEnable()
    {
        _lifeTimer = 0f;
    }

    private void Update()
    {
        if (_lifeTimer >= _lifeTime && _ownerPool != null)
        {
            _ownerPool.ReturnBullet(gameObject);
        }

        _lifeTimer += Time.deltaTime;

        if (_lifeTimer >= _lifeTime)
        {
            _ownerPool.ReturnBullet(gameObject);
        }
    }

    public void SetOwnerPool(Gun_Implementation ownerPool)
    {
        _ownerPool = ownerPool;
    }


}
