using UnityEngine;
using UnityEngine.SceneManagement;

// =====================================================
//  KeysBridge.cs
//
//  ATTACH TO: Level1Controller GameObject
//  (same object jis par Level1Controller + LevelUIManager hai)
//
//  KAAM:
//  Level1Controller.cs ko bilkul touch nahi karta
//  Sirf keys count monitor karta hai
//  Jab saari keys collect ho → LevelUIManager.ShowVictory()
//
//  Level1Controller.cs mein koi change NAHI
//
//  INSPECTOR MEIN:
//  Total Keys Required → scene mein jitni keys hain utna likho (default 3)
// =====================================================
namespace RPGGame
{
    public class KeysBridge : MonoBehaviour
    {
        [Header("=== KEYS SETTINGS ===")]
        [Tooltip("Scene mein total kitni keys hain — Level1Controller se same rakho")]
        public int totalKeysRequired = 3;

        private Level1Controller levelController;
        private bool victoryTriggered = false;

        // Level1Controller ka keys field private hai
        // Isliye hum KeyPickup objects count karke track karte hain
        private int keysInScene      = 0;
        private int keysCollected    = 0;

        void Start()
        {
            levelController = GetComponent<Level1Controller>();

            if (levelController == null)
                Debug.LogWarning("[KeysBridge] Level1Controller nahi mili is GameObject pe!");

            // Scene mein saari keys count karo shuru mein
            keysInScene = FindObjectsByType<KeyPickup>(FindObjectsSortMode.None).Length;

            if (keysInScene == 0)
                Debug.LogWarning("[KeysBridge] Scene mein koi KeyPickup nahi mila!");

            Debug.Log("[KeysBridge] Keys in scene: " + keysInScene);
        }

        // KeyPickup.cs is method ko call karega
        // Ya hum Update mein remaining keys check karte hain
        void Update()
        {
            if (victoryTriggered) return;

            // Scene mein kitni keys baqi hain check karo
            int remaining = FindObjectsByType<KeyPickup>(FindObjectsSortMode.None).Length;
            keysCollected = keysInScene - remaining;

            if (keysInScene > 0 && remaining == 0)
            {
                victoryTriggered = true;

                if (LevelUIManager.Instance != null)
                    LevelUIManager.Instance.ShowVictory();
                else
                    Debug.LogWarning("[KeysBridge] LevelUIManager.Instance nahi mila!");
            }
        }
    }
}
