using UnityEngine;

public partial class Gun_Implementation : MonoBehaviour
{
    [Header("BurstFireRifle 총알 기본 세팅")]
    [SerializeField] private int _burstFireRifleBulletCount = 3;
    [SerializeField] private float _burstFireRifleBulletPower = 3000.0f;
    [SerializeField] private float _spawnBurstFireRifleForwardOffset = 1f;
    [SerializeField] private float _fireBurstFireRifleDelay = 0.3f;
    [SerializeField] private Vector3 _burstFireRifleBulletScale = new Vector3(0.15f, 0.15f, 0.15f);

    [Header("BurstFireRifle 반동")]
    [SerializeField] private float _burstFireRifleRecoilPower = 2f;

    [Header("재장전")]
    [SerializeField] private int _burstFireRifleMagazineSize = 30;
    [SerializeField] private int _burstFireRiflecurrentAmmo = 30;

    private float _nextBurstFireRifleFireTime;
    private bool _isBurstFireRifleReloading;
    private float _burstFireRifleReloadEndTime;

    private void FireBurstFireRiflePrefab()
    {

        if (_bulletPrefab == null)
        {
            CPrint.Warn("BulletPrefeb 없음!");
            return;
        }

        Transform fp = GetFireTransform();

        for (int i = 0; i < _burstFireRifleBulletCount; i++)
        {
            GameObject burstFireRifleBullet = GetBullet();

            Bullet bullet = burstFireRifleBullet.GetComponent<Bullet>();

            if (bullet == null)
            {
                Debug.LogError("3점사 라이플 총알에 Bullet 컴포넌트가 없음!");
                return;
            }

            bullet.SetOwnerPool(this);

            burstFireRifleBullet.transform.position = fp.position + fp.forward * _spawnBurstFireRifleForwardOffset;
            burstFireRifleBullet.transform.localScale = _burstFireRifleBulletScale;
            _burstFireRiflecurrentAmmo--;

            Rigidbody rb = burstFireRifleBullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = Vector3.zero;

                Vector3 shootForward = fp.forward.normalized;

                rb.AddForce(shootForward * _burstFireRifleBulletPower);
            }

            if (_playerRb != null)
            {
                _playerRb.AddForce(
                    -fp.forward * _burstFireRifleRecoilPower,
                    ForceMode.Impulse
                );
            }
        }
    }

    private void FireBurstFireRifle()
    {
        // R키 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            BurstFireRifleReload();
        }

        // 재장전 완료 확인
        if (_isBurstFireRifleReloading && Time.time >= _burstFireRifleReloadEndTime)
        {
            _burstFireRiflecurrentAmmo = _burstFireRifleMagazineSize;
            _isBurstFireRifleReloading = false;

            Debug.Log("재장전 완료");
        }

        // 재장전 중이면 발사 금지
        if (_isBurstFireRifleReloading)
        {
            return;
        }

        // 탄 없으면 발사 금지
        if (_burstFireRiflecurrentAmmo <= 0)
        {
            Debug.Log("탄약 없음! R키로 재장전");
            return;
        }

        if (!Input.GetMouseButton(0))
        {
            return;
        }

        if (Time.time < _nextBurstFireRifleFireTime)
        {
            return;
        }

        FireBurstFireRiflePrefab();

        _nextBurstFireRifleFireTime = Time.time + _fireBurstFireRifleDelay;
    }

    private void BurstFireRifleReload()
    {
        if (_isBurstFireRifleReloading)
        {
            return;
        }

        if (_burstFireRiflecurrentAmmo >= _burstFireRifleMagazineSize)
        {
            return;
        }

        _isBurstFireRifleReloading = true;
        _burstFireRifleReloadEndTime = Time.time + _reloadTime;
    }

}