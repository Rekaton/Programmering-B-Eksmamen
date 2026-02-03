using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    public Rigidbody2D targetRb;

    [Header("Camera Settings")]
    public float lookAheadDistance = 3f;
    public float smoothTime = 0.2f;
    public float yPositionMod = 4f;

    private Vector3 velocity = Vector3.zero;
    public float shakeDuration = 5f;
    public float shakeMagnitude = 5f;
    private Vector3 originalPos;
    private float shakeTimeRemaining = 0f;

    void Update()
    {
        if (!target || !targetRb)
        {
            return;
        }

        float lookDirectionX = Mathf.Sign(targetRb.linearVelocity.x);
        float lookAheadX;

        if (Mathf.Abs(targetRb.linearVelocity.x) > 0.1f)
        {
            lookAheadX = lookDirectionX * lookAheadDistance;
        }
        else
        {
            lookAheadX = 0f;
        }

        Vector3 targetPos = new Vector3(target.position.x + lookAheadX, target.position.y + yPositionMod, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        if (Input.GetKeyDown(KeyCode.X))
        {
            ShakeCamera(shakeDuration,shakeMagnitude);
        }

        if (shakeTimeRemaining > 0)
        {
            transform.position += (Vector3)Random.insideUnitCircle * shakeMagnitude;
            shakeTimeRemaining -= Time.deltaTime;
        }
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        shakeTimeRemaining = duration;
        shakeMagnitude = magnitude;
    }
}
