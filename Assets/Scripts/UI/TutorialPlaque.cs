using UnityEngine;

public class TutorialPlaque : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject tutorialPanel;
    private void Start()
    {

        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (tutorialPanel != null)
            {
                tutorialPanel.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (tutorialPanel != null)
            {
                tutorialPanel.SetActive(false);
            }
        }
    }
}