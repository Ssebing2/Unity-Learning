using System.Collections.Generic;
using UnityEngine;

public class PatternGame : MonoBehaviour
{
    private enum GameState
    {
        ShowingPattern,    // 컴퓨터 패턴 표시 중
        PlayerInput,       // 플레이어 입력 중
        RoundResult,       // 라운드 결과 표시 중
        GameOver           // 게임 종료
    }

    [Header("큐브 색상")]
    [SerializeField] private Color _defaultColor = Color.gray;
    [SerializeField] private Color _redColor = Color.red;
    [SerializeField] private Color _greenColor = Color.green;
    [SerializeField] private Color _blueColor = Color.blue;
    [SerializeField] private Color _yellowColor = Color.yellow;

    [Header("시간 설정")]
    [SerializeField] private float _colorInterval = 0.2f;
    [SerializeField] private float _resultDelay = 1.5f;

    // 큐브 Renderer
    private Renderer _renderer;

    // 현재 게임 상태
    private GameState _gameState;

    // 컴퓨터가 만든 패턴
    private int[] _computerPattern = new int[4];

    // 현재 라운드
    private int _currentRound = 1;

    // 플레이어가 현재 몇 번째 색을 입력 중인지
    private int _playerInputIndex;

    // 컴퓨터가 현재 몇 번째 패턴을 보여주는지
    private int _patternIndex;

    // 타이머
    private float _timer;

    // 현재 색상을 보여주는 중인지 확인
    private bool _isShowingColor;

    // 라운드 성공 여부
    private bool _isRoundSuccess;

    // 라운드별 색상 유지 시간
    private readonly float[] _roundShowTimes =
    {
        1.5f,  // 1라운드
        1.0f,  // 2라운드
        0.5f   // 3라운드
    };

    private void Start()
    {
        _renderer = GetComponent<Renderer>();

        StartGame();
    }

    private void Update()
    {
        switch (_gameState)
        {
            case GameState.ShowingPattern:
                UpdateShowingPattern();
                break;

            case GameState.PlayerInput:
                UpdatePlayerInput();
                break;

            case GameState.RoundResult:
                UpdateRoundResult();
                break;

            case GameState.GameOver:
                UpdateGameOver();
                break;
        }
    }

    private void StartGame()
    {
        _currentRound = 1;

        Debug.Log("==============================");
        Debug.Log("패턴 기억 게임 시작!");
        Debug.Log("1번: 빨강 / 2번: 초록 / 3번: 파랑 / 4번: 노랑");
        Debug.Log("==============================");

        StartRound();
    }

    private void StartRound()
    {
        _renderer.material.color = _defaultColor;

        _patternIndex = 0;
        _playerInputIndex = 0;
        _timer = 0f;
        _isShowingColor = false;

        CreateRandomPattern();

        _gameState = GameState.ShowingPattern;

        Debug.Log($"========== {_currentRound} 라운드 ==========");
        Debug.Log("컴퓨터 패턴을 확인하세요.");
    }

    private void CreateRandomPattern()
    {
        List<int> colorNumbers = new List<int>()
        {
            0,  // 빨강
            1,  // 초록
            2,  // 파랑
            3   // 노랑
        };

        // 리스트 섞기
        for (int i = 0; i < colorNumbers.Count; i++)
        {
            int randomIndex = Random.Range(i, colorNumbers.Count);

            int temp = colorNumbers[i];
            colorNumbers[i] = colorNumbers[randomIndex];
            colorNumbers[randomIndex] = temp;
        }

        // 섞은 값을 컴퓨터 패턴에 저장
        for (int i = 0; i < _computerPattern.Length; i++)
        {
            _computerPattern[i] = colorNumbers[i];
        }
    }

