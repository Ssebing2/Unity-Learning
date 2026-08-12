using UnityEngine;

public class SituationColor : MonoBehaviour
{
    private Renderer _rend;

    private void Awake()
    {
        _rend = GetComponent<Renderer>();
        _rend = GetComponentInChildren<Renderer>();
    }

    // 순찰 회색
    public void PatrolColor()
    {
        _rend.material.color = Color.yellow;
    }

    // 대기 초록색
    public void WaitColor()
    {
        _rend.material.color = Color.green;
    }

    // 추적 빨간색
    public void TrackColor()
    {
        _rend.material.color = Color.red;
    }
}