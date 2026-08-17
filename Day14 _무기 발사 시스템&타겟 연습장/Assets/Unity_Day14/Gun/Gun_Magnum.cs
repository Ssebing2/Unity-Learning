using UnityEngine;

public partial class Gun_Implementation : MonoBehaviour
{
    [Header("Magnum 총알 기본 세팅")]
    [SerializeField] private float _magnumBulletPower = 10000.0f;
    [SerializeField] private float _spawnMagnumForwardOffset = 1f;
    [SerializeField] private float _fireMagnumDelay = 1f;
    [SerializeField] private Vector3 _magnumBulletScale = new Vector3(0.4f, 0.4f, 0.4f);

    [Header("Magnum 반동")]
    [SerializeField] private float _magnumRecoilPower = 2f;

    [Header("재장전")]
    [SerializeField] private int _magnumMagazineSize = 6;
    [SerializeField] private int _magnumcurrentAmmo = 6;
    [SerializeField] private float _reloadTime = 2f;

    [Header("Pool 관련")]
    private Vector2 _moveDirection = Vector2.up;
    private Gun_Implementation _ownerPool;

    private float _nextMagnumFireTime;
    private bool _isMagnumReloading;
    private float _magnumreloadEndTime;

    private void FireMagnumPrefab()
    {
        if (_bulletPrefab == null)
        {
            CPrint.Warn("BulletPrefeb 없음!");
            return ;
        }

        Transform fp = GetFireTransform();

        GameObject magnumBullet = GetBullet();

        Bullet bullet = magnumBullet.GetComponent<Bullet>();

        bullet.SetOwnerPool(this);

        magnumBullet.transform.position = fp.position + fp.forward * _spawnMagnumForwardOffset;
        magnumBullet.transform.rotation = fp.rotation;
        magnumBullet.transform.localScale = _magnumBulletScale;

        Rigidbody rb = magnumBullet.GetComponent<Rigidbody>();
        _magnumcurrentAmmo--;

        if (rb != null )
        {
            rb.velocity = Vector3.zero;

            Vector3 shootForward = fp.forward;
            rb.AddForce(_magnumBulletPower * shootForward);

            if (_playerRb != null)
            {
                _playerRb.AddForce(-fp.forward * _magnumRecoilPower, ForceMode.Impulse);
            }

        }

        else
        {
            CPrint.Warn("총알에 Rigidbody 없음!");
        }

            magnumBullet.transform.localScale = _magnumBulletScale;

    }

    public void SetDirection(Vector2 direction)
    {
        _moveDirection = direction.normalized;
    }

    public void SetOwnerPool(Gun_Implementation pool)
    {
        _ownerPool = pool;
    }

    private void FireMagnum()
    {
        // R키 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            MagnumReload();
        }

        // 재장전 완료 확인
        if (_isMagnumReloading && Time.time >= _magnumreloadEndTime)
        {
            _magnumcurrentAmmo = _magnumMagazineSize;
            _isMagnumReloading = false;

            Debug.Log("재장전 완료");
        }

        // 재장전 중이면 발사 금지
        if (_isMagnumReloading)
        {
            return;
        }

        // 탄 없으면 발사 금지
        if (_magnumcurrentAmmo <= 0)
        {
            Debug.Log("탄약 없음! R키로 재장전");
            return;
        }

        // 여기서부터 발사 입력
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (Time.time < _nextMagnumFireTime)
        {
            return;
        }

        FireMagnumPrefab();

        _nextMagnumFireTime = Time.time + _fireMagnumDelay;
    }

    private void MagnumReload()
    {
        if (_isMagnumReloading)
        {
            return;
        }

        if (_magnumcurrentAmmo >= _magnumMagazineSize)
        {
            return;
        }

        _isMagnumReloading = true;
        _magnumreloadEndTime = Time.time + _reloadTime;
    }

}
