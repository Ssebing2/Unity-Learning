using UnityEngine;

public class GrenadeFragment : MonoBehaviour
{
    [Header("파편 설정")]
    [SerializeField] private GameObject _fragmentPrefab;
    [SerializeField] private int _fragmentCount = 8;
    [SerializeField] private float _fragmentPower = 5f;
    [SerializeField] private float _fragmentUpPower = 2f;

    [SerializeField]
    private Vector3 _fragmentScale = new Vector3(0.1f, 0.1f, 0.1f);

    private bool _isExploded;

    private void OnCollisionEnter(Collision collision)
    {
        if (_isExploded)
        {
            return;
        }

        _isExploded = true;

        Explode();

        Destroy(gameObject);
    }


    private void Explode()
    {
        if (_fragmentPrefab == null)
        {
            Debug.LogWarning("Fragment Prefab이 연결되지 않았습니다.");
            return;
        }

        for (int i = 0; i < _fragmentCount; i++)
        {
            GameObject fragment = Instantiate(_fragmentPrefab,transform.position,Quaternion.identity);

            fragment.transform.localScale = _fragmentScale;

            Rigidbody rb = fragment.GetComponent<Rigidbody>();

            if (rb == null)
            {
                Debug.LogWarning("Fragment에 Rigidbody가 없습니다.");
                Destroy(fragment);
                continue;
            }

            // 사방으로 랜덤한 방향 생성
            Vector3 randomDirection = Random.insideUnitSphere;

            // 바닥으로 박히는 걸 줄이기 위해 Y값을 위쪽으로 변경
            randomDirection.y = Mathf.Abs(randomDirection.y) + _fragmentUpPower;

            randomDirection.Normalize();

            rb.AddForce(randomDirection * _fragmentPower,ForceMode.Impulse);

            // 파편이 계속 남지 않도록 삭제
            Destroy(fragment, 3f);
        }
    }
}
