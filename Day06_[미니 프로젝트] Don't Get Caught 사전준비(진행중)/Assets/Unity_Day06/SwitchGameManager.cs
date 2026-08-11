using System.Collections;
using UnityEngine;

public class SwitchGameManager : MonoBehaviour
{
    [Header("스위치")]
    [SerializeField] private SwitchObject[] _switches;

    [Header("라인")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _lineStart;

    [Header("라운드 설정")]
    [SerializeField] private int _totalRound = 5;
    [SerializeField] private float _nextRoundDelay = 1.5f;

    // 현재 정답 인덱스
    private int _answerIndex;

    // 현재까지 진행한 라운드
    private int _currentRound;

    // 맞춘 횟수
    private int _correctCount;

    // 입력 가능 여부
    private bool _canSelect;

    private void Start()
    {
        if (_switches.Length != 3)
        {
            Debug.LogError("스위치를 정확히 3개 연결해주세요.");
            return;
        }

        _lineRenderer.positionCount = 2;
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.enabled = false;

        StartRound();
    }

    private void Update()
    {
        if (!_canSelect)
        {
            return;
        }

        // 숫자키 1
        if (Input.GetKeyDown(KeyCode.Alpha1) ||
            Input.GetKeyDown(KeyCode.Keypad1))
        {
            SelectSwitch(0);
        }

        // 숫자키 2
        else if (Input.GetKeyDown(KeyCode.Alpha2) ||
                 Input.GetKeyDown(KeyCode.Keypad2))
        {
            SelectSwitch(1);
        }

        // 숫자키 3
        else if (Input.GetKeyDown(KeyCode.Alpha3) ||
                 Input.GetKeyDown(KeyCode.Keypad3))
        {
            SelectSwitch(2);
        }
    }

    // 라운드 시작
    private void StartRound()
    {
        ResetSwitches();

        _lineRenderer.enabled = false;

        // 0, 1, 2 중 하나를 정답으로 결정
        _answerIndex = Random.Range(0, 3);

        _canSelect = true;

        Debug.Log("============================");
        Debug.Log($"{_currentRound + 1} 라운드 시작");
        Debug.Log("1, 2, 3 중 하나를 선택하세요.");

        // 치트
        Debug.Log(
            $"[치트] 정답은 {_answerIndex + 1}번 스위치입니다. " +
            $"정답 인덱스: {_answerIndex}"
        );
    }

    // 스위치 선택
    private void SelectSwitch(int selectedIndex)
    {
        _canSelect = false;

        DrawLine(selectedIndex);

        if (selectedIndex == _answerIndex)
        {
            _switches[selectedIndex].SetCorrectColor();

            _correctCount++;

            Debug.Log("정답입니다!");
        }
        else
        {
            _switches[selectedIndex].SetWrongColor();

            Debug.Log(
                $"오답입니다! 정답은 {_answerIndex + 1}번이었습니다."
            );
        }

        _currentRound++;

        if (_currentRound >= _totalRound)
        {
            StartCoroutine(FinishGame());
        }
        else
        {
            StartCoroutine(NextRound());
        }
    }

    // 선택한 스위치까지 선 표시
    private void DrawLine(int selectedIndex)
    {
        _lineRenderer.enabled = true;

        _lineRenderer.SetPosition(0, _lineStart.position);

        _lineRenderer.SetPosition(1,_switches[selectedIndex].GetLinePosition());
    }

    // 다음 라운드
    private IEnumerator NextRound()
    {
        yield return new WaitForSeconds(_nextRoundDelay);

        StartRound();
    }

    // 게임 종료
    private IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(_nextRoundDelay);

        float accuracy =
            (float)_correctCount / _totalRound * 100f;

        Debug.Log("============================");
        Debug.Log("게임 종료!");
        Debug.Log($"정답 횟수: {_correctCount} / {_totalRound}");
        Debug.Log($"정답률: {accuracy:F1}%");
        Debug.Log("============================");

        _lineRenderer.enabled = false;
    }

    // 모든 스위치를 회색으로 초기화
    private void ResetSwitches()
    {
        for (int i = 0; i < _switches.Length; i++)
        {
            _switches[i].ResetColor();
        }
    }
}