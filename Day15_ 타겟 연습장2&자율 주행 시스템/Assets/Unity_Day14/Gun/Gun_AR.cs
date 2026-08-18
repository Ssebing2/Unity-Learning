using UnityEngine;

public partial class Gun_Implementation : MonoBehaviour
{
    [Header("AR 총알 기본 세팅")]
    [SerializeField] private float _arBulletPower = 3500.0f; 
    [SerializeField] private float _spawnArForwardOffset = 0.7f;
    [SerializeField] private float _fireArDelay = 0.3f;
    [SerializeField] private Vector3 _arBulletScale = new Vector3(0.15f, 0.15f, 0.15f);

    [Header("AR 반동")]
    [SerializeField] private float _arRecoilPower = 1f;

    [Header("재장전")]
    [SerializeField] private int _ArMagazineSize = 30;
    [SerializeField] private int _ArcurrentAmmo = 30;

    private float _nextArFireTime;
    private bool _isArReloading;
    private float _arReloadEndTime;

    private void FireArPrefab()
    {
        if (_bulletPrefab == null)
        {
            CPrint.Warn("BulletPrefeb 없음!");
            return;
        }

        Transform fp = GetFireTransform();

        GameObject arnumBullet = GetBullet();

        Bullet bullet = arnumBullet.GetComponent<Bullet>();

        bullet.SetOwnerPool(this);

        arnumBullet.transform.position = fp.position + fp.forward * _spawnArForwardOffset;
        arnumBullet.transform.rotation = fp.rotation;
        arnumBullet.transform.localScale = _arBulletScale;
        _ArcurrentAmmo--;

        Rigidbody rb = arnumBullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;

            Vector3 shootForward = fp.forward;
            rb.AddForce(_arBulletPower * shootForward);

            if (_playerRb != null)
            {
                _playerRb.AddForce(-fp.forward * _arRecoilPower, ForceMode.Impulse);
            }

        }

        else
        {
            CPrint.Warn("총알에 Rigidbody 없음!");
        }

        arnumBullet.transform.localScale = _arBulletScale;

    }

    private void FireAr()
    {
        // R키 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            ArReload();
        }

        // 재장전 완료 확인
        if (_isArReloading && Time.time >= _arReloadEndTime)
        {
            _ArcurrentAmmo = _ArMagazineSize;
            _isArReloading = false;

            Debug.Log("재장전 완료");
        }

        // 재장전 중이면 발사 금지
        if (_isArReloading)
        {
            return;
        }

        // 탄 없으면 발사 금지
        if (_ArcurrentAmmo <= 0)
        {
            Debug.Log("탄약 없음! R키로 재장전");
            return;
        }

        // 여기서부터 발사 입력
        if (!Input.GetMouseButton(0))
        {
            return;
        }

        if (Time.time < _nextArFireTime)
        {
            return;
        }

        FireArPrefab();

        _nextArFireTime = Time.time + _fireArDelay;
    }

    private void ArReload()
    {
        if (_isArReloading)
        {
            return;
        }

        if (_ArcurrentAmmo >= _ArMagazineSize)
        {
            return;
        }

        _isArReloading = true;
        _arReloadEndTime = Time.time + _reloadTime;
    }


}

