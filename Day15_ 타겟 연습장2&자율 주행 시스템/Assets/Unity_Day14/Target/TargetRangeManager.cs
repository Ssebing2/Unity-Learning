using UnityEngine;

public class TargetRangeManager : MonoBehaviour
{
    private int _hitCount;
    private int _score;


    public void AddScore(int score)
    {
        _hitCount++;
        _score += score;

        Debug.Log(
            $"¸ÂÃá È½¼ö : {_hitCount} / ÇöÀç Á¡¼ö : {_score}"
        );
    }


    private void OnGUI()
    {
        GUI.color = new Color(0f, 0f, 0f, 0.8f);
        GUI.Box(new Rect(530, 10, 150, 60), "");
       
        GUI.color = Color.white;
        GUI.Label(
            new Rect(550, 20, 200, 30),
            $"¸ÂÃá È½¼ö : {_hitCount}"
        );

        GUI.Label(
            new Rect(550, 40, 200, 30),
            $"ÇöÀç Á¡¼ö : {_score}"
        );
    }
}