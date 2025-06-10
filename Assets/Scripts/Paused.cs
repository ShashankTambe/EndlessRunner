using UnityEngine;

public class Paused : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject pauseMenuPanel;

    // Drag your player (or any GameObject with the SwerveMovement component) into this slot:
    [Tooltip("Drag your player (with SwerveMovement) here so it can be disabled while paused.")]
    public SwerveMovement playerMovement; 

    /// <summary>
    /// Called (for example) by your Resume button in the Pause Menu UI.
    /// </summary>
    public void Resume()
    {
        // 1) Hide pause menu UI
        pauseMenuPanel.SetActive(false);

        // 2) Un‐freeze time
        Time.timeScale = 1f;

        // 3) Un‐pause game state
        GamePaused = false;

        // 4) Re‐enable the player’s movement script so input works again
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }

    /// <summary>
    /// Called (for example) by your Pause button or when the player hits Esc.
    /// </summary>
    public void Pause()
    {
        // 1) Show pause menu UI
        pauseMenuPanel.SetActive(true);

        // 2) Freeze time
        Time.timeScale = 0f;

        // 3) Mark game as paused
        GamePaused = true;

        // 4) Disable the player’s movement script so no input can move the character
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    }
}
