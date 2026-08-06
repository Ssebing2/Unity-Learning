using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerandCamerMove : MonoBehaviour
{
    [Header("플레이어 움직임")]
    [SerializeField] private float _movespeed = 5.0f;
    [SerializeField] private float _rotationSpeed = 100.0f;

    private void Start()
    {
        
    }

    private void Update()
    {
        PlayerMoving();
        PlayerRotation();
    }

    private void PlayerMoving()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += transform.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= transform.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= transform.right;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += transform.right;
        }

        moveDirection = moveDirection.normalized;

        transform.position += moveDirection * _movespeed * Time.deltaTime;
    }

    private void PlayerRotation()
    {
        float rotationDirection = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            rotationDirection = -1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotationDirection = 1f;
        }

        transform.Rotate(Vector3.up, rotationDirection * _rotationSpeed * Time.deltaTime);
    }

}
