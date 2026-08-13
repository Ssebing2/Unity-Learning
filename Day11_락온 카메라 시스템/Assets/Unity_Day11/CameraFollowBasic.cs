using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class CameraFollowBasic : MonoBehaviour
{
    public enum ECameraMode
    {
        ThirdPerson,
        FirstPerson,
        QuarterView,
        LockOnView
    }

    #region 인스펙터
    [Header("필수 연결")]
    [SerializeField] private Transform _target;
    [SerializeField] private Camera _camera;

    [Header("시점별 토글 키")]
    [SerializeField] private KeyCode _KeyThird = KeyCode.Alpha1;
    [SerializeField] private KeyCode _KeyFirst = KeyCode.Alpha2;
    [SerializeField] private KeyCode _KeyQuarter = KeyCode.Alpha3;
    [SerializeField] private KeyCode _KeyLockOn = KeyCode.L;

    [Header("시작 모드")]
    [SerializeField] private ECameraMode _startmode = ECameraMode.ThirdPerson;
    [SerializeField] private bool _snapOnModeChange = true;

    [Header("디버그")]
    [SerializeField] private bool _drawDebug = true;
    #endregion

    #region 내부변수
    private Transform _camTr;
    private ECameraMode _mode;
    #endregion

    private void Start()
    {
        if (_camera == null)
        {
            GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

            if (mainCamera != null)
            {
                _camera = mainCamera.GetComponent<Camera>();
            }

            if (_target == null || _camera == null)
            {
                CPrint.Warn("필수 연결 확인");
                enabled = false;
                return;
            }

            _camTr = _camera.transform;
        
        }
    }

    void Update()
    {
       if (Input.GetKeyDown(_KeyThird))
       {
            SetMode(ECameraMode.ThirdPerson, _snapOnModeChange);
       }

       if (Input.GetKeyDown(_KeyFirst))
       {
            SetMode(ECameraMode.FirstPerson, _snapOnModeChange);
        }

       if (Input.GetKeyDown(_KeyQuarter))
       {
            SetMode(ECameraMode.QuarterView, _snapOnModeChange);
        }

       if (Input.GetKeyDown(_KeyLockOn))
       {
            LockOn();
       }
    }

    private void LateUpdate()
    {
        if (_target == null || _camTr == null)
        {
            return;
        }

        switch (_mode)
        {
            case ECameraMode.ThirdPerson:
                TickThird();
                break;
            case ECameraMode.FirstPerson:
                TickFirst();
                break;
            case ECameraMode.QuarterView:
                TickQuarter();
                break;
            case ECameraMode.LockOnView:
                TickLockOn();
                break;
        }

        if (_drawDebug)
        {
            CPrint.Line3D(_camTr.position, _target.position, Color.yellow);
        }
    }

    private void SetMode(ECameraMode mode, bool snap)
    {
        _mode = mode;

        CPrint.Section($"모드 : {_mode}");

        switch (_mode)
        {
            case ECameraMode.ThirdPerson:
                InitThird(snap);
                break;
            case ECameraMode.FirstPerson:
                InitFirst(snap);
                break;
            case ECameraMode.QuarterView:
                InitQuarter(snap);
                break;
            case ECameraMode.LockOnView:
                InitQuarter(snap);
                break;
        }
    }

    private float GetSmoothT(float sharpness)
    {
        return 1f - Mathf.Exp(-sharpness * Time.deltaTime);
    }

    private void ApplyPose(Vector3 desiredPos, Quaternion desiredRot, float sharpness, bool snap)
    {
        if (snap)
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
