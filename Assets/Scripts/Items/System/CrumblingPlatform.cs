using UnityEngine;
using System.Collections;

public class CrumblingPlatform : Platform
{
    public float delay = 1f;
    private bool triggered = false;

    public override void OnPlayerContact(GameObject player)
    {
        if (!triggered)
        {
            triggered = true;
            StartCoroutine(Crumble());
        }
    }

    private IEnumerator Crumble()
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}