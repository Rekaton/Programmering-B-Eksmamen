using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void PlayTutorial()
    {
        SceneManager.LoadScene("TutorialLevel");
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
        Application.Quit();
    }
}