using UnityEngine;

public class Cameracontroller : MonoBehaviour
{
    public Transform container;
    public Vector3 offset = new Vector3(0, 0, -10f);

    void LateUpdate()
    {
        // Follow container position but maintain camera distance
        transform.position = container.position + offset;

        // Always look at the container
        transform.LookAt(container);
    }
}