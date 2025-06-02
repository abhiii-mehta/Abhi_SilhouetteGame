using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EyesUIPlayer : MonoBehaviour
{
    public Sprite[] eyeFrames;
    public float frameRate = 0.28f;

    private Image image;
    private Coroutine playRoutine;

    void Awake()
    {
        image = GetComponent<Image>();
        gameObject.SetActive(false);
    }

    public void PlayEyesAnimation()
    {
        if (playRoutine != null) StopCoroutine(playRoutine);
        gameObject.SetActive(true);
        playRoutine = StartCoroutine(PlayAnimation());
    }

    public void HideEyes()
    {
        if (playRoutine != null) StopCoroutine(playRoutine);
        gameObject.SetActive(false);
    }

    IEnumerator PlayAnimation()
    {
        for (int i = 0; i < eyeFrames.Length; i++)
        {
            image.sprite = eyeFrames[i];
            yield return new WaitForSeconds(frameRate);
        }

        gameObject.SetActive(false);
    }
}
