using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    //Singelton
    public static GameTimer Instance { get; private set; }

    [Header("UI Referencer")]
    public TextMeshProUGUI hudTimerText;
    public TextMeshProUGUI winPanelTimerText;

    private float currentTime = 0f;
    private bool isFinished = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isFinished) return;

        currentTime += Time.deltaTime;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        float fractions = (currentTime % 1) * 100;

        hudTimerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fractions);
    }

    public void ShowFinalTime()
    {
        isFinished = true;

        if (winPanelTimerText != null && hudTimerText != null)
        {
            winPanelTimerText.text = "Din tid: " + hudTimerText.text;
            hudTimerText.gameObject.SetActive(false);
        }
    }
}