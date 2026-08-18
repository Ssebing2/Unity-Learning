using UnityEngine;

public class CRaycastBasic : MonoBehaviour
{
    [Header("속도")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotateSpeed = 80f;

    [Header("Ray 설정")]
    [SerializeField] private float _rayDistance = 5f;
    [SerializeField] private float _rayAngle = 30f;

    [Header("성공 연출")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _cameraRotateSpeed = 30f;
    [SerializeField] private CameraFollowBasic _cameraFollow;

    private float _survivalTime;         // 몇 초 동안 안부딪혔는지
    private int _currentCheckpoint;     // 현재 체크포인트 도달횟수

    private bool _isSuccess;            // 최종 성공했는지

    private void Update()
    {
        if (!_isSuccess)
        {
            AutoDrive();

            _survivalTime += Time.deltaTime;

            Debug.Log($"시간 : {_survivalTime:F1} / 체크포인트 : {_currentCheckpoint}");
        }

        if (!_isSuccess && _survivalTime >= 30f && _currentCheckpoint >= 5)
        {
            _isSuccess = true;
            _cameraFollow.enabled = false;
            Debug.Log("SUCCESS!");
        }

        if (_isSuccess)
        {
            SuccessCamera();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            _survivalTime = 0f;

            Debug.Log("벽 충돌! 시간 초기화");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"트리거 감지 : {other.name}");

        Checkpoint checkpoint = other.GetComponent<Checkpoint>();

        if (checkpoint == null)
        {
            return;
        }

        if (checkpoint.Checkpointindex == _currentCheckpoint)
        {
            _currentCheckpoint++;
            Debug.Log($"체크포인트 : {_currentCheckpoint}");
        }
    }

    private void AutoDrive()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 centerDir = transform.forward;

        Vector3 leftDir = Quaternion.Euler(0f, -_rayAngle, 0f) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0f, _rayAngle, 0f) * transform.forward;

        bool centerHit = Physics.Raycast(origin, centerDir, _rayDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        bool leftHit = Physics.Raycast(origin, leftDir, _rayDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        bool rightHit = Physics.Raycast(origin, rightDir, _rayDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        if (!centerHit)
        {
            transform.Translate(Vector3.forward *_moveSpeed *Time.deltaTime);
        }

        if (centerHit)
        {
            if(leftHit && !rightHit)
            {
                transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);
            }

            else if (rightHit && !leftHit)
            {
                transform.Rotate(Vector3.up * -_rotateSpeed * Time.deltaTime);
            }

            else
            {
                transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);
            }
        }

        Debug.DrawRay(origin, centerDir * _rayDistance, centerHit ? Color.red : Color.green);
        Debug.DrawRay(origin, leftDir * _rayDistance, leftHit ? Color.red : Color.yellow);
        Debug.DrawRay(origin, rightDir * _rayDistance, rightHit ? Color.red : Color.blue);
    }

    private void SuccessCamera()
    {
        _mainCamera.transform.RotateAround(transform.position,Vector3.up,_cameraRotateSpeed * Time.deltaTime);

        _mainCamera.transform.LookAt(transform);
    }

    private void OnGUI()
    {
        GUI.color = new Color(0f, 0f, 0f, 0.8f);
        GUI.Box(new Rect(10, 10, 200, 80), "");

        GUI.color = Color.white;
        GUI.Label(
           new Rect(20, 20, 300, 30),
           $"경과 시간 : {_survivalTime}"
       );

        GUI.Label(
            new Rect(20, 40, 300, 30),
            $"체크포인트 갯수 : {_currentCheckpoint} / 5"
        );

        GUI.Label(
            new Rect(20, 60, 300, 30),
            $"성공 여부 : {_isSuccess}"
        );
    }
}
