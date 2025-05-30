using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BreakableLight : MonoBehaviour
{
    public bool canBeBroken = true;
    public Light2D lightComponent;

    public void Break()
    {
        Debug.Log("Break() called on: " + gameObject.name);

        if (canBeBroken && lightComponent != null)
        {
            lightComponent.enabled = false;
            Debug.Log("Light has been broken!");
        }
        else
        {
            Debug.LogWarning("Cannot break: lightComponent is null or canBeBroken is false");
        }
    }
}
