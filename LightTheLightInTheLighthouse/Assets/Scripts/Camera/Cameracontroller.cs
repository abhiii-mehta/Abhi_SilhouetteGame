using UnityEngine;

public class Cameracontroller : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    private float shakeDuration = 0f;
    private float shakeIntensity = 0.4f;
    private Vector3 initialPosition;

    private bool isShaking = false;

    void Start()
    {
        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;

        if (isShaking && shakeDuration > 0)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
            shakeOffset.z = 0;
            transform.position = targetPosition + shakeOffset;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            isShaking = false;
            transform.position = targetPosition;
        }
    }

    public void StartShake(float duration = 1f, float intensity = 0.2f)
    {
        shakeDuration = duration;
        shakeIntensity = intensity;
        isShaking = true;
        Debug.Log("CAMERA SHAKE STARTED");
    }

    public void StopShake()
    {
        shakeDuration = 0f;
        isShaking = false;
        Debug.Log("CAMERA SHAKE STOPPED");
    }
}
