using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour 
{

    [SerializeField] Text scoreText;
    int score = 0;

    private void OnEnable()
    {
        ResetScore();
        EnemyAI.OnEnemyDied += UpdateScore;
    }

    private void OnDisable()
    {
        EnemyAI.OnEnemyDied -= UpdateScore;
    }

    void UpdateScore()
    {
        score += 10;
        scoreText.text = score.ToString("D10");
    }

    void ResetScore()
    {
        score = 0;
        scoreText.text = score.ToString("D10");
    }

}
