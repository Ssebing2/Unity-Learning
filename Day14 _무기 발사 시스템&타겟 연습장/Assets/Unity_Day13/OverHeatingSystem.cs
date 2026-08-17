using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverHeatingSystem : MonoBehaviour
{
    private enum EOverHeatingSystem
    {
        Cooling,
        OverHeating
    }

    #region 인스펙터
    [Header("Heat 관련")]
    [SerializeField, Range(0f, 100f)] private float _heatPower;
    [SerializeField, Range(0f, 100f)] private float _WarningHeat = 40f;
    [SerializeField, Range(0f, 100f)] private float _overHeat = 80f;
    [SerializeField, Range(0f, 100f)] private float _coolingHeat = 20f;
    [SerializeField, Min(0f)] private float _heatIncreaseSpeed = 20f;
    [SerializeField, Min(0f)] private float _heatdecreaseSpeed = 15f;

    [Header("시각화 관련")]
    [SerializeField, Min(0f)] private float _heatMaxRotate = 5f;
    [SerializeField] private float _baseMagnitude = 0.1f;
    [SerializeField] private Color _lowHeatColor = Color.blue;
    [SerializeField] private Color _middleHeatColor = Color.yellow;
    [SerializeField] private Color _highHeatColor = Color.red;

    [Header("토글 키")]
    [SerializeField] private KeyCode _KeyHeat = KeyCode.Space;
    #endregion

    #region 변수
    private Renderer _heatColor;
    private EOverHeatingSystem _currentHeat;
    private Quaternion _resetRotate;
    private Vector3 _resetScale;
    private float _currentFreq;
    private float _currentMag;
    private float _shake;
    private float _scaleValue;
    private float _overHeatTimer;
    private float _overHeatDelay = 2f;
    #endregion

    private void Awake()
    {
        _heatColor = GetComponent<Renderer>();

        if (_heatColor == null)
        {
            Debug.LogError("HeatController 오브젝트에 Renderer가 없습니다.");
        }
    }

    private void Start()
    {
        _resetRotate = transform.rotation;
        _resetScale = transform.localScale;
        CurrentHeat(EOverHeatingSystem.OverHeating);

        Debug.Log("==== 과열 시스템이 시작됩니다 ====");
        Debug.Log("Space를 누르면 히트가 증가됩니다.");

    }

    private void Update()
    {
       if (_currentHeat == EOverHeatingSystem.OverHeating)
        {
            _overHeatTimer += Time.deltaTime;

            if (_overHeatTimer >= _overHeatDelay)
            {
                Cooling();
            }
        }

       else
        {
            if ( Input.GetKey(_KeyHeat))
            {
                Heating();
            }
            else
            {
                Cooling();
            }
        }

        HeatingColorSystem();

    }

    private void Heating()
    {
        _heatPower += _heatIncreaseSpeed * Time.deltaTime;
        _heatPower = Mathf.Clamp(_heatPower, 0f, 100f);

        HeatShake();
        HeatSalce();

        if (_heatPower >= _overHeat)
        {
            Debug.Log("히트 과열 상태 진입");

            CurrentHeat(EOverHeatingSystem.OverHeating);

            _overHeatTimer = 0;
        }
    }

    private void Cooling()
    {
        _heatPower -= _heatdecreaseSpeed * Time.deltaTime;
        _heatPower = Mathf.Clamp(_heatPower, 0f, 100f);

        HeatSalce();

        if (_heatPower <= _coolingHeat)
        {
            _heatColor.material.color = _highHeatColor;
            CurrentHeat(EOverHeatingSystem.Cooling);
        }
    }

    private void CurrentHeat(EOverHeatingSystem state)
    {
        _currentHeat = state;
    }


    private void HeatingColorSystem()
    {
        if (_heatPower < _WarningHeat)
        {
            _heatColor.material.color = _lowHeatColor;
        }

        else if (_heatPower < _overHeat)
        {
            _heatColor.material.color = _middleHeatColor;
        }

        else
        {
            _heatColor.material.color = _highHeatColor;
        }
    }

    private void HeatShake()
    {
        float heatRatio = _heatPower / 100f;

        _currentFreq = _heatMaxRotate + (_heatPower * 0.1f);
        _currentMag = _baseMagnitude + (heatRatio * _heatMaxRotate);

        _shake = Mathf.Sin(Time.time * _currentFreq) * _currentMag;

        transform.localRotation = _resetRotate * Quaternion.Euler(0f, 0f, _shake);
    }

    private void HeatSalce()
    {
        float heatRatio = _heatPower / 100f;

        _scaleValue = Mathf.Lerp(1f, 1.5f, heatRatio);

        transform.localScale = _resetScale * _scaleValue;
    }

    private void OnGUI()
    {
        string keyModeText = "입력키 : Space";
        string currenVelue = $"히트 값 : {_heatPower}";
        string currentHeat = $"과열 상태 : {_currentHeat}";


        GUI.color = new Color(0f, 0f, 0f, 0.8f);
        GUI.Box(new Rect(10, 10, 230, 80), "");

        GUI.color = Color.white;
        GUI.Label(new Rect(20, 20, 200, 20), keyModeText);
        GUI.Label(new Rect(20, 40, 200, 20), currenVelue);
        GUI.Label(new Rect(20, 60, 200, 20), currentHeat);

    }
}
