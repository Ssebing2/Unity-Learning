using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("이동 / 회전")]
    [SerializeField] private float _moveSpeed = 45.0f;
    [SerializeField] private float _rotateSpeed = 120.0f;

    [Header("점프 (리지드 바디 필요")]
    [SerializeField] private float _jumpImpulse = 6.0f;
    [SerializeField] private float _groundCheckDistance = 25f;
    [SerializeField] private LayerMask _groundMask = ~0;

    [Header("옵션")]
    [SerializeField] private bool _resetRotationOnRMB = true;

    private Rigidbody _playerRb;

    private Vector3 _moveDirection;
    private bool _isMoving;


    void Start()
    {
        TryGetComponent(out _playerRb);

        if (_playerRb == null)
        {
            CPrint.Warn("점프 → 리지드 바디가 필요합니다.");

            enabled = false;
            return;
        }
    }

    private void Update()
    {
        InputRotateMove();
        InputIdentity();
        InputJump();
    }

    private void InputRotateMove()
    {
        float rotate = Input.GetAxis("Horizontal");
        float move = Input.GetAxis("Vertical");

        rotate *= _rotateSpeed * Time.deltaTime;
        move *= _moveSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up * rotate);
        transform.Translate(Vector3.forward*move, Space.Self);


    }

    private void InputIdentity()
    {
        if (!_resetRotationOnRMB)
        {
            return;
        }

        if (Input.GetMouseButton(1))
        {
            transform.localRotation = Quaternion.identity;
        }
    }

    private void InputJump()
    {
        if (_playerRb == null)
        {
            return;
        }

        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        if (!isGrounded())
        {
            CPrint.Once("점프 로그", "점프 : 바닥이 아닐 때는 점프불가");
            return;
        }

        _playerRb.AddForce(Vector3.up * _jumpImpulse, ForceMode.Impulse);
    }

    private bool isGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(origin, Vector3.down, _groundCheckDistance, _groundMask);
    }

}