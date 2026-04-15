using UnityEngine;
using System.Collections.Generic;

public class ColorToggleManager : MonoBehaviour
{
    public static ColorToggleManager Instance;

    private List<ColorToggleObject> objects = new List<ColorToggleObject>();

    private string currentColor;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Register(ColorToggleObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("Tried to register a null object.");
            return;
        }

        if (objects == null)
        {
            objects = new List<ColorToggleObject>();
        }

        objects.Add(obj);
    }
    
    public void SetColor(string color)
    {
        currentColor = color;

        if (objects == null)
        {
            return;
        }

        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i] != null)
            {
                objects[i].SetActiveColor(color);
            }
        }
    }
}