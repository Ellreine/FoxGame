using System.Collections;
using UnityEngine;

public class FireballManager : MonoBehaviour
{
    public static FireballManager Instance;

    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float waveInterval = 1f; // Интервал между волнами
    [SerializeField] private int wavesCount = 5; // Количество волн

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartFireballRain()
    {
        StartCoroutine(SpawnFireballWaves());
    }

    private IEnumerator SpawnFireballWaves()
    {
        for (int i = 0; i < wavesCount; i++)
        {
            SpawnFireballs();
            yield return new WaitForSeconds(waveInterval);
        }
    }

    public void SpawnFireballs()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            Instantiate(fireballPrefab, spawnPoint.position, Quaternion.Euler(0, 0, -90)).SetActive(true);
        }
    }
}
