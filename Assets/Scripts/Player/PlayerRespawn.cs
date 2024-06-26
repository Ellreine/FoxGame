using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpoint;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;

    private List<Transform> checkpoints = new List<Transform>();

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();

        // Load the saved data
        LoadCheckpointData();
    }

    private void Start()
    {
        // Find all checkpoints on the current level
        FindCheckpoints();

        // Load checkpoint data after checkpoints are found
        LoadCheckpointData();
    }

    public void RespawnCheck()
    {
        if (currentCheckpoint == null)
        {
            uiManager.GameOver();
            return;
        }

        playerHealth.Respawn(); // Restore player health and reset animation
        transform.position = currentCheckpoint.position; // Move player to checkpoint location

        // Move the camera to the checkpoint's room
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform;
            SoundManager.instance.PlaySound(checkpoint);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("activate");

            // Save checkpoint data
            GameDataManager.SaveLocationData(SceneManager.GetActiveScene().buildIndex, currentCheckpoint.name);
        }
    }

    private void FindCheckpoints()
    {
        checkpoints.Clear();
        GameObject[] checkpointObjects = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach (GameObject checkpointObject in checkpointObjects)
        {
            checkpoints.Add(checkpointObject.transform);
        }
    }

    private void LoadCheckpointData()
    {
        int savedLevel = GameDataManager.LoadLevel();
        string savedCheckpoint = GameDataManager.LoadCheckpoint();

        if (savedLevel == SceneManager.GetActiveScene().buildIndex && !string.IsNullOrEmpty(savedCheckpoint))
        {
            foreach (var checkpoint in checkpoints)
            {
                if (checkpoint.name == savedCheckpoint)
                {
                    currentCheckpoint = checkpoint;
                    transform.position = currentCheckpoint.position;
                    break;
                }
            }
        }
        else
        {
            // ≈сли данных нет, начнем с первой контрольной точки
            if (checkpoints.Count > 0)
            {
                currentCheckpoint = checkpoints[0];
                transform.position = currentCheckpoint.position;
            }
        }
    }
}
