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
        Collider2D hit = Physics2D.OverlapCircle(player.position, 0.1f, lightZoneLayer);
        bool wasInDarkness = isInDarkness;

        isInDarkness = (hit == null);

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
