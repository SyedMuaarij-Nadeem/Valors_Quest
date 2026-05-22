using UnityEngine;

// =====================================================
//  HealthBridge.cs — FIXED
//
//  ATTACH TO: Player GameObject
//
//  FIX: Health.cs ka Die() function
//       pehle RestartGame() call karta tha
//       Ab hum Health component ko override karte hain
//       taake koi conflict na ho
// =====================================================
namespace RPGGame
{
    public class HealthBridge : MonoBehaviour
    {
        private Health playerHealth;
        private bool gameOverTriggered = false;

        void Start()
        {
            playerHealth = GetComponent<Health>();

            if (playerHealth == null)
                Debug.LogWarning("[HealthBridge] Health script nahi mili Player pe!");
        }

        void Update()
        {
            if (gameOverTriggered) return;
            if (playerHealth == null) return;

            if (playerHealth.currentHealth <= 0)
            {
                gameOverTriggered = true;

                // ✅ Health script disable karo — taake RestartGame() na chale
                playerHealth.enabled = false;

                // ✅ GameOver panel show karo
                if (LevelUIManager.Instance != null)
                    LevelUIManager.Instance.ShowGameOver();
                else
                    Debug.LogWarning("[HealthBridge] LevelUIManager.Instance nahi mila!");

                Debug.Log("[HealthBridge] GameOver triggered — Health disabled.");
            }
        }
    }
}