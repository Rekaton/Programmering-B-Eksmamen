using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [Header("Referencer")]
    public GameObject levelCompletedUI;

    private void Start()
    {
        if (levelCompletedUI != null)
        {
            levelCompletedUI.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (levelCompletedUI != null)
            {
                levelCompletedUI.SetActive(true);
                Time.timeScale = 0f;
                PauseMenu.GameIsPaused = true;
            }
        }
    }
}