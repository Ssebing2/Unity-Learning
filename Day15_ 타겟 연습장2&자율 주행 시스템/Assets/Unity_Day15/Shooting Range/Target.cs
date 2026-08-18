using UnityEngine;

public class Target2 : MonoBehaviour
{
    private TargetSpawner _spawner;
    private bool _isHit;

    [Header("Hit Effect")]
    [SerializeField] private float _downSpeed = 2f;
    [SerializeField] private float _destroyDelay = 1.5f;

    private float _destroyTimer;

    private void Update()
    {
        if (_isHit)
        {
            MoveDown();
        }
    }

    public void SetSpawner(TargetSpawner spawner)
    {
        _spawner = spawner;
    }

    public void Hit()
    {
        if (_isHit)
        {
            return;
        }

        _isHit = true;

        Debug.Log($"{gameObject.name} 맞음!");

        if (_spawner != null)
        {
            _spawner.RemoveTarget(gameObject);
        }
        else
        {
            Debug.LogError("Spawner가 연결되어 있지 않음!");
        }

        Collider col = GetComponent<Collider>();

        if (col != null)
        {
            col.enabled = false;
        }
    }

    private void MoveDown()
    {
        transform.position += Vector3.down * _downSpeed * Time.deltaTime;

        _destroyTimer += Time.deltaTime;

        if (_destroyTimer >= _destroyDelay)
        {
            Destroy(gameObject);
        }
    }
}
