using UnityEngine;
using System.Collections;
using UnityEditor.Rendering.LookDev;

public class PanicAttackManager : MonoBehaviour
{
    public EyesUIPlayer eyesUIPlayer;
    public Transform player;
    public LayerMask lightZoneLayer;
    public float timeBeforePanic = 5f;
    public float panicDuration = 7f;
    public GameObject loseGamePanel;
    public PlayerController playerController;

    private float darknessTimer = 0f;
    private bool isInDarkness = false;
    private bool panicTriggered = false;
    private Coroutine panicCoroutine = null;
    private bool hasLoggedDarknessEntry = false;
    public Cameracontroller cameraShake;
    public AudioSource panicAudio;

    void Update()
    {
        UpdateDarknessState();

        if (isInDarkness && !panicTriggered)
        {
            darknessTimer += Time.deltaTime;

            if (!hasLoggedDarknessEntry)
            {
                Debug.Log("Player entered darkness. Panic countdown started.");
                hasLoggedDarknessEntry = true;
            }

            if (darknessTimer >= timeBeforePanic)
            {
                TriggerPanicAttack();
            }
        }
        else if (!isInDarkness && !panicTriggered)
        {
            darknessTimer = 0f;
            hasLoggedDarknessEntry = false;
        }
    }

    void UpdateDarknessState()
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

        if (!isInDarkness && panicTriggered)
        {
            Debug.Log("Player escaped panic attack in time!");
            StopPanicAttack();
        }
    }

    void TriggerPanicAttack()
    {
        panicTriggered = true;
        Debug.Log("PANIC ATTACK TRIGGERED.");
        eyesUIPlayer.PlayEyesAnimation();

        if (panicCoroutine != null)
            StopCoroutine(panicCoroutine);
        if (panicAudio != null && !panicAudio.isPlaying)
            StartCoroutine(FadeAudioIn(panicAudio, 1f)); // 1 second fade-in

        panicCoroutine = StartCoroutine(PanicTimer());
        if (cameraShake != null)
            cameraShake.StartShake();

    }
    void StopPanicAttack()
    {
        if (panicCoroutine != null)
        {
            StopCoroutine(panicCoroutine);
            panicCoroutine = null;
        }

        eyesUIPlayer.HideEyes();
        panicTriggered = false;
        darknessTimer = 0f;
        hasLoggedDarknessEntry = false;

        if (cameraShake != null)
            cameraShake.StopShake();
        if (panicAudio != null && panicAudio.isPlaying)
            StartCoroutine(FadeAudioOut(panicAudio, 1f)); // 1 second fade-out

    }

    IEnumerator PanicTimer()
    {
        float timer = 0f;

        while (timer < panicDuration)
        {
            UpdateDarknessState();

            if (!isInDarkness)
            {
                Debug.Log("Panic attack averted during animation.");
                StopPanicAttack();
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Debug.Log("PLAYER HAD A PANIC ATTACK. GAME OVER.");
        eyesUIPlayer.HideEyes();
        if (panicAudio != null && panicAudio.isPlaying)
            panicAudio.Stop();

        if (cameraShake != null)
            cameraShake.StopShake();
        TriggerGameOver();
    }

    void TriggerGameOver()
    {
        if (loseGamePanel != null)
        {
            loseGamePanel.SetActive(true);
        }

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        Time.timeScale = 0f;
    }
    IEnumerator FadeAudioIn(AudioSource audioSource, float duration)
    {
        float timer = 0f;
        audioSource.volume = 0f;
        audioSource.Play();

        while (timer < duration)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 1f;
    }

    IEnumerator FadeAudioOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }

}
