using UnityEngine;

public class FallDetector : MonoBehaviour
{
    [SerializeField] private float fallThreshold = -10f; // Y position where player is considered fallen
    [SerializeField] private GameObject loseGamePanel; // Reference to your lose game panel UI
    [SerializeField] private string playerTag = "Player"; // Tag of your player object

    private bool gameOver = false;

    private void Update()
    {
        // Check if player exists and game isn't already over
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null && !gameOver)
        {
            // Check if player has fallen below the threshold
            if (player.transform.position.y < fallThreshold)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        gameOver = true;

        // Activate the lose game panel
        if (loseGamePanel != null)
        {
            loseGamePanel.SetActive(true);
        }

        // Stop the game (pause time)
        Time.timeScale = 0f;

        // Optional: Disable player controls
        PlayerController playerController = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    // Call this function to reset the game if you have a restart button
    public void RestartGame()
    {
        gameOver = false;
        Time.timeScale = 1f;

        if (loseGamePanel != null)
        {
            loseGamePanel.SetActive(false);
        }
    }
}