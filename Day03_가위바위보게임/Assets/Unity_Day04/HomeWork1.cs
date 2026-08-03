using UnityEngine;

public class HomeWork1 : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _upperBody;
    [SerializeField] private Transform _boomPivot;
    [SerializeField] private Transform _armPivot;
    [SerializeField] private Transform _bucketPivot;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotationSpeed = 50f;

    private void Start()
    {
        void Warn(string msg) => Debug.Log($"=== [Warn]{msg}");

        if (_player == null ||
            _upperBody == null ||
            _boomPivot == null ||
            _armPivot == null ||
            _bucketPivot == null)
        {
            Warn("인스펙터에 굴착기 오브젝트를 모두 연결해주세요.");
        }
    }

    private void Update()
    {
        MoveExcavator();
        RotateUpperBody();
        RotateBoom();
        RotateArm();
        RotateBucket();
    }

    private void MoveExcavator()
    {
        if (_player == null)
        {
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            _player.position += Vector3.forward * (_speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            _player.position += Vector3.back * (_speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            _player.position += Vector3.left * (_speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            _player.position += Vector3.right * (_speed * Time.deltaTime);
        }
    }

    private void RotateUpperBody()
    {
        if (_upperBody == null)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            _upperBody.Rotate(Vector3.up,-_rotationSpeed * Time.deltaTime,Space.Self);
        }

        if (Input.GetKey(KeyCode.E))
        {
            _upperBody.Rotate(Vector3.up,_rotationSpeed * Time.deltaTime,Space.Self);
        }
    }

    private void RotateBoom()
    {
        if (_boomPivot == null)
        {
            return;
        }

        if (Input.GetKey(KeyCode.R))
        {
            _boomPivot.Rotate( Vector3.right, -_rotationSpeed * Time.deltaTime,Space.Self);
        }

        if (Input.GetKey(KeyCode.F))
        {
            _boomPivot.Rotate(Vector3.right,_rotationSpeed * Time.deltaTime,Space.Self);
        }
    }

    private void RotateArm()
    {
        if (_armPivot == null)
        {
            return;
        }

        if (Input.GetKey(KeyCode.T))
        {
            _armPivot.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime,Space.Self);
        }

        if (Input.GetKey(KeyCode.G))
        {
            _armPivot.Rotate(Vector3.forward, -_rotationSpeed * Time.deltaTime,Space.Self);
        }
    }

    private void RotateBucket()
    {
        if (_bucketPivot == null)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Y))
        {
            _bucketPivot.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime,Space.Self);
        }

        if (Input.GetKey(KeyCode.H))
        {
            _bucketPivot.Rotate(Vector3.forward, -_rotationSpeed * Time.deltaTime,Space.Self);
        }
    }
}
