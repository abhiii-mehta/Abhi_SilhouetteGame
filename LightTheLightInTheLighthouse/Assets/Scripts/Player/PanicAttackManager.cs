using UnityEngine;

public class PanicAttackManager : MonoBehaviour
{
    public EyesUIPlayer eyesUIPlayer;
    public Transform player;
    public LayerMask lightZoneLayer;

    public float timeBeforePanic = 5f;
    private float darknessTimer = 0f;
    private bool isInDarkness = false;
    private bool panicTriggered = false;

    void Update()
    {
        CheckIfInLight();

        if (isInDarkness && !panicTriggered)
        {
            darknessTimer += Time.deltaTime;

            if (Mathf.Approximately(darknessTimer, 0f))
            {
                Debug.Log("Player entered darkness. Panic countdown started.");
            }

            if (darknessTimer >= timeBeforePanic)
            {
                TriggerPanicAttack();
            }
        }
        else
        {
            darknessTimer = 0f;
        }
    }

    void CheckIfInLight()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(player.position, 0.1f, lightZoneLayer);
        bool isInValidLight = false;

        foreach (Collider2D hit in hits)
        {
            LightZoneTrigger lightZone = hit.GetComponent<LightZoneTrigger>();
            if (lightZone != null && lightZone.IsLightStillWorking())
            {
                isInValidLight = true;
                break;
            }
        }

        bool wasInDarkness = isInDarkness;
        isInDarkness = !isInValidLight;

        if (!isInDarkness && wasInDarkness)
        {
            Debug.Log("Player entered a working light. Panic countdown reset.");
        }

        if (!isInDarkness && panicTriggered)
        {
            Debug.Log("Player escaped panic attack in time!");
            eyesUIPlayer.HideEyes();
            panicTriggered = false;
        }
    }

    void TriggerPanicAttack()
    {
        panicTriggered = true;
        Debug.Log("PLAYER HAD A PANIC ATTACK.");
        eyesUIPlayer.PlayEyesAnimation();
    }
}
