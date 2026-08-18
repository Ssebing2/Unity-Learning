using UnityEngine;

public partial class Gun_Implementation : MonoBehaviour
{
    [Header("ShotGun 총알 기본 세팅")]
    [SerializeField] private int _shotGunBulletCount = 5;
    [SerializeField] private float _shotGunSpread = 0.5f;
    [SerializeField] private float _shotGunBulletPower = 3000.0f;
    [SerializeField] private float _spawnShotGunForwardOffset = 1f;
    [SerializeField] private float _fireShotGunDelay = 1.5f;
    [SerializeField] private Vector3 _shotGunBulletScale = new Vector3(0.15f, 0.15f, 0.15f);

    [Header("ShotGun 반동")]
    [SerializeField] private float _shotGunRecoilPower = 0.5f;
    
    [Header("재장전")]
    [SerializeField] private int _shotGunMagazineSize = 30;
    [SerializeField] private int _shotGuncurrentAmmo = 30;


    private float _nextShotGunFireTime;
    private bool _isShotGunReloading;
    private float _shotGunreloadEndTime;

    private void FireShotGunPrefab()
    {
        if (_bulletPrefab == null)
        {
            CPrint.Warn("BulletPrefeb 없음!");
            return;
        }

        Transform fp = GetFireTransform();

        for (int i = 0; i < _shotGunBulletCount; i++)
        {
            GameObject shotGunBullet = GetBullet();

            Bullet bullet = shotGunBullet.GetComponent<Bullet>();

            if (bullet == null)
            {
                Debug.LogError("샷건 총알에 Bullet 컴포넌트가 없음!");
                return;
            }

            bullet.SetOwnerPool(this);

            shotGunBullet.transform.position = fp.position + fp.forward * _spawnShotGunForwardOffset;
            shotGunBullet.transform.localScale = _shotGunBulletScale;
            _shotGuncurrentAmmo--;

            Rigidbody rb = shotGunBullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = Vector3.zero;

                float randomX = Random.Range(-1f, 1f);

                Vector3 shootForward = (fp.forward + (fp.right * randomX * _shotGunSpread)).normalized;

                rb.AddForce(shootForward * _shotGunBulletPower);
            }

            if (_playerRb != null)
            {
                _playerRb.AddForce(
                    -fp.forward * _shotGunRecoilPower,
                    ForceMode.Impulse
                );
            }
        }
    }

    private void FireShotGun()
    {
        // R키 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            ShotGunReload();
        }

        // 재장전 완료 확인
        if (_isShotGunReloading && Time.time >= _shotGunreloadEndTime)
        {
            _shotGuncurrentAmmo = _shotGunMagazineSize;
            _isShotGunReloading = false;

            Debug.Log("재장전 완료");
        }

        // 재장전 중이면 발사 금지
        if (_isShotGunReloading)
        {
            return;
        }

        // 탄 없으면 발사 금지
        if (_shotGuncurrentAmmo <= 0)
        {
            Debug.Log("탄약 없음! R키로 재장전");
            return;
        }

        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (Time.time < _nextShotGunFireTime)
        {
            return;
        }

        FireShotGunPrefab();

        _nextShotGunFireTime = Time.time + _fireShotGunDelay;
    }

    private void ShotGunReload()
    {
        if (_isShotGunReloading)
        {
            return;
        }

        if (_shotGuncurrentAmmo >= _shotGunMagazineSize)
        {
            return;
        }

        _isShotGunReloading = true;
        _shotGunreloadEndTime = Time.time + _reloadTime;
    }

}

