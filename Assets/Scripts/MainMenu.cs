using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Difficulty Selection UI")]
    [Tooltip("Drag your DifficultySelectorPanel (GameObject) here")]
    [SerializeField] private GameObject difficultySelectorPanel;

    [Header("Speed‐Increase Values (per coin)")]
    [Tooltip("Smallest increment for Novice difficulty")]
    [SerializeField] private float noviceSpeedInc = 0.005f;
    [Tooltip("Moderate increment for Amateur difficulty")]
    [SerializeField] private float amateurSpeedInc = 0.01f;
    [Tooltip("Highest increment for Experienced difficulty")]
    [SerializeField] private float experiencedSpeedInc = 0.02f;

    private void Awake()
    {
        // Make sure the difficulty panel is hidden at startup
        if (difficultySelectorPanel != null)
        {
            difficultySelectorPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("MainMenu: Difficulty Selector Panel reference is missing in Inspector.");
        }
    }

    /// <summary>
    /// Called by the Play button on the Main Menu.
    /// Instead of loading the game immediately, show the difficulty panel.
    /// </summary>
    public void PlayGame()
    {
        if (difficultySelectorPanel != null)
        {
            difficultySelectorPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Called by the Quit button on the Main Menu.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Called by the Back button (if you have one) to return to a previous menu.
    /// </summary>
    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    // ─── These three methods are hooked up to your three difficulty buttons ───

    /// <summary>
    /// Called when the player selects "Novice" difficulty.
    /// Saves the small speed‐increase value, then loads the next scene.
    /// </summary>
    public void SelectNovice()
    {
        PlayerPrefs.SetFloat("SpeedIncPerCoin", noviceSpeedInc);
        PlayerPrefs.Save();
        LoadGameLevel();
    }

    /// <summary>
    /// Called when the player selects "Amateur" difficulty.
    /// </summary>
    public void SelectAmateur()
    {
        PlayerPrefs.SetFloat("SpeedIncPerCoin", amateurSpeedInc);
        PlayerPrefs.Save();
        LoadGameLevel();
    }

    /// <summary>
    /// Called when the player selects "Experienced" difficulty.
    /// </summary>
    public void SelectExperienced()
    {
        PlayerPrefs.SetFloat("SpeedIncPerCoin", experiencedSpeedInc);
        PlayerPrefs.Save();
        LoadGameLevel();
    }

    /// <summary>
    /// Common helper: loads the next scene in the build index (your Infinite Level).
    /// </summary>
    private void LoadGameLevel()
    {
        // If you want to mark a reload flag for the level, do it here:
        PlayerPrefs.SetInt("ReloadedLevel", 1);
        PlayerPrefs.Save();

        // Now actually load the gameplay scene (assumed to be current index + 1)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
