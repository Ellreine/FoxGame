using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpoint;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();

        // Load the saved data
        LoadCheckpointData();
    }

    public void RespawnCheck()
    {
        if (currentCheckpoint == null)
        {
            uiManager.GameOver();
            return;
        }

        playerHealth.Respawn(); //Restore player health and reset animation
        transform.position = currentCheckpoint.position; //Move player to checkpoint location

        //Move the camera to the checkpoint's room
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;
            SoundManager.instance.PlaySound(checkpoint);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("activate");

            // Save checkpoint data
            SaveCheckpointData();
        }
    }

    private void SaveCheckpointData()
    {
        PlayerPrefs.SetFloat("PlayerX", currentCheckpoint.position.x);
        PlayerPrefs.SetFloat("PlayerY", currentCheckpoint.position.y);
        PlayerPrefs.SetFloat("PlayerHealth", playerHealth.currentHealth);
        PlayerPrefs.Save();
    }

    private void LoadCheckpointData()
    {
        if (PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            transform.position = new Vector2(x, y);

            float health = PlayerPrefs.GetFloat("PlayerHealth", playerHealth.startingHealth);
            playerHealth.SetHealth(health);
        }
    }
}