    private void UpdateShowingPattern()
    {
        _timer += Time.deltaTime;

        // 현재 패턴을 모두 보여줬다면 플레이어 입력 시작
        if (_patternIndex >= _computerPattern.Length)
        {
            _renderer.material.color = _defaultColor;
            _playerInputIndex = 0;
            _gameState = GameState.PlayerInput;

            Debug.Log("패턴 표시 완료!");
            Debug.Log("숫자키 1, 2, 3, 4를 사용하여 입력하세요.");

            return;
        }

        // 아직 색상을 보여주고 있지 않은 경우
        if (!_isShowingColor)
        {
            ShowColor(_computerPattern[_patternIndex]);

            _isShowingColor = true;
            _timer = 0f;
        }
        // 라운드에 맞는 시간이 지나면 기본색으로 변경
        else if (_timer >= GetCurrentShowTime())
        {
            _renderer.material.color = _defaultColor;

            _isShowingColor = false;
            _timer = -_colorInterval;

            _patternIndex++;
        }
    }

    // 현재 라운드의 색상 유지 시간 반환
    private float GetCurrentShowTime()
    {
        return _roundShowTimes[_currentRound - 1];
    }

    // 플레이어 입력 처리
    private void UpdatePlayerInput()
    {
        int inputColor = -1;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inputColor = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inputColor = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inputColor = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            inputColor = 3;
        }

        // 아무 키도 입력하지 않았다면 종료
        if (inputColor == -1)
        {
            return;
        }

        // 플레이어가 누른 색상으로 큐브 변경
        ShowColor(inputColor);

        Debug.Log(
            $"{_playerInputIndex + 1}번째 입력: {GetColorName(inputColor)}"
        );

        // 현재 입력이 컴퓨터 패턴과 다른 경우
        if (inputColor != _computerPattern[_playerInputIndex])
        {
            FailRound();
            return;
        }

        _playerInputIndex++;

        // 4개를 모두 입력했다면 라운드 성공
        if (_playerInputIndex >= _computerPattern.Length)
        {
            SuccessRound();
        }
    }

    // 큐브 색상 표시
    private void ShowColor(int colorNumber)
    {
        switch (colorNumber)
        {
            case 0:
                _renderer.material.color = _redColor;
                break;

            case 1:
                _renderer.material.color = _greenColor;
                break;

            case 2:
                _renderer.material.color = _blueColor;
                break;

            case 3:
                _renderer.material.color = _yellowColor;
                break;
        }
    }

    // 라운드 성공
    private void SuccessRound()
    {
        _renderer.material.color = _greenColor;

        _isRoundSuccess = true;
        _timer = 0f;
        _gameState = GameState.RoundResult;

        Debug.Log($"{_currentRound}라운드 성공!");
    }

    // 라운드 실패
    private void FailRound()
    {
        _renderer.material.color = _redColor;

        _isRoundSuccess = false;
        _gameState = GameState.GameOver;

        Debug.Log($"{_currentRound}라운드 실패!");
        Debug.Log($"정답은 {GetPatternText()}");
        Debug.Log("R키: 재시작 / ESC키: 게임 종료");
    }

    // 라운드 결과 처리
    private void UpdateRoundResult()
    {
        _timer += Time.deltaTime;

        if (_timer < _resultDelay)
        {
            return;
        }

        if (_isRoundSuccess && _currentRound >= 3)
        {
            _renderer.material.color = _greenColor;
            _gameState = GameState.GameOver;

            Debug.Log("==============================");
            Debug.Log("모든 라운드를 성공했습니다!");
            Debug.Log("R키: 재시작 / ESC키: 게임 종료");
            Debug.Log("==============================");
        }
        else
        {
            _currentRound++;
            StartRound();
        }
    }

    private void UpdateGameOver()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    private void QuitGame()
    {
        Debug.Log("게임을 종료합니다.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private string GetColorName(int colorNumber)
    {
        switch (colorNumber)
        {
            case 0:
                return "빨강";

            case 1:
                return "초록";

            case 2:
                return "파랑";

            case 3:
                return "노랑";

            default:
                return "알 수 없음";
        }
    }

    private string GetPatternText()
    {
        string patternText = "";

        for (int i = 0; i < _computerPattern.Length; i++)
        {
            patternText += GetColorName(_computerPattern[i]);

            if (i < _computerPattern.Length - 1)
            {
                patternText += " → ";
            }
        }

        return patternText;
    }
}
