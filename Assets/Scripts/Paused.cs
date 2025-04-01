using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paused : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject pauseMenuButton;

    public void Resume()
    {
        pauseMenuButton.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    public void Pause()
    {
        pauseMenuButton.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }
}
