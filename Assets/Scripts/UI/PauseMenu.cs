using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Paneler")]
    public GameObject pauseMenuUI;
    public GameObject levelCompletedUI;

    [Header("Referencer")]
    // Vi skal bruge health-scriptet for at kunne respawne via knappen
    public PlayerHealthDamageRespawn playerHealth;

    // En statisk variabel, så andre scripts (f.eks. dit våben/dash) kan se, om spillet er pauset
    public static bool GameIsPaused = false;

    private void Start()
    {
        // Sørg for at menuerne er usynlige når banen starter
        pauseMenuUI.SetActive(false);
        if (levelCompletedUI != null) levelCompletedUI.SetActive(false);

        ResumeGame(); // Sikr at tiden kører
    }

    private void Update()
    {
        // Lyt efter ESC-knappen
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Tænd for tiden igen
        GameIsPaused = false;
    }

    private void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Frys tiden
        GameIsPaused = true;
    }

    public void RespawnAtCheckpoint()
    {
        ResumeGame(); // Start tiden igen
        if (playerHealth != null)
        {
            playerHealth.Respawn(); // Kald funktionen fra dit andet script
        }
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // VIGTIGT: Husk at sætte tiden i gang, inden du skifter scene!
        SceneManager.LoadScene("StartGame"); // Navnet på din start menu scene
    }

    // Denne kan kaldes fra et "Mål-streg" trigger-script, når spilleren rører mål
    public void ShowLevelCompleted()
    {
        levelCompletedUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}