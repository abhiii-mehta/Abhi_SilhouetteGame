using UnityEngine;

public class ShadowLightReaction : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool inLightZone = false;
    private BreakableLight currentLightZone;

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (inLightZone && currentLightZone != null)
        {
            if (currentLightZone.lightComponent.enabled)
                sr.enabled = false;
            else
                sr.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LightZone"))
        {
            inLightZone = true;
            currentLightZone = other.GetComponentInParent<BreakableLight>();

            if (currentLightZone != null && currentLightZone.lightComponent.enabled)
                sr.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("LightZone"))
        {
            inLightZone = false;
            currentLightZone = null;
            sr.enabled = true;
        }
    }
}
