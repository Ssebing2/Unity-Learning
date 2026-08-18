using UnityEngine;

public partial class Gun_Implementation : MonoBehaviour
{
    [Header("SMG 총알 기본 세팅")]
    [SerializeField] private float _smgBulletPower = 3000.0f;
    [SerializeField] private float _spawnSmgForwardOffset = 0.7f;
    [SerializeField] private float _fireSmgDelay = 0.1f;
    [SerializeField] private Vector3 _smgBulletScale = new Vector3(0.1f, 0.1f, 0.1f);

    [Header("SMG 반동")]
    [SerializeField] private float _smgRecoilPower = 0.7f;

    [Header("재장전")]
    [SerializeField] private int _smgMagazineSize = 35;
    [SerializeField] private int _smgcurrentAmmo = 35;

    private float _nextSmgFireTime;
    private bool _isSmgReloading;
    private float _smgreloadEndTime;



    private void FireSmgPrefab()
    {
        if (_bulletPrefab == null)
        {
            CPrint.Warn("BulletPrefeb 없음!");
            return;
        }

        Transform fp = GetFireTransform();

        GameObject smgnumBullet = GetBullet();

        Bullet bullet = smgnumBullet.GetComponent<Bullet>();

        bullet.SetOwnerPool(this);

        smgnumBullet.transform.position = fp.position + fp.forward * _spawnSmgForwardOffset;
        smgnumBullet.transform.rotation = fp.rotation;
        smgnumBullet.transform.localScale = _smgBulletScale;
        _smgcurrentAmmo--;

        Rigidbody rb = smgnumBullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;

            Vector3 shootForward = fp.forward;
            rb.AddForce(_smgBulletPower * shootForward);

            if (_playerRb != null)
            {
                _playerRb.AddForce(-fp.forward * _smgRecoilPower, ForceMode.Impulse);
            }

        }

        else
        {
            CPrint.Warn("총알에 Rigidbody 없음!");
        }

        smgnumBullet.transform.localScale = _smgBulletScale;

    }


    private void FireSmg()
    {
        // R키 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            SmgReload();
        }

        // 재장전 완료 확인
        if (_isSmgReloading && Time.time >= _smgreloadEndTime)
        {
            _smgcurrentAmmo = _smgMagazineSize;
            _isSmgReloading = false;

            Debug.Log("재장전 완료");
        }

        // 재장전 중이면 발사 금지
        if (_isSmgReloading)
        {
            return;
        }

        // 탄 없으면 발사 금지
        if (_smgcurrentAmmo <= 0)
        {
            Debug.Log("탄약 없음! R키로 재장전");
            return;
        }

        if (!Input.GetMouseButton(0))
        {
            return;
        }

        if (Time.time < _nextSmgFireTime)
        {
            return;
        }


        FireSmgPrefab();

        _nextSmgFireTime = Time.time + _fireSmgDelay;
    }

    private void SmgReload()
    {
        if (_isSmgReloading)
        {
            return;
        }

        if (_smgcurrentAmmo >= _smgMagazineSize)
        {
            return;
        }

        _isSmgReloading = true;
        _smgreloadEndTime = Time.time + _reloadTime;
    }

}
