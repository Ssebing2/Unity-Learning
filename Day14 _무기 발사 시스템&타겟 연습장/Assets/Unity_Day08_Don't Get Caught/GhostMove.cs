using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostMove : MonoBehaviour
{
    [Header("색깔 표현")]
    private SituationColor _situationColor;
    private Renderer _rend;

    [Header("귀신 움직임")]
    [SerializeField] private List<GameObject> _waypoints = new List<GameObject>();
    [SerializeField] private float _moveSpeed = 40f;
    [SerializeField] private float _range = 50f;
    [SerializeField] private float _stoprange = 10f;

    [SerializeField] private float _time = 0f;
    [SerializeField] private float _delaytime = 2f;

    [Header("타겟 (바라볼 대상)")]
    [SerializeField] private Transform _target;


    private int _currentIndex = 0;
    private Vector3 _starPos;

    private void Awake()
    {
        _situationColor = GetComponent<SituationColor>();
        
    }

    private void Start()
    {
        _starPos = transform.position;
        if (_waypoints == null )
        {
            Debug.Log("WayPoint가 지정이 안되어있습니다. 인스펙터 확인필요");
        }
    }

    private void Update()
    {
        GhostLookAt();
    }

 
    private void GhostLookAt()
    {
        if (_target == null)
        {
            return;
        }

       float distance = Vector3.Distance(transform.position, _target.transform.position);


        if (distance <= _stoprange)
        {
            _situationColor.TrackColor();
            transform.LookAt(_target);
            transform.localScale = Vector3.one * 1.5f;
        }

        else if (distance <= _range)
        {
            Debug.Log("플레이어 발견!");
            _situationColor.TrackColor();
            transform.LookAt(_target);
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _moveSpeed * Time.deltaTime);
            transform.localScale = Vector3.one * 1.5f;
        }

        else
        {
            PrintInfo();
            transform.localScale = Vector3.one;
        }
    
        
    }

    private bool DelayTime()
    {
        _situationColor.WaitColor();
        _time += Time.deltaTime;
        if (_time >= _delaytime)
        {
            _time = 0.0f;
            return true;
        }

        else
        {
            return false;
        }
            
    }

    private void PrintInfo()
    {
        _situationColor.PatrolColor();
        float step = _moveSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentIndex].transform.position, step);

       if (transform.position == _waypoints[_currentIndex].transform.position)
        { 
            if (DelayTime())
            { 
                _currentIndex++;
                Debug.Log($"[{_currentIndex}]번째 장소 도착");
            }
            
            if (_currentIndex >= _waypoints.Count )
            {
                _currentIndex = 0;
            }
        }
    }
}
