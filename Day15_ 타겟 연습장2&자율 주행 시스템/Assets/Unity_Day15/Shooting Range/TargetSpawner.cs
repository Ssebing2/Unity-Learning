using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private GameObject _targetPrefab;

    [Header("Spawn Setting")]
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private int _maxTargetCount = 3;

    [Header("Spawn Range")]
    [SerializeField] private Vector3 _minSpawnPosition;
    [SerializeField] private Vector3 _maxSpawnPosition;

    private float _spawnTimer;

    private List<GameObject> _activeTargets = new List<GameObject>();

    private void Update()
    {
        SpawnTimer();
    }

    private void SpawnTimer()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _spawnInterval)
        {
            _spawnTimer = 0f;

            SpawnTarget();
        }
    }

    private void SpawnTarget()
    {
        if (_activeTargets.Count >= _maxTargetCount)
        {
            return;
        }

        Vector3 randomPosition = new Vector3(
            Random.Range(_minSpawnPosition.x, _maxSpawnPosition.x),
            Random.Range(_minSpawnPosition.y, _maxSpawnPosition.y),
            Random.Range(_minSpawnPosition.z, _maxSpawnPosition.z)
        );

        GameObject target = Instantiate(_targetPrefab, randomPosition, Quaternion.identity);

        _activeTargets.Add(target);

        Target2 targetScript = target.GetComponent<Target2>();

        if (targetScript != null)
        {
            targetScript.SetSpawner(this);
        }
    }

    public void RemoveTarget(GameObject target)
    {
        Debug.Log($"제거 전 Count : {_activeTargets.Count}");

        bool removed = _activeTargets.Remove(target);

        Debug.Log($"Remove 성공 여부 : {removed}");
        Debug.Log($"제거 후 Count : {_activeTargets.Count}");

        SpawnTarget();
    }
}
