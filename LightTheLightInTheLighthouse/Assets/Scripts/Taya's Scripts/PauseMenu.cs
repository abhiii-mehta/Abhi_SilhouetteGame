using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel; // Reference to your pause panel UI
    [SerializeField] private string playerTag = "Player"; // Should match your player tag

    private bool isPaused = false;

    private void Update()
    {
        // Toggle pause when ESC is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        // Activate the pause panel
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        // Stop the game time
        Time.timeScale = 0f;

        // Optional: Disable player controls
        DisablePlayerControls(true);
    }

    public void ResumeGame()
    {
        isPaused = false;

        // Deactivate the pause panel
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // Resume the game time
        Time.timeScale = 1f;

        // Optional: Re-enable player controls
        DisablePlayerControls(false);
    }

    public void RestartGame()
    {
        // Resume time before reloading
        Time.timeScale = 1f;

        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        // Always reset time scale before loading a new scene
        Time.timeScale = 1f;

        // Use MenuButtonController's method to load the menu
        FindObjectOfType<MenuButtonController>().OnMenuButtonClicked();
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