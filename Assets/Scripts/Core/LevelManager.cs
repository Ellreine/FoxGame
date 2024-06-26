using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] trapsAndMobs; 


    private void Start()
    {
        // Активируем все ловушки
        foreach (GameObject trapAndMobs in trapsAndMobs)
        {
            trapAndMobs.SetActive(true);
        }
    }
}
