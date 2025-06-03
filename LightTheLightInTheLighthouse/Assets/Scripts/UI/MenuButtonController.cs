using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonController : MonoBehaviour
{
    // Scene names to load (set these in the Inspector)
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private string creditsSceneName = "CreditsScene";
    [SerializeField] private string settingsSceneName = "SettingsScene";
    [SerializeField] private string menuSceneName = "MenuScene";


    // Button click methods
    public void OnStartButtonClicked()
    {
        LoadScene(gameSceneName);
    }

    public void OnCreditsButtonClicked()
    {
        LoadScene(creditsSceneName);
    }

    public void OnSettingsButtonClicked()
    {
        LoadScene(settingsSceneName);
    }

    public void OnMenuButtonClicked()
    {
        LoadScene(menuSceneName);
    }

    public void OnExitButtonClicked()
    {
        QuitGame();
    }

    private void LoadScene(string sceneName)
    {
        // Always reset time scale before loading a new scene
        Time.timeScale = 1f;

        // Optional: Add fade-out effect or loading screen here
        SceneManager.LoadScene(sceneName);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}