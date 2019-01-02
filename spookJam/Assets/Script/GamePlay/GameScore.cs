using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Score listens for enemies dying and adds a fixed amount to the score outputing it to the Text Component.
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
        scoreText.text = score.ToString("D5");
    }

    void ResetScore()
    {
        score = 0;
        scoreText.text = score.ToString("D5");
    }

}
