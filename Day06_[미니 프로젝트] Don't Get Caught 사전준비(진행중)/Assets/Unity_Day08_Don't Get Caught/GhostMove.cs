using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostMove : MonoBehaviour
{
    [Header("귀신 움직임")]
    [SerializeField] private List<GameObject> _waypoints = new List<GameObject>();
    [SerializeField] private float _moveSpeed = 20f;

    [SerializeField] private float _time = 0f;
    [SerializeField] private float _delaytime = 2f;


    private int _currentIndex = 0;
    private Vector3 _starPos;

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
        PrintInfo();
        Debug.Log(_waypoints);
    }

 

    private bool DelayTime()
    {
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
        float step = _moveSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentIndex].transform.position, step);

       if (transform.position == _waypoints[_currentIndex].transform.position)
        {
            Debug.Log($"[{_currentIndex}]번째 장소 도착");
         
            if (DelayTime())
            {
                _currentIndex++;
            }
            
            

            if (_currentIndex >= _waypoints.Count )
            {
                _currentIndex = 0;
            }
        }
    }
}
