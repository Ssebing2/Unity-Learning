using UnityEngine;

public partial class Gun_Implementation : MonoBehaviour
{
    [Header("Grenade Prefab")]
    [SerializeField] private GameObject _grenadePrefab;

    [Header("GrenadeLauncher 기본 세팅")]
    [SerializeField] private float _grenadeForwardPower = 10f;
    [SerializeField] private float _grenadeUpPower = 5f;
    [SerializeField] private float _fireGrenadeDelay = 1.5f;
    [SerializeField] private float _spawnGrenadeForwardOffset = 1f;

    [SerializeField]
    private Vector3 _grenadeBulletScale = new Vector3(0.1f, 0.1f, 0.1f);

    [Header("GrenadeLauncher 반동")]
    [SerializeField] private float _grenadeLauncherRecoilPower = 2f;

    [Header("재장전")]
    [SerializeField] private int _grenadeLauncherMagazineSize = 5;
    [SerializeField] private int _grenadeLaunchercurrentAmmo = 5;

    private float _nextGrenadeLauncherFireTime;
    private bool _isGrenadeLauncherReloading;
    private float _grenadeLauncherReloadEndTime;


    private void FireGrenadeLauncherPrefab()
    {
        // 유탄 전용 Prefab 확인
        if (_grenadePrefab == null)
        {
            CPrint.Warn("Grenade Prefab 없음!");
            return;
        }

        Transform fp = GetFireTransform();

        // 유탄 생성
        GameObject grenade = Instantiate(_grenadePrefab,fp.position + fp.forward * _spawnGrenadeForwardOffset,fp.rotation);

        // 유탄 크기
        grenade.transform.localScale = _grenadeBulletScale;
        _grenadeLaunchercurrentAmmo--;

        // Rigidbody 가져오기
        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        if (rb == null)
        {
            CPrint.Warn("Grenade에 Rigidbody 없음!");
            Destroy(grenade);
            return;
        }

        // 혹시 모를 기존 물리값 초기화
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 앞으로 가는 힘 + 위로 뜨는 힘
        Vector3 grenadeForce = fp.forward * _grenadeForwardPower + fp.up * _grenadeUpPower;

        // 포물선 발사
        rb.AddForce(grenadeForce,ForceMode.Impulse);

        // 플레이어 반동
        if (_playerRb != null)
        {
            _playerRb.AddForce(-fp.forward * _grenadeLauncherRecoilPower,ForceMode.Impulse
            );
        }
    }


    private void FireGrenadeLauncher()
    {
        // R키 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            GrenadeLauncherReload();
        }

        // 재장전 완료 확인
        if (_isGrenadeLauncherReloading && Time.time >= _grenadeLauncherReloadEndTime)
        {
            _grenadeLaunchercurrentAmmo = _grenadeLauncherMagazineSize;
            _isGrenadeLauncherReloading = false;

            Debug.Log("재장전 완료");
        }

        // 재장전 중이면 발사 금지
        if (_isGrenadeLauncherReloading)
        {
            return;
        }

        // 탄 없으면 발사 금지
        if (_grenadeLaunchercurrentAmmo <= 0)
        {
            Debug.Log("탄약 없음! R키로 재장전");
            return;
        }

        // 좌클릭 순간에만 발사
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        // 발사 딜레이
        if (Time.time < _nextGrenadeLauncherFireTime)
        {
            return;
        }

        FireGrenadeLauncherPrefab();

        // 다음 발사 가능 시간 저장
        _nextGrenadeLauncherFireTime = Time.time + _fireGrenadeDelay;
    }

    private void GrenadeLauncherReload()
    {
        if (_isGrenadeLauncherReloading)
        {
            return;
        }

        if (_grenadeLaunchercurrentAmmo >= _grenadeLauncherMagazineSize)
        {
            return;
        }

        _isGrenadeLauncherReloading = true;
        _grenadeLauncherReloadEndTime = Time.time + _reloadTime;
    }
}
