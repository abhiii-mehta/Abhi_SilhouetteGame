using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonController : MonoBehaviour
{
    // Scene names to load (set these in the Inspector)
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private string creditsSceneName = "CreditsScene";
    [SerializeField] private string settingsSceneName = "SettingsScene";

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

    public void OnExitButtonClicked()
    {
        QuitGame();
    }

    private void LoadScene(string sceneName)
    {
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