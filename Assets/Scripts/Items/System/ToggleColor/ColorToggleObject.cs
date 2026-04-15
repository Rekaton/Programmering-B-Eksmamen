using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorToggleObject : MonoBehaviour
{
    public string colorID;

    [Header("Visual Settings")]
    [Range(0f, 1f)] public float inactiveAlpha = 0.2f;
    public float fadeSpeed = 5f;

    private SpriteRenderer sr;
    private Collider2D col;

    private float targetAlpha;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        if (ColorToggleManager.Instance != null)
        {
            ColorToggleManager.Instance.Register(this);
        }
        else
        {
            Debug.LogError("ColorToggleManager not found in scene!");
        }
    }

    void Update()
    {
        if (sr == null)
        {
            return;
        }

        Color c = sr.color;

        float newAlpha = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
        c.a = newAlpha;

        sr.color = c;
    }
    
    public void SetActiveColor(string activeColor)
    {
        bool isActive;

        if (colorID == activeColor)
        {
            isActive = true;
        }
        else
        {
            isActive = false;
        }

        SetVisualState(isActive);
    }
    
    private void SetVisualState(bool active)
    {
        if (active)
        {
            targetAlpha = 1f;

            if (col != null)
            {
                col.enabled = true;
            }
        }
        else
        {
            targetAlpha = inactiveAlpha;

            if (col != null)
            {
                col.enabled = false;
            }
        }
    }
}