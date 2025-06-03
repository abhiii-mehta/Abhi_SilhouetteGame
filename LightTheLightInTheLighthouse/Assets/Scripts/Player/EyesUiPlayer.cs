using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EyesUIPlayer : MonoBehaviour
{
    public Sprite[] eyeFrames;
    public float frameRate = 0.28f;

    private Image image;
    private Coroutine playRoutine;

    public Cameracontroller cameraController;  //  Reference to camera controller

    void Awake()
    {
        image = GetComponent<Image>();
        gameObject.SetActive(false);
    }

    public void PlayEyesAnimation()
    {
        if (playRoutine != null) StopCoroutine(playRoutine);

        gameObject.SetActive(true);

        if (cameraController != null)
            cameraController.StartShake(eyeFrames.Length * frameRate, 0.4f);  // Shake while animation plays

        playRoutine = StartCoroutine(PlayAnimation());
    }

    public void HideEyes()
    {
        if (playRoutine != null) StopCoroutine(playRoutine);

        gameObject.SetActive(false);

        if (cameraController != null)
            cameraController.StartShake(0f, 0f);  //  Stop shaking immediately
    }

    IEnumerator PlayAnimation()
    {
        for (int i = 0; i < eyeFrames.Length; i++)
        {
            image.sprite = eyeFrames[i];
            yield return new WaitForSeconds(frameRate);
        }

        gameObject.SetActive(false);

        if (cameraController != null)
            cameraController.StartShake(0f, 0f);  //  Stop shaking after animation finishes
    }
}
