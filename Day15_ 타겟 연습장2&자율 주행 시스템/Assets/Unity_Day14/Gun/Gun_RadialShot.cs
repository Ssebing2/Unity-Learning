using UnityEngine;

public partial class Gun_Implementation : MonoBehaviour
{
    [Header("RadialShot FirePoint")]
    [SerializeField] private Transform _radialShotFirePoin = null;

    [Header("RadialShot 총알 기본 세팅")]
    [SerializeField] private float _radialShotBulletPower = 3000.0f;
    [SerializeField] private float _spawnRadialShotForwardOffset = 1f;
    [SerializeField] private float _fireRadialShotDelay = 0.2f;
    [SerializeField] private Vector3 _radialShotBulletScale = new Vector3(0.15f, 0.15f, 0.15f);

    [Header("RadialShot 반동")]
    [SerializeField] private float _radialShotRecoilPower = 0.5f;

    [Header("재장전")]
    [SerializeField] private int _radialShotMagazineSize = 10;
    [SerializeField] private int _radialShotcurrentAmmo = 10;

    private float _nextRadialShotFireTime;
    private bool _isRadialShotReloading;
    private float _radialShotReloadEndTime;

    private void FireRadialShotPrefab()
    {
        if (_bulletPrefab == null)
        {
            CPrint.Warn("BulletPrefeb 없음!");
            return;
        }

        Transform fp = radialFireTransform();

            GameObject radialShotBullet = GetBullet();

            Bullet bullet = radialShotBullet.GetComponent<Bullet>();

            if (bullet == null)
            {
                Debug.LogError("샷건 총알에 Bullet 컴포넌트가 없음!");
                return;
            }

            bullet.SetOwnerPool(this);

            radialShotBullet.transform.position = fp.position + fp.forward * _spawnRadialShotForwardOffset;
            radialShotBullet.transform.localScale = _radialShotBulletScale;
           _radialShotcurrentAmmo--;

            Rigidbody rb = radialShotBullet.GetComponent<Rigidbody>();

            if (rb != null)
            {

                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                float randomAngle = Random.Range(0f, 360f);

             Vector3 forwardFlat = new Vector3(fp.forward.x,0f,fp.forward.z).normalized;

             Vector3 shootDirection =
                Quaternion.Euler(0f, randomAngle, 0f) * forwardFlat;

            rb.AddForce(shootDirection * _radialShotBulletPower);
            }

            if (_playerRb != null)
            {
                _playerRb.AddForce(
                    -fp.forward * _radialShotRecoilPower,
                    ForceMode.Impulse
                );
            }
    }

    private void FireRadialShot()
    {
        // R키 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            RadialShotReload();
        }

        // 재장전 완료 확인
        if (_isRadialShotReloading && Time.time >= _radialShotReloadEndTime)
        {
            _radialShotcurrentAmmo = _radialShotMagazineSize;
            _isRadialShotReloading = false;

            Debug.Log("재장전 완료");
        }

        // 재장전 중이면 발사 금지
        if (_isRadialShotReloading)
        {
            return;
        }

        // 탄 없으면 발사 금지
        if (_radialShotcurrentAmmo <= 0)
        {
            Debug.Log("탄약 없음! R키로 재장전");
            return;
        }

        if (!Input.GetMouseButton(0))
        {
            return;
        }

        if (Time.time < _nextRadialShotFireTime)
        {
            return;
        }

        FireRadialShotPrefab();

        _nextRadialShotFireTime = Time.time + _fireRadialShotDelay;
    }

    private void RadialShotReload()
    {
        if (_isRadialShotReloading)
        {
            return;
        }

        if (_radialShotcurrentAmmo >= _radialShotMagazineSize)
        {
            return;
        }

        _isRadialShotReloading = true;
        _radialShotReloadEndTime = Time.time + _reloadTime;
    }

    private Transform radialFireTransform() // 연결 안되있으면 본인 몸 기준으로 발사
    {
        return (_radialShotFirePoin != null) ? _radialShotFirePoin : transform;
    }

}
