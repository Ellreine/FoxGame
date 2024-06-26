using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndTrigger : MonoBehaviour
{
    [SerializeField] private int nextLevelIndex; // индекс следующего уровня в Build Settings

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Сбрасываем состояние игрока перед загрузкой следующего уровня
            collision.gameObject.GetComponent<Health>().Reset();
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1; // Следующий уровень
        GameDataManager.SaveLocationData(nextLevelIndex, ""); // Сохраняем данные уровня без контрольной точки
        SceneManager.LoadScene(nextLevelIndex, LoadSceneMode.Single);
    }

}
