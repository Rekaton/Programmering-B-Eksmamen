using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    public virtual void OnPlayerContact(GameObject player) { }
}