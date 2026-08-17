using UnityEngine;

public partial class Gun_Implementation : MonoBehaviour
{
    [Header("DoubbleBarrel 총알 기본 세팅")]
    [SerializeField] private int _doubbleBarrelBulletCount = 2;
    [SerializeField] private float _doubbleBarrelSpread = 0.5f;
    [SerializeField] private float _doubbleBarrelBulletPower = 3000.0f;
    [SerializeField] private float _spawnDoubbleBarrelForwardOffset = 1f;
    [SerializeField] private float _fireDoubbleBarrelDelay = 1.5f;
    [SerializeField] private Vector3 _doubbleBarrelBulletScale = new Vector3(0.15f, 0.15f, 0.15f);


    [Header("DoubbleBarrel 반동")]
    [SerializeField] private float _doubbleBarrelRecoilPower = 0.5f;

    [Header("재장전")]
    [SerializeField] private int _doubbleBarrelMagazineSize = 10;
    [SerializeField] private int _doubbleBarrelcurrentAmmo = 10;

    private float _nextDoubbleBarrelFireTime;
    private bool _isDoubbleBarrelReloading;
    private float _doubbleBarrelReloadEndTime;

    private void FireDoubbleBarrelPrefab()
    {
        if (_bulletPrefab == null)
        {
            CPrint.Warn("BulletPrefeb 없음!");
            return;
        }

        Transform fp = GetFireTransform();

        for (int i = 0; i < _doubbleBarrelBulletCount; i++)
        {
            GameObject doubbleBarrelBullet = GetBullet();

            Bullet bullet = doubbleBarrelBullet.GetComponent<Bullet>();

            if (bullet == null)
            {
                Debug.LogError("샷건 총알에 Bullet 컴포넌트가 없음!");
                return;
            }

            bullet.SetOwnerPool(this);

            if (i == 0)
            {
                doubbleBarrelBullet.transform.position =
                    fp.position + fp.forward * _spawnDoubbleBarrelForwardOffset + fp.right * 0.2f;
            }
            else
            {
                doubbleBarrelBullet.transform.position =
                    fp.position + fp.forward * _spawnDoubbleBarrelForwardOffset - fp.right * 0.2f;
            }

            doubbleBarrelBullet.transform.localScale = _doubbleBarrelBulletScale;
            _doubbleBarrelcurrentAmmo--;

            Rigidbody rb = doubbleBarrelBullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = Vector3.zero;

                Vector3 shootForward;

                if (i == 0)
                {
                    shootForward = (fp.forward + fp.right * _doubbleBarrelSpread).normalized;
                }

                else
                {
                    shootForward = (fp.forward - fp.right * _doubbleBarrelSpread).normalized;
                }

                rb.AddForce(shootForward * _doubbleBarrelBulletPower);
            }

            if (_playerRb != null)
            {
                _playerRb.AddForce(
                    -fp.forward * _doubbleBarrelRecoilPower,
                    ForceMode.Impulse
                );
            }
        }
    }

    private void FireDoubbleBarrel()
    {
        // R키 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            DoubbleBarrelReload();
        }

        // 재장전 완료 확인
        if (_isDoubbleBarrelReloading && Time.time >= _doubbleBarrelReloadEndTime)
        {
            _doubbleBarrelcurrentAmmo = _doubbleBarrelMagazineSize;
            _isDoubbleBarrelReloading = false;

            Debug.Log("재장전 완료");
        }

        // 재장전 중이면 발사 금지
        if (_isDoubbleBarrelReloading)
        {
            return;
        }

        // 탄 없으면 발사 금지
        if (_doubbleBarrelcurrentAmmo <= 0)
        {
            Debug.Log("탄약 없음! R키로 재장전");
            return;
        }

        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (Time.time < _nextDoubbleBarrelFireTime)
        {
            return;
        }

        FireDoubbleBarrelPrefab();

        _nextDoubbleBarrelFireTime = Time.time + _fireDoubbleBarrelDelay;
    }

    private void DoubbleBarrelReload()
    {
        if (_isDoubbleBarrelReloading)
        {
            return;
        }

        if (_doubbleBarrelcurrentAmmo >= _doubbleBarrelMagazineSize)
        {
            return;
        }

        _isDoubbleBarrelReloading = true;
        _doubbleBarrelReloadEndTime = Time.time + _reloadTime;
    }

}
