using UnityEngine;

public class Target : MonoBehaviour
{
    private enum ETargetType
    {
        Big,
        Small,
        Bomb
    }

    private enum EMoveType
    {
        Horizontal,
        Vertical
    }

    [Header("타겟 설정")]
    [SerializeField] private ETargetType _targetType;
    [SerializeField] private EMoveType _moveType;

    [Header("이동 설정")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _moveDistance = 3f;

    [Header("넘어지는 설정")]
    [SerializeField] private float _fallSpeed = 180f;
    [SerializeField] private float _fallAngle = 90f;

    [Header("매니저")]
    [SerializeField] private TargetRangeManager _targetRangeManager;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Quaternion _fallRotation;

    private int _direction = 1;
    private bool _isHit;

    private void Start()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;

        _fallRotation = _startRotation * Quaternion.Euler(_fallAngle, 0f, 0f);
    }

    private void Update()
    {
        if (!_isHit)
        {
            MoveTarget();
        }

        else
        {
            FallTarget();
        }
    }

    private void MoveTarget()
    {
        Vector3 moveDirection = Vector3.zero;

        switch (_moveType)
        {
            case EMoveType.Horizontal:
                moveDirection = Vector3.right;
                break;

            case EMoveType.Vertical:
                moveDirection = Vector3.up;
                break;
        }

        transform.position +=
            moveDirection *
            _direction *
            _moveSpeed *
            Time.deltaTime;

        if (_moveType == EMoveType.Horizontal)
        {
            if (transform.position.x >= _startPosition.x + _moveDistance)
            {
                _direction = -1;
            }

            if (transform.position.x <= _startPosition.x - _moveDistance)
            {
                _direction = 1;
            }
        }

        if (_moveType == EMoveType.Vertical)
        {
            if (transform.position.y >= _startPosition.y + _moveDistance)
            {
                _direction = -1;
            }

            if (transform.position.y <= _startPosition.y - _moveDistance)
            {
                _direction = 1;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_isHit)
        {
            return;
        }

        if (!collision.gameObject.CompareTag("Bullet"))
        {
            return;
        }

        _isHit = true;

        int score = GetScore();

        if (_targetRangeManager != null)
        {
            _targetRangeManager.AddScore(score);
        }
    }

    private int GetScore()
    {
        switch (_targetType)
        {
            case ETargetType.Big:
                return 1;

            case ETargetType.Small:
                return 3;

            case ETargetType.Bomb:
                return -3;
        }

        return 0;
    }

    private void FallTarget()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _fallRotation, _fallSpeed * Time.deltaTime);
    }
}