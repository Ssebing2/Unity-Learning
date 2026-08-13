using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class CameraFollowBasic : MonoBehaviour
{
    [SerializeField] private List<Transform> _enemy = new List<Transform>();
    [SerializeField] private Color _lockonColor = Color.red;
    [SerializeField] private float _lockonDistanceMax = 10f;
    [SerializeField] private bool _enemyEnable;
    [SerializeField] private float _lockonSharpness = 1.0f;

    [SerializeField] private KeyCode _previouslyEnemy = KeyCode.Q;
    [SerializeField] private KeyCode _nextEnemy = KeyCode.E;
    [SerializeField] private KeyCode _disableEnemy = KeyCode.K;

    private Transform _lockTarget;
    private Color _lockoncolorsave;
    private Renderer _lockRend;
    private int _currentLockon;

    private Transform FindNearestEnemy() // 가까운 적 찾기
    {
        Transform nearestEnemy = null;
        float nearestDistance = _lockonDistanceMax;

        for (int i = 0; i < _enemy.Count; i++)
        {
            float distance = Vector3.Distance(_enemy[i].position, _target.position);

            if (distance < nearestDistance)
            {
                nearestEnemy = _enemy[i];
                nearestDistance = distance;

                _currentLockon = i;
            }
        }
        return nearestEnemy;
    }

    private void LockOn() // 적 유무에따른 카메라 전환
    {
        if (_mode == ECameraMode.LockOnView)
        {
            LockOnClear();

            return;
        }

        _lockTarget = FindNearestEnemy();

        if (_lockTarget !=  null )
        {
            _lockRend = _lockTarget.GetComponent<Renderer>();
            _lockoncolorsave = _lockRend.material.color;
            _lockRend.material.color = _lockonColor; // 색 변경

            Debug.Log("락온이 시작됐습니다.");
            Debug.Log($"현재 대상 : {_lockTarget.name}");
            SetMode(ECameraMode.LockOnView, _snapOnModeChange);
        }
    }

    private void InitLockOn(bool snap)
    {
        Vector3 desiredPos;
        Quaternion desiredRot;
        
        BuildLockOnPose(out desiredPos, out desiredRot);

        ApplyPose(desiredPos, desiredRot, _lockonSharpness, snap);
    }

    private void TickLockOn()
    {
        if (_lockTarget == null)
        {
            LockOnClear();           
            return;
        }

        float distance = Vector3.Distance(_lockTarget.position, _target.position);

        if (distance > _lockonDistanceMax)
        {
            LockOnClear();
            return;
        }

        if (!_lockTarget.gameObject.activeSelf)
        {
            LockOnClear();
            return;
        }

        UpdateLockOnKey();
        DisableEnemy();

        Vector3 desiredPos;
        Quaternion desiredRot;

        BuildLockOnPose(out desiredPos, out desiredRot);

        ApplyPose(desiredPos, desiredRot, _lockonSharpness, false);
    }

    private void BuildLockOnPose(out Vector3 desiredPos, out Quaternion desiredRot)
    {
        Vector3 middlePoint = (_target.position + _lockTarget.position) / 2f;

        desiredPos = _target.position + (_target.rotation * _thirdOffset);

        desiredRot = Quaternion.LookRotation(middlePoint - desiredPos, Vector3.up);
    }

    private void LockOnClear() // 락온 해제
    {
        Debug.Log("락온이 해제됐습니다.");
        SetMode(ECameraMode.ThirdPerson, _snapOnModeChange);
        if (_lockRend != null)
        {
            _lockRend.material.color = _lockoncolorsave;
        }  
        _lockTarget = null;

    }

    private void UpdateLockOnKey() // Q / E 키 적용
    {
        if (Input.GetKeyDown(_previouslyEnemy))
        {
            _lockRend.material.color = _lockoncolorsave;

            _currentLockon--;


            if (_currentLockon < 0)
            {
                _currentLockon = _enemy.Count - 1;
            }

            for (int i = 0; i < _enemy.Count; i++)
            {
                if (_enemy[_currentLockon] == null || !_enemy[_currentLockon].gameObject.activeSelf)
                {
                    _currentLockon--;

                    if (_currentLockon < 0)
                    {
                        _currentLockon = _enemy.Count - 1;
                    }

                    continue;
                }

                break;
            }

            _lockTarget = _enemy[_currentLockon];

            _lockRend = _lockTarget.GetComponent<Renderer>();
            _lockoncolorsave = _lockRend.material.color;
            _lockRend.material.color = _lockonColor; // 색 변경
        }

        if (Input.GetKeyDown(_nextEnemy))
        {
            _lockRend.material.color = _lockoncolorsave;

            _currentLockon++;
            

            if ( _currentLockon > _enemy.Count - 1) 
            {
                _currentLockon = 0;
            }

            for (int i = 0; i < _enemy.Count; i++)
            {
                if (_enemy[_currentLockon] == null || !_enemy[_currentLockon].gameObject.activeSelf)
                {
                    _currentLockon++;

                    if (_currentLockon > _enemy.Count - 1)
                    {
                        _currentLockon = 0;
                    }

                    continue;
                }

                break;
            }

            _lockTarget = _enemy[_currentLockon];

            _lockRend = _lockTarget.GetComponent<Renderer>();
            _lockoncolorsave = _lockRend.material.color;
            _lockRend.material.color = _lockonColor; // 색 변경
        }

    }

    private void DisableEnemy()
    {
        if (Input.GetKeyDown(_disableEnemy))
        {
            if (_lockTarget != null)
            {
                _lockTarget.gameObject.SetActive(false);
            }
        }
    }
    

}
