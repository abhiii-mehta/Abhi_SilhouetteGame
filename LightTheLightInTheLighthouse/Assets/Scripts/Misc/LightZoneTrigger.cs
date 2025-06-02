using UnityEngine;

public class LightZoneTrigger : MonoBehaviour
{
    public float timeToBreak = 5f;
    private float timer = 0f;
    private bool playerInside = false;
    private BreakableLight breakable;

    void Start()
    {
        breakable = GetComponentInParent<BreakableLight>();
    }

    void Update()
    {
        if (playerInside && breakable != null && breakable.canBeBroken)
        {
            timer += Time.deltaTime;

            if (timer >= timeToBreak)
            {
                Debug.Log("Player stayed too long — breaking light...");
                breakable.Break();
                timer = 0f;
            }
        }
        else
        {
            timer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            timer = 0f;
        }
    }
    public bool IsLightStillWorking()
    {
        BreakableLight breakable = GetComponentInParent<BreakableLight>();
        return breakable != null && breakable.lightComponent.enabled;
    }
}
