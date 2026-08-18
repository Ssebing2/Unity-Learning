using UnityEngine;

public class ShootingGameManager : MonoBehaviour
{
    [Header("Game Setting")]
    [SerializeField] private float _gameTime = 30f;
    [SerializeField] private int _goalScore = 10;

    [Header("Ray Setting")]
    [SerializeField] private float _rayDistance = 100f;

    private int _score;
    private float _currentTime;
    private bool _isGameOver;

    private void Start()
    {
        _currentTime = _gameTime;
    }

    private void Update()
    {
        if (_isGameOver)
        {
            return;
        }

        UpdateTimer();
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _rayDistance))
            {
                Debug.Log($"Ray 맞음 : {hit.collider.name}");

                Target2 target =
                    hit.collider.GetComponentInParent<Target2>();

                if (target != null)
                {
                    target.Hit();

                    _score++;

                    Debug.Log($"현재 점수 : {_score}");
                }
                else
                {
                    Debug.Log("맞긴 맞았는데 Target 스크립트가 없음");
                }
            }
            else
            {
                Debug.Log("Ray가 아무것도 못 맞춤");
            }

            Debug.DrawRay(ray.origin,ray.direction * _rayDistance,Color.red,2f);
        }
    }

    private void UpdateTimer()
    {
        _currentTime -= Time.deltaTime;

        if (_currentTime <= 0f)
        {
            _currentTime = 0f;

            GameOver();
        }
    }

    private void CheckGoalScore()
    {
        if (_score >= _goalScore)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        if (_isGameOver)
        {
            return;
        }

        _isGameOver = true;

        Debug.Log("게임 종료!");
        Debug.Log($"최종 점수 : {_score}");
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();

        style.fontSize = 30;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(20, 20, 300, 50),$"Score : {_score} / {_goalScore}",style);

        GUI.Label(new Rect(20, 60, 300, 50),$"Time : {_currentTime:F1}",style);

        if (_isGameOver)
        {
            GUIStyle gameOverStyle = new GUIStyle();

            gameOverStyle.fontSize = 50;
            gameOverStyle.alignment = TextAnchor.MiddleCenter;
            gameOverStyle.normal.textColor = Color.red;

            GUI.Label(new Rect(Screen.width / 2 - 250,Screen.height / 2 - 100,500,80),"GAME OVER", gameOverStyle);

            GUIStyle scoreStyle = new GUIStyle();

            scoreStyle.fontSize = 35;
            scoreStyle.alignment = TextAnchor.MiddleCenter;
            scoreStyle.normal.textColor = Color.white;

            GUI.Label(new Rect( Screen.width / 2 - 250, Screen.height / 2, 500, 60),$"Final Score : {_score}",scoreStyle);
        }
    }
}