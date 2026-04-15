using UnityEngine;
using UnityEngine.SceneManagement; // Vigtigt for at skifte baner

public class StartGame : MonoBehaviour
{
    // Funktioner der kaldes af dine UI knapper
    public void PlayTutorial()
    {
        SceneManager.LoadScene("TutorialLevel"); // Skriv det præcise navn på din scene
    }

    public void PlayLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void PlayLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void PlayLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void QuitGame()
    {
        Debug.Log("Spillet lukkes!");
        Application.Quit(); // Lukker spillet (virker kun når det er bygget, ikke i Editoren)
    }
}