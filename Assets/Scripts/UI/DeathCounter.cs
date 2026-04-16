using UnityEngine;
using TMPro;

public class DeathCounter : MonoBehaviour
{
    public static DeathCounter Instance { get; private set; }

    [Header("UI Referencer")]
    public TextMeshProUGUI hudDeathText;
    public TextMeshProUGUI winPanelDeathText;

    private int deathCount = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateDisplay();
    }

    public void AddDeath()
    {
        deathCount++;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (hudDeathText != null)
        {
            hudDeathText.text = "Døde: " + deathCount;
        }
    }

    public void ShowFinalDeaths()
    {
        if (winPanelDeathText != null)
        {
            winPanelDeathText.text = "Antal dødsfald: " + deathCount;
            if (hudDeathText != null) hudDeathText.gameObject.SetActive(false);
        }
    }
}