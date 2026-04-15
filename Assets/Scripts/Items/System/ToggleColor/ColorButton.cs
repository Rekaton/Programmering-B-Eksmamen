using UnityEngine;

public class ColorButton : MonoBehaviour
{
    public string colorID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ColorToggleManager.Instance.SetColor(colorID);
        }
    }
}