using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Paneler")]
    public GameObject pauseMenuUI;

    [Header("Referencer")]
    public PlayerHealthDamageRespawn playerHealth;
    public static bool GameIsPaused = false;
    private void Start()
    {
        pauseMenuUI.SetActive(false);
        ResumeGame();
    }
    private void Update()
    {
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
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    private void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void RespawnAtCheckpoint()
    {
        ResumeGame();
        if (playerHealth != null)
        {
            playerHealth.Respawn();

        }   
    }
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("StartGame");
    }
}