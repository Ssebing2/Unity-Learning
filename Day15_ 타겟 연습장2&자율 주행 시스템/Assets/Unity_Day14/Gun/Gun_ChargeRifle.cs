using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Gun_Implementation : MonoBehaviour
{
    [Header("ChargeRifle 총알 기본 세팅")]
    [SerializeField] private float _spawnChargeRifleForwardOffset = 1f;
    [SerializeField] private float _fireChargeRifleDelay = 1f;

    [Header("ChargeRifle 반동")]
    [SerializeField] private float _chargeRifleRecoilPower = 2f;

    [Header("Charge 관련")]
    [SerializeField] private float _maxChargeTime = 3f;
    [SerializeField] private float _chargeRifleBulletMinPower = 1000.0f;
    [SerializeField] private float _chargeRifleBulletMaxPower = 10000.0f;
    [SerializeField] private Vector3 _chargeRifleMinBulletScale = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private Vector3 _chargeRifleMaxBulletScale = new Vector3(0.4f, 0.4f, 0.4f);

    [Header("재장전")]
    [SerializeField] private int _chargeRifleMagazineSize = 10;
    [SerializeField] private int _chargeRiflecurrentAmmo = 10;

    private float _nextChargeRifleFireTime;
    private float _chargeTime;
    private bool _isChargeRifleReloading;
    private float _chargeRifleReloadEndTime;

    private void FireChargeRiflePrefab()
    {
        if (_bulletPrefab == null)
        {
            CPrint.Warn("BulletPrefeb 없음!");
            return;
        }

        Transform fp = GetFireTransform();

        GameObject chargeRifleBullet = GetBullet();

        Bullet bullet = chargeRifleBullet.GetComponent<Bullet>();

        bullet.SetOwnerPool(this);

        chargeRifleBullet.transform.position = fp.position + fp.forward * _spawnChargeRifleForwardOffset;
        chargeRifleBullet.transform.rotation = fp.rotation;
        _chargeRiflecurrentAmmo--;

        Rigidbody rb = chargeRifleBullet.GetComponent<Rigidbody>();

        float chargeRatio = _chargeTime / _maxChargeTime;
        Vector3 bulletScale = Vector3.Lerp(_chargeRifleMinBulletScale, _chargeRifleMaxBulletScale, chargeRatio);
        float bulletPower = Mathf.Lerp(_chargeRifleBulletMinPower, _chargeRifleBulletMaxPower, chargeRatio);

        if (rb != null)
        {
            rb.velocity = Vector3.zero;

            Vector3 shootForward = fp.forward;

            rb.AddForce(bulletPower * shootForward);


            if (_playerRb != null)
            {
                _playerRb.AddForce(-fp.forward * _chargeRifleRecoilPower, ForceMode.Impulse);
            }

        }

        else
        {
            CPrint.Warn("총알에 Rigidbody 없음!");
        }

        chargeRifleBullet.transform.localScale = bulletScale;

    }

    private void FireChargeRifle()
    {
        // R키 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChargeRifleReload();
        }

        // 재장전 완료 확인
        if (_isChargeRifleReloading && Time.time >= _chargeRifleReloadEndTime)
        {
            _chargeRiflecurrentAmmo = _chargeRifleMagazineSize;
            _isChargeRifleReloading = false;

            Debug.Log("재장전 완료");
        }

        // 재장전 중이면 발사 금지
        if (_isChargeRifleReloading)
        {
            return;
        }

        // 탄 없으면 발사 금지
        if (_chargeRiflecurrentAmmo <= 0)
        {
            Debug.Log("탄약 없음! R키로 재장전");
            return;
        }

        if (Input.GetMouseButton(0))
        {
            _chargeTime += Time.deltaTime;

            _chargeTime = Mathf.Clamp(
                _chargeTime,
                0f,
                _maxChargeTime
            );
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Time.time < _nextChargeRifleFireTime)
            {
                return;
            }

            FireChargeRiflePrefab();

            _nextChargeRifleFireTime =
                Time.time + _fireChargeRifleDelay;

            _chargeTime = 0f;
        }
    }
 
    private void ChargeRifleReload()
    {
        if (_isChargeRifleReloading)
        {
            return;
        }

        if (_chargeRiflecurrentAmmo >= _chargeRifleMagazineSize)
        {
            return;
        }

        _isChargeRifleReloading = true;
        _chargeRifleReloadEndTime = Time.time + _reloadTime;
    }

}

