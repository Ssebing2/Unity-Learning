using UnityEngine;

public class SwitchObject : MonoBehaviour
{
    private Renderer _rend;

    private void Awake()
    {
        _rend = GetComponent<Renderer>();

        ResetColor();
    }

    // 회색으로 초기화
    public void ResetColor()
    {
        _rend.material.color = Color.gray;
    }

    // 정답일 때 초록색
    public void SetCorrectColor()
    {
        _rend.material.color = Color.green;
    }

    // 오답일 때 빨간색
    public void SetWrongColor()
    {
        _rend.material.color = Color.red;
    }

    // 라인이 연결될 위치
    public Vector3 GetLinePosition()
    {
        return transform.position + Vector3.up * 0.5f;
    }
}
