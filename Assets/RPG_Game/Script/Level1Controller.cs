
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace RPGGame
{
    /// <summary>
    /// Attach to the "Level1Controller" GameObject already in your Canvas hierarchy.
    /// Wire all references in the Inspector exactly as labelled below.
    /// </summary>
    public class Level1Controller : MonoBehaviour
    {
        // ---------------------------------------------
        //  INSPECTOR REFERENCES
        // ---------------------------------------------

        [Header("--- Timer ---")]
        [Tooltip("Drag the 'Timer' TMP object from your Canvas here")]
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private float levelTimeLimit = 180f;   // 3:00 for Level 1
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color urgentColor = Color.red; // < 30s

        [Header("--- Keys ---")]
        [Tooltip("Drag the 'Keys' TMP object from your Canvas here")]
        [SerializeField] private TextMeshProUGUI keysText;
        [SerializeField] private int totalKeysRequired = 3;     // Set to 3 keys

        [Header("--- UI Panels ---")]
        [Tooltip("Drag your VictoryPanel GameObject from the hierarchy here")]
        [SerializeField] private GameObject victoryPanel;       // Victory Panel ka reference

        public AudioClip victorySound; // NAYA

        // ---------------------------------------------
        //  PRIVATE STATE
        // ---------------------------------------------

        private float m_TimeRemaining;
        private int m_KeysCollected = 0;
        private bool m_LevelEnded = false;

        // ---------------------------------------------
        //  START
        // ---------------------------------------------

        void Start()
        {
            m_TimeRemaining = levelTimeLimit;

            // Shuru mein victory panel ko band rkhna hai
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(false);
            }

            RefreshAllUI();
        }

        // ---------------------------------------------
        //  UPDATE
        // ---------------------------------------------

        void Update()
        {
            if (m_LevelEnded) return;

            TickTimer(); // Timer har frame chalega
        }

        // ---------------------------------------------
        //  TIMER
        // ---------------------------------------------

        void TickTimer()
        {
            m_TimeRemaining -= Time.deltaTime;

            if (m_TimeRemaining <= 0f)
            {
                m_TimeRemaining = 0f;
                RefreshTimerText();
                TimeOutFail(); // Time khatam hone par direct restart ya main menu handling
                return;
            }

            RefreshTimerText();
        }

        void RefreshTimerText()
        {
            if (timerText == null) return;

            int minutes = Mathf.FloorToInt(m_TimeRemaining / 60f);
            int seconds = Mathf.FloorToInt(m_TimeRemaining % 60f);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            timerText.color = (m_TimeRemaining <= 30f) ? urgentColor : normalColor;
        }

        // ---------------------------------------------
        //  KEY COLLECTION
        // ---------------------------------------------

        public void OnKeyCollected()
        {
            if (m_LevelEnded) return;

            m_KeysCollected = Mathf.Min(m_KeysCollected + 1, totalKeysRequired);
            RefreshKeyText();

            // Agar saari keys (3/3) collect ho jayein to Victory state trigger karein
            if (m_KeysCollected >= totalKeysRequired)
            {
                WinLevel();
            }
        }

        void RefreshKeyText()
        {
            if (keysText == null) return;
            keysText.text = $"{m_KeysCollected}/{totalKeysRequired}";
        }

        // ---------------------------------------------
        //  LEVEL END LOGIC
        // ---------------------------------------------

        void WinLevel()
        {
            m_LevelEnded = true;
            Debug.Log("Level Won! Calling LevelUIManager.");

            if (victorySound != null)
            {
                AudioSource.PlayClipAtPoint(victorySound, Camera.main.transform.position);
            }

            // LevelUIManager ke Singleton instance ko call karein taake time scale freeze ho aur panel animation chale
            if (LevelUIManager.Instance != null)
            {
                LevelUIManager.Instance.ShowVictory();
            }
            else
            {
                // Fallback: Agar kisi wajah se manager missing ho to direct panel active karein ya menu pe jayen
                if (victoryPanel != null)
                {
                    victoryPanel.SetActive(true);
                }
                else
                {
                    ButtonMainMenu();
                }
            }
        }

        void TimeOutFail()
        {
            m_LevelEnded = true;
            Debug.Log("Time Out! Redirecting to Main Menu.");
            ButtonMainMenu();
        }

        // ---------------------------------------------
        //  UI Refresh & Button callbacks
        // ---------------------------------------------

        void RefreshAllUI()
        {
            RefreshTimerText();
            RefreshKeyText();
        }

        public void ButtonRestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        public void ButtonMainMenu() => SceneManager.LoadScene("MainMenu");
    }
}