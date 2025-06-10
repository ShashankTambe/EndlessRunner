using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;

    [Header("Score UI")]
    [Tooltip("This single Text shows both label and value, separated by a newline.")]
    [SerializeField] private Text ScoreText;
    [SerializeField] private Text highScoreText;
    private int score = 0;
    private int highScore = 0;

    [Header("Player Reference")]
    [SerializeField] private SwerveMovement playerMovement;

    [Header("Death Screen UI")]
    [Tooltip("Drag the root DeathScreenPanel GameObject here")]
    [SerializeField] private GameObject deathScreenPanel;

    private void Awake()
    {
        // Singleton
        if (inst == null) inst = this;
        else { Destroy(gameObject); return; }

        // Hide death screen
        if (deathScreenPanel != null) deathScreenPanel.SetActive(false);

        // Load high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Update the text fields with label + newline + value
        if (ScoreText != null)
            ScoreText.text = $"SCORE\n{score}";
        if (highScoreText != null)
            highScoreText.text = $"HIGHSCORE\n{highScore}";

        Time.timeScale = 1f;
    }

    private void Start()
    {
        // In case you want to reset at start again
        if (ScoreText != null)
            ScoreText.text = $"SCORE\n{score}";
    }

    /// <summary>
    /// Call this whenever player collects a point.
    /// </summary>
    public void IncrementScore()
    {
        score++;

        // Update score display
        if (ScoreText != null)
            ScoreText.text = $"SCORE\n{score}";

        // Speed up player
        if (playerMovement != null)
            playerMovement.IncreaseSpeed(playerMovement.speedIncPerPoint);

        // Check high score
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();

            if (highScoreText != null)
                highScoreText.text = $"HIGHSCORE\n{highScore}";
        }
    }

    public void OnPlayerDeath()
    {
        if (playerMovement != null)
            playerMovement.enabled = false;
        if (deathScreenPanel != null)
            deathScreenPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnRetryButton()
    {
        if (deathScreenPanel != null)
            deathScreenPanel.SetActive(false);
        Time.timeScale = 1f;
        score = 0;
        if (ScoreText != null)
            ScoreText.text = $"SCORE\n{score}";
        if (playerMovement != null)
        {
            playerMovement.ResetToStart();
            playerMovement.enabled = true;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMenuButton()
    {
        if (deathScreenPanel != null)
            deathScreenPanel.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
