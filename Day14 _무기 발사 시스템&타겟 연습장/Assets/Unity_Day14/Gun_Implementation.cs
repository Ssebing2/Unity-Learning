using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class Gun_Implementation : MonoBehaviour
{
    private enum EGunType
    {
        Magnum,
        AR,
        SMG,
        ShotGun,
        BurstFireRifle,
        DoubleBarrel,
        RadialShot,
        ChargeRifle,
        GrenadeLauncher
    }

    #region 인스펙터
    [Header("필수 연결")]
    [SerializeField] private GameObject _bulletPrefab = null;
    [SerializeField] private Transform _firePoint = null;

    [Header("탄 색깔")]
    [SerializeField] private Color _magnumColor = Color.red;
    [SerializeField] private Color _arColor = Color.yellow;
    [SerializeField] private Color _smgColor = Color.green;
    [SerializeField] private Color _burstFireRifleColor = Color.blue;
    [SerializeField] private Color _doubleBarrelColor = Color.cyan;
    [SerializeField] private Color _radialShotColor = Color.gray;
    [SerializeField] private Color _chargeRifleColor = Color.white;
    [SerializeField] private Color _grenadeLauncherColor = Color.black;

    [Header("토글 키")]
    [SerializeField] private KeyCode _magnumKey = KeyCode.Alpha1;
    [SerializeField] private KeyCode _arKey = KeyCode.Alpha2;
    [SerializeField] private KeyCode _smgKey = KeyCode.Alpha3;
    [SerializeField] private KeyCode _shotGunKey = KeyCode.Alpha4;
    [SerializeField] private KeyCode _burstFireRifleKey = KeyCode.Alpha5;
    [SerializeField] private KeyCode _doubleBarrelKey = KeyCode.Alpha6;
    [SerializeField] private KeyCode _radialShotKey = KeyCode.Alpha7;
    [SerializeField] private KeyCode _chargeRifleKey = KeyCode.Alpha8;
    [SerializeField] private KeyCode _grenadeLauncherKey = KeyCode.Alpha9;

    [Header("풀 보관공간")]
    [SerializeField] private Transform _bulletPoolParent;

    #endregion



    #region 변수
    private Renderer _gunColor;
    private Rigidbody _playerRb;
    private EGunType _mode;
    private Vector3 _firePointOriginalPosition;
    private Quaternion _firePointOriginalRotation;
    #endregion

    private void Awake()
    {
        _gunColor = GetComponent<Renderer>();
        _playerRb = GetComponent<Rigidbody>();

        if (_bulletPrefab == null)
        {
            CPrint.Warn("총알 프리팹이 비어있습니다. 인스펙터 확인");

            enabled = false;
            return;
        }

        if (_firePoint == null)
        {
            CPrint.Warn("총알 소환위치가 비어있습니다. 인스펙터 확인");

            enabled = false;
            return;
        }

        for (int i = 0; i < _initialSize; i++)
        {
            GameObject bullet = CreateBullet();
            bullet.SetActive(false);
            pool.Enqueue(bullet);
        }

    }

    private Transform GetFireTransform() // 연결 안되있으면 본인 몸 기준으로 발사
    {
        return (_firePoint != null) ? _firePoint : transform;
    }

    private void Start()
    {
        CPrint.Title("총 구현");

        _firePointOriginalPosition = _firePoint.localPosition;
        _firePointOriginalRotation = _firePoint.localRotation;
    }

    private void Update()
    {

        if (Input.GetKeyDown(_magnumKey)) // 매그넘 총알발사
        {
            SetMode(EGunType.Magnum);
        }

        if (Input.GetKeyDown(_arKey)) // AR
        {
            SetMode(EGunType.AR);
        }

        if (Input.GetKeyDown(_smgKey)) // SMG
        {
            SetMode(EGunType.SMG);
        }

        if (Input.GetKeyDown(_shotGunKey)) // 샷건
        {
            SetMode(EGunType.ShotGun);
        }

        if (Input.GetKeyDown(_burstFireRifleKey)) // 3점사 총
        {
            SetMode(EGunType.BurstFireRifle);
        }

        if (Input.GetKeyDown(_doubleBarrelKey)) // 듀얼샷
        {
            SetMode(EGunType.DoubleBarrel);
        }

        if (Input.GetKeyDown(_radialShotKey)) // 원형 발사
        {
            SetMode(EGunType.RadialShot);
        }

        if (Input.GetKeyDown(_chargeRifleKey)) // 차지 라이플
        {
            SetMode(EGunType.ChargeRifle);
        }

        if (Input.GetKeyDown(_grenadeLauncherKey)) // 유탄총
        {
            SetMode(EGunType.GrenadeLauncher);
        }

        Fire();
    }

    private void SetMode(EGunType mode)
    {
        _mode = mode;

        CPrint.Section($"모드 : {_mode}");
        Debug.Log($"모드 : {_mode}");

    }

    private void Fire()
    {
        switch (_mode)
        {
            case EGunType.Magnum:
                FireMagnum();
                break;
            case EGunType.AR:
                FireAr();
                break;
            case EGunType.SMG:
                FireSmg();
                break;
            case EGunType.ShotGun:
                FireShotGun();
                break;
            case EGunType.BurstFireRifle:
                FireBurstFireRifle();
                break;
            case EGunType.DoubleBarrel:
                FireDoubbleBarrel();
                break;
            case EGunType.RadialShot:
                FireRadialShot();
                break;
            case EGunType.ChargeRifle:
                FireChargeRifle();
                break;
            case EGunType.GrenadeLauncher:
                FireGrenadeLauncher();
                break;
        }
    }

    public string GetCurrentGunName()
    {
        return _mode.ToString();
    }

    public int GetCurrentAmmo()
    {
        switch (_mode)
        {
            case EGunType.Magnum:
                return _magnumcurrentAmmo;

            case EGunType.AR:
                return _ArcurrentAmmo;

            case EGunType.SMG:
                return _smgcurrentAmmo;

            case EGunType.ShotGun:
                return _shotGuncurrentAmmo;

            case EGunType.BurstFireRifle:
                return _burstFireRiflecurrentAmmo;

            case EGunType.DoubleBarrel:
                return _doubbleBarrelcurrentAmmo;

            case EGunType.RadialShot:
                return _radialShotcurrentAmmo;

            case EGunType.ChargeRifle:
                return _chargeRiflecurrentAmmo;

            case EGunType.GrenadeLauncher:
                return _grenadeLaunchercurrentAmmo;
        }

        return 0;
    }

    public int GetMagazineSize()
    {
        switch (_mode)
        {
            case EGunType.Magnum:
                return _magnumMagazineSize;

            case EGunType.AR:
                return _ArMagazineSize;

            case EGunType.SMG:
                return _smgMagazineSize;

            case EGunType.ShotGun:
                return _shotGunMagazineSize;

            case EGunType.BurstFireRifle:
                return _burstFireRifleMagazineSize;

            case EGunType.DoubleBarrel:
                return _doubbleBarrelMagazineSize;

            case EGunType.RadialShot:
                return _radialShotMagazineSize;

            case EGunType.ChargeRifle:
                return _chargeRifleMagazineSize;

            case EGunType.GrenadeLauncher:
                return _grenadeLauncherMagazineSize;
        }

        return 0;
    }

    private void OnGUI()
    {
        GUI.color = new Color(0f, 0f, 0f, 0.8f);
        GUI.Box(new Rect(10, 10, 300, 100), "");

        GUI.color = Color.white;
        GUI.Label(
            new Rect(20, 20, 300, 30),
            $"현재 무기 : {GetCurrentGunName()}"
        );

        GUI.Label(
            new Rect(20, 40, 300, 30),
            $"탄약 : {GetCurrentAmmo()} / {GetMagazineSize()}"
        );

        GUI.Label(
            new Rect(20, 60, 300, 30),
            "사격 : Mouse Left"
        );

        GUI.Label(
            new Rect(20, 80, 300, 30),
            "재장전 : R"
        );
    }
}

