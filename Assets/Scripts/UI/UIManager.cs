using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;

    [Header("MainMenu")]
    [SerializeField] private GameObject mainScreen;

    private void Awake()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
        if (pauseScreen != null)
            pauseScreen.SetActive(false);
        if (mainScreen != null)
            mainScreen.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(!pauseScreen.activeInHierarchy);
        }
    }

    #region Game Over
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    #endregion

    #region Pause
    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);

        if (status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }
    #endregion

    #region Main Menu
    public void StartGame(int level = 1)
    {
        GameDataManager.ResetData();
        SceneManager.LoadScene(level);
    }

    public void ContinueGame()
    {
        int currentLevel = GameDataManager.LoadLevel();
        string currentCheckpoint = GameDataManager.LoadCheckpoint();

        // «агружаем уровень
        SceneManager.LoadScene(currentLevel);

        // ≈сли есть контрольна€ точка, перемещаем игрока к ней (этот код может потребовать доработки в зависимости от вашей реализации контрольных точек)
        if (!string.IsNullOrEmpty(currentCheckpoint))
        {
            // ƒополните этот код, чтобы найти контрольную точку в сцене и переместить игрока к ней
        }
    }

    #endregion
}
