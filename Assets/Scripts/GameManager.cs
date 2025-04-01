using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int score = 0;
    int highScore = 0;
    public static GameManager inst;
    [SerializeField] Text ScoreText;
    [SerializeField] Text highScoreText;
    [SerializeField] SwerveMovement playerMovement;

    public void IncrementScore()
    {
        score++;
        ScoreText.text = "Score : " +score;
        //inc player speed
        playerMovement.speed += playerMovement.speedIncperpoint;
        if(score > PlayerPrefs.GetInt("HighScore : ", 0))
        {
            PlayerPrefs.SetInt("HighScore : ", score);
            highScore = score;
        }
    }

    void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore : ", 0);
        highScoreText.text = "HighScore : " +highScore.ToString();
    }
}
