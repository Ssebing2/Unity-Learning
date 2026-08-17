using UnityEngine;

public class RockPaperScissors : MonoBehaviour
{
    private enum RPS
    {
        Scissors,  // 가위
        Rock,      // 바위
        Paper      // 보
    }

    private enum Result
    {
        Win,
        Lose,
        Draw
    }

    [Header("오브젝트")]
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _monster;

    [Header("시각적 요소")]
    [SerializeField] private LineRenderer _lineRenderer;

    [Header("게임 설정")]
    [SerializeField] private float _startDistance = 5f;
    [SerializeField] private float _winDistance = 10f;
    [SerializeField] private float _loseDistance = 2f;

    private Vector3 _playerStartPosition;
    private Vector3 _monsterStartPosition;

    private float _currentDistance;

    private int _winCount;
    private int _loseCount;
    private int _drawCount;

    private bool _isGameOver;

    private string _resultMessage = "1: 가위 / 2: 바위 / 3: 보";

    private void Start()
    {
        InitializeGame();
        SetLineColor(Color.red);

        Debug.Log("==============================");
        Debug.Log("     가위바위보 게임 시작!");
        Debug.Log("1 : 가위 / 2 : 바위 / 3 : 보");
        Debug.Log("R : 게임 재시작");
        Debug.Log("==============================");
    }

    private void Update()
    {
        UpdateDistance();
        UpdateLine();

        // 게임이 끝나도 R 키로 재시작은 가능
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
            return;
        }

        // 게임 종료 시 가위바위보 입력 차단
        if (_isGameOver)
        {
            return;
        }

