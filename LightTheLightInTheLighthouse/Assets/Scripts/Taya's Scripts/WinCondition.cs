using UnityEngine;
using System.Collections;

public class WinTrigger : MonoBehaviour
{
    [Header("Win Objects")]
    [SerializeField] private GameObject winLight; // Your 2D light object
    [SerializeField] private GameObject winPanel; // Your win panel UI
    [SerializeField] private float delayBeforeWinPanel = 3f; // Delay in seconds

    [Header("Player Settings")]
    [SerializeField] private string playerTag = "Player";

    private bool hasWon = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if player entered the trigger and we haven't already won
        if (other.CompareTag(playerTag) && !hasWon)
        {
            StartWinSequence();
        }
    }

    private void StartWinSequence()
    {
        hasWon = true;

        // Activate the light
        if (winLight != null)
        {
            winLight.SetActive(true);
        }

        // Start coroutine for delayed win panel
        StartCoroutine(ShowWinPanelAfterDelay());

        // Optional: Disable player controls
        DisablePlayerControls(true);
    }

    private IEnumerator ShowWinPanelAfterDelay()
    {
        // Wait for specified delay
        yield return new WaitForSeconds(delayBeforeWinPanel);

        // Activate win panel
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        // Stop game time
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        // Resume time before reloading
        Time.timeScale = 1f;

        // Reload current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    private void DisablePlayerControls(bool disable)
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = !disable;
            }
        }
    }
}