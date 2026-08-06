using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMove : MonoBehaviour
{
    [Header("±Í½Å ¿òÁ÷ÀÓ")]
    [SerializeField] private Transform _ghost;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _leftLimit = -5f;
    [SerializeField] private float _rightLimit = 5f;

    private int _direction = 1;

    private void Start()
    {
        
    }

    private void Update()
    {
        GhostMoving();
    }

    private void GhostMoving()
    {
            _ghost.position += Vector3.right * _direction * _moveSpeed *Time.deltaTime;

            if (_ghost.position.x >= _rightLimit )
            {
                _direction = -1;
            }

            if (_ghost.position.x <= _leftLimit)
            {
                _direction = 1;
            }
    }    
}
