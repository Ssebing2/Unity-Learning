using UnityEngine;

public class PlayerandCamerMove : MonoBehaviour
{
    [Header("플레이어 움직임")]
    [SerializeField] private float _moveSpeed = 20.0f;

    private Vector3 _moveDirection;
    private bool _isMoving;

    private void Update()
    {
        CheckDirectionInput();
        PlayerMoving();
    }

    private void CheckDirectionInput()
    {
        // 키를 처음 누른 순간에만 이동 방향을 결정한다.
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetDirection(transform.forward);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SetDirection(-transform.forward);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            SetDirection(-transform.right);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SetDirection(transform.right);
        }

        // 방향키 중 하나라도 누르고 있으면 이동
        _isMoving =
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D);
    }

    private void SetDirection(Vector3 direction)
    {
        direction.y = 0f;
        _moveDirection = direction.normalized;

        // Player 전체 회전
        // 자식인 몸과 카메라도 함께 돌아간다.
        transform.forward = _moveDirection;
    }

    private void PlayerMoving()
    {
        if (!_isMoving)
        {
            return;
        }

        transform.position += _moveDirection * _moveSpeed * Time.deltaTime;
    }
}