using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CameraFollowBasic : MonoBehaviour
{
    #region 인스펙터
    [Header("3인칭 오빗")]
    [SerializeField] private Vector3 _thirdOffset = new Vector3(0f, 2f, -2f);
    [SerializeField] private float _thirdLookAtHeight = 1.5f;
    [Min(0f)]
    [SerializeField] private float _thirdSharpness = 18f;

    [Header("3인칭 오빗 옵션")]
    [SerializeField] private bool _thirdUseOrbit = true;
    [SerializeField] private float _orbitSensitivity = 3.0f;
    [SerializeField] private float _orbitPitchMin = -10.0f;
    [SerializeField] private float _orbitPitchMax = 25.0f;
    #endregion

    #region 내부 변수
    private float _orbitYaw;        // 좌우 회전 (방향 전환)
    private float _orbitPitch;      // 상하 회전 (올려보기 / 내려보기)
    #endregion

    private void InitThird(bool snap)
    {
        _orbitYaw = _target.eulerAngles.y;
        _orbitPitch = 12.0f;

        Vector3 desiredPos;
        Quaternion desiredRot;

        BuildThirdPose(out desiredPos, out desiredRot);

        ApplyPose(desiredPos, desiredRot, _thirdSharpness, snap);
    }

    private void TickThird()
    {
        Vector3 desiredPos;
        Quaternion desiredRot;

        BuildThirdPose(out desiredPos, out desiredRot);

        ApplyPose(desiredPos, desiredRot, _thirdSharpness, false);

        if (_drawDebug)
        {
            // 카메라 정면 확인
            CPrint.Ray(_camTr.position, _camTr.forward * 2.0f, Color.red);
        }
    }

    private void BuildThirdPose(out Vector3 desiredPos, out Quaternion desiredRot)
    {
        if(_thirdUseOrbit && Input.GetMouseButton(1))
        {
            // Yaw 누적
            float mx = Input.GetAxis("Mouse X");
            // Pitch 누적
            float my = Input.GetAxis("Mouse Y");

            _orbitYaw += mx * _orbitSensitivity;
            _orbitPitch -= my * _orbitSensitivity;

            _orbitPitch = Mathf.Clamp(_orbitPitch, _orbitPitchMin, _orbitPitchMax);
        }

        // orbit On : 위치계산
        if (_thirdUseOrbit)
        {
            Quaternion orbitRot = Quaternion.Euler(_orbitPitch, _orbitYaw, 0f);

            desiredPos = _target.position + (orbitRot * _thirdOffset);

            Vector3 lookPos = _target.position + Vector3.up * _thirdLookAtHeight;
            desiredRot = Quaternion.LookRotation(lookPos - desiredPos, Vector3.up);
        }

        // 기본 3인칭
        else
        {
            desiredPos = _target.position + (_target.rotation * _thirdOffset);

            Vector3 lookPos = _target.position + Vector3.up * _thirdLookAtHeight;
            desiredRot = Quaternion.LookRotation(lookPos - desiredPos, Vector3.up);
        }
    }
}
