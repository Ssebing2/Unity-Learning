using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("필수 연결")]
    [SerializeField] private Transform _target;
    [SerializeField] private Camera _camera;

    [Header("디버그")]
    [SerializeField] private bool _drawDebug = true;

    [Header("1인칭")]
    [SerializeField] private Vector3 _firstOffset = new Vector3(0f, 1.6f, 0.1f);
    [Min(0f)]
    [SerializeField] private float _sharpness = 20f;
    [Header("1인칭 옵션")]
    [SerializeField] private bool _firstUseTargetRotation = true;

    private Transform _camTr;
    private bool _snap;

    private void Start()
    {
        // 카메라 가져오기
        if (_camTr == null)
        {
            GameObject mainGamGO = GameObject.FindGameObjectWithTag("MainCamera");

            if (mainGamGO != null)
            {
                _camera = mainGamGO.GetComponent<Camera>();
            }
        }

        if ( _target == null || _camera == null)
        {
            CPrint.Warn("필수 참조 확인");
            enabled = false;
            return;
        }

        // 캐싱
        _camTr = _camera.transform;
    }

    private void Update()
    {
        if (_target == null || _camTr == null)
        {
            return;
        }

        InitFirst();

        if (_drawDebug)
        {
            // 카메라 - 타겟 관계 확인 용도
            CPrint.Line3D(_camTr.position, _target.position, Color.yellow);
        }

    }

    private void LateUpdate()
    {
        TickFirst();
    }


    private void InitFirst()
    {
        Vector3 desiredPos;
        Quaternion desiredRot;

        BuildFirstPose(out desiredPos, out desiredRot);
        ApplyPose(desiredPos, desiredRot, _sharpness, _snap);
    }

    private void TickFirst()
    {
        Vector3 desiredPos;
        Quaternion desiredRot;

        BuildFirstPose(out  desiredPos, out desiredRot);
        ApplyPose(desiredPos, desiredRot, _sharpness, false);
    }

    private void BuildFirstPose(out Vector3 desiredPos, out Quaternion desiredRot)
    {
        desiredPos = _target.position + (_target.rotation * _firstOffset);

        if (_firstUseTargetRotation)
        {
            desiredRot = _target.rotation;
        }

        else
        {
            desiredRot = _camTr.rotation;
        }
    }

    private float GetSmoothT(float sharpness)
    {
        return 1f - Mathf.Exp(-sharpness * Time.deltaTime);
    }

    private void ApplyPose(Vector3 desiredPos, Quaternion desiredRot, float sharpness, bool snap)
    {
        if (_snap)
        {
            _camTr.position = desiredPos;
            _camTr.rotation = desiredRot;

            return;
        }

        float t = GetSmoothT(sharpness);

        _camTr.position = Vector3.Lerp(_camTr.position, desiredPos, t);
        _camTr.rotation = Quaternion.Slerp(_camTr.rotation, desiredRot, t);
    }
}