        CheckPlayerInput();
    }

    private void InitializeGame()
    {
        if (_player == null || _monster == null)
        {
            Debug.LogError("플레이어 또는 몬스터가 연결되지 않았습니다.");
            enabled = false;
            return;
        }

        // 플레이어의 현재 위치를 기준으로 시작
        _playerStartPosition = _player.position;

        // 몬스터를 플레이어 오른쪽 5만큼 배치
        _monsterStartPosition =
            _playerStartPosition + Vector3.right * _startDistance;

        _player.position = _playerStartPosition;
        _monster.position = _monsterStartPosition;

        _winCount = 0;
        _loseCount = 0;
        _drawCount = 0;

        _isGameOver = false;
        _resultMessage = "1: 가위 / 2: 바위 / 3: 보";

        SetLineColor(Color.red);
        UpdateDistance();
    }

    private void CheckPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayRound(RPS.Scissors);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayRound(RPS.Rock);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayRound(RPS.Paper);
        }
    }

    private void PlayRound(RPS playerChoice)
    {
        // 0, 1, 2 중 하나를 랜덤으로 선택
        RPS monsterChoice = (RPS)Random.Range(0, 3);

        Result result = CheckResult(playerChoice, monsterChoice);

        ProcessResult(result);

        UpdateDistance();
        CheckGameOver();

        Debug.Log(
            $"플레이어 : {GetChoiceText(playerChoice)} / " +
            $"몬스터 : {GetChoiceText(monsterChoice)}"
        );

        Debug.Log(
            $"결과 : {GetResultText(result)} / " +
            $"승 {_winCount} | 패 {_loseCount} | 무 {_drawCount} / " +
            $"현재 거리 : {_currentDistance:F1}"
        );
    }

    private Result CheckResult(RPS player, RPS monster)
    {
        if (player == monster)
        {
            return Result.Draw;
        }

        bool isPlayerWin =
            player == RPS.Scissors && monster == RPS.Paper ||
            player == RPS.Rock && monster == RPS.Scissors ||
            player == RPS.Paper && monster == RPS.Rock;

        if (isPlayerWin)
        {
            return Result.Win;
        }

        return Result.Lose;
    }

    private void ProcessResult(Result result)
    {
        // 플레이어에서 몬스터로 향하는 방향
        Vector3 direction =
            (_monster.position - _player.position).normalized;

        switch (result)
        {
            case Result.Win:
                _winCount++;

                // 몬스터가 플레이어에게서 1만큼 멀어짐
                _monster.position += direction * 1f;

                _resultMessage = "승리! 몬스터가 멀어집니다.";
                break;

            case Result.Lose:
                _loseCount++;

                // 몬스터가 플레이어에게 1만큼 가까워짐
                _monster.position -= direction * 1f;

                _resultMessage = "패배! 몬스터가 가까워집니다.";
                break;

            case Result.Draw:
                _drawCount++;

                // 플레이어와 몬스터가 서로 반대 방향으로 0.5씩 이동
                _player.position -= direction * 0.5f;
                _monster.position += direction * 0.5f;

                _resultMessage = "무승부! 서로 반대 방향으로 이동합니다.";
                break;
        }
    }

    private void UpdateDistance()
    {
        _currentDistance = Vector3.Distance(
            _player.position,
            _monster.position
        );
    }

    private void CheckGameOver()
    {
        if (_currentDistance >= _winDistance)
        {
            _isGameOver = true;
            _resultMessage = "게임 종료! 플레이어 승리! R키로 재시작";
            SetLineColor(Color.green);

            Debug.Log("==============================");
            Debug.Log("플레이어 최종 승리!");
            PrintScore();
        }
        else if (_currentDistance <= _loseDistance)
        {
            _isGameOver = true;
            _resultMessage = "게임 종료! 플레이어 패배! R키로 재시작";
            SetLineColor(Color.red);

            Debug.Log("==============================");
            Debug.Log("플레이어 최종 패배!");
            PrintScore();
        }
    }

    private void UpdateLine()
    {
        if (_lineRenderer == null)
        {
            return;
        }

        _lineRenderer.useWorldSpace = true;
        _lineRenderer.positionCount = 2;

        Vector3 startPosition = _player.position + Vector3.up * 0.5f;
        Vector3 endPosition = _monster.position + Vector3.up * 0.5f;

        _lineRenderer.SetPosition(0, startPosition);
        _lineRenderer.SetPosition(1, endPosition);

        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
    }

    private void SetLineColor(Color color)
    {
        if (_lineRenderer == null)
        {
            return;
        }

        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }

    private void RestartGame()
    {
        _player.position = _playerStartPosition;
        _monster.position = _monsterStartPosition;

        _winCount = 0;
        _loseCount = 0;
        _drawCount = 0;

        _isGameOver = false;
        _resultMessage = "게임 재시작! 1: 가위 / 2: 바위 / 3: 보";

        SetLineColor(Color.white);
        UpdateDistance();

        Debug.Log("==============================");
        Debug.Log("게임을 재시작했습니다.");
        Debug.Log("==============================");
    }

    private void PrintScore()
    {
        Debug.Log(
            $"최종 스코어 - 승 : {_winCount} / " +
            $"패 : {_loseCount} / 무승부 : {_drawCount}"
        );

        Debug.Log("==============================");
    }

    private string GetChoiceText(RPS choice)
    {
        switch (choice)
        {
            case RPS.Scissors:
                return "가위";

            case RPS.Rock:
                return "바위";

            case RPS.Paper:
                return "보";

            default:
                return "";
        }
    }

    private string GetResultText(Result result)
    {
        switch (result)
        {
            case Result.Win:
                return "승리";

            case Result.Lose:
                return "패배";

            case Result.Draw:
                return "무승부";

            default:
                return "";
        }
    }

    private void OnGUI()
    {
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 25;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = Color.black;

        GUIStyle textStyle = new GUIStyle(GUI.skin.label);
        textStyle.fontSize = 20;
        textStyle.normal.textColor = Color.black;

        GUI.Label(
            new Rect(20, 20, 500, 40),
            "Transform 가위바위보",
            titleStyle
        );

        GUI.Label(
            new Rect(20, 70, 500, 30),
            $"현재 거리 : {_currentDistance:F1}",
            textStyle
        );

        GUI.Label(
            new Rect(20, 105, 500, 30),
            $"승 : {_winCount} / 패 : {_loseCount} / 무 : {_drawCount}",
            textStyle
        );

        GUI.Label(
            new Rect(20, 140, 700, 30),
            _resultMessage,
            textStyle
        );

        GUI.Label(
            new Rect(20, 175, 500, 30),
            "1: 가위 / 2: 바위 / 3: 보 / R: 재시작",
            textStyle
        );
    }
}
