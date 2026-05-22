using UnityEngine;

public class PlayerFallDetector : MonoBehaviour
{
    [Header("=== SETTINGS ===")]
    public float fallThreshold = -5f;

    [Header("=== UI ===")]
    public GameObject gameOverPanel;

    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return;

        if (transform.position.y < fallThreshold)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}