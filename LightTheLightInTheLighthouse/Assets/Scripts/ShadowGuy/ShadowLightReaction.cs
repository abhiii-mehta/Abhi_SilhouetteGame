using UnityEngine;

public class ShadowLightReaction : MonoBehaviour
{
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LightZone"))
        {
            Debug.Log("Shadow entered light zone");
            sr.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("LightZone"))
        {
            Debug.Log("Shadow exited light zone");
            sr.enabled = true;
        }
    }
}
