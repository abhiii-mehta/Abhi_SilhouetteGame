using UnityEngine;

public class Cameracontroller : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    private float shakeDuration = 0f;
    private float shakeIntensity = 0.4f; // increased intensity
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;

        if (shakeDuration > 0)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
            shakeOffset.z = 0; // ensure Z doesn't change
            transform.position = targetPosition + shakeOffset;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            transform.position = targetPosition;
        }
    }
    public void StartShake(float duration = 1f, float intensity = 0.2f)
    {
        shakeDuration = duration;
        shakeIntensity = intensity;
        Debug.Log("CAMERA SHAKE STARTED");
    }

}
