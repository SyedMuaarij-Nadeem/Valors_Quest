using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

// =====================================================
//  LevelUIManager.cs — FIXED
//  FIX: GameOver Invoke → Coroutine
//       (Invoke Time.timeScale=0 par kaam nahi karta)
// =====================================================
namespace RPGGame
{
    public class LevelUIManager : MonoBehaviour
    {
        public static LevelUIManager Instance { get; private set; }

        [Header("=== PAUSE BUTTON (Game Screen) ===")]
        public Button pauseButton;

        [Header("=== PAUSE PANEL ===")]
        public GameObject pausePanel;
        public Button btnResume;
        public Button btnPauseRestart;
        public Button btnPauseMainMenu;
        public Button btnPauseExit;

        [Header("=== VICTORY PANEL ===")]
        public GameObject victoryPanel;
        public Button btnNextLevel;
        public Button btnVictoryRestart;
        public Button btnVictoryMainMenu;
        public Button btnVictoryExit;

        [Header("=== GAMEOVER PANEL ===")]
        public GameObject gameOverPanel;
        public Button btnGameOverRestart;
        public Button btnGameOverMainMenu;
        public Button btnGameOverExit;

        private bool isPaused = false;
        private bool levelEnded = false;

        void Awake() => Instance = this;

        void Start()
        {
            ForceHidePanel(pausePanel);
            ForceHidePanel(victoryPanel);
            ForceHidePanel(gameOverPanel);

            pauseButton?.onClick.AddListener(TogglePause);

            btnResume?.onClick.AddListener(ResumeGame);
            btnPauseRestart?.onClick.AddListener(RestartLevel);
            btnPauseMainMenu?.onClick.AddListener(GoToMainMenu);
            btnPauseExit?.onClick.AddListener(ExitGame);

            btnNextLevel?.onClick.AddListener(LoadNextLevel);
            btnVictoryRestart?.onClick.AddListener(RestartLevel);
            btnVictoryMainMenu?.onClick.AddListener(GoToMainMenu);
            btnVictoryExit?.onClick.AddListener(ExitGame);

            btnGameOverRestart?.onClick.AddListener(RestartLevel);
            btnGameOverMainMenu?.onClick.AddListener(GoToMainMenu);
            btnGameOverExit?.onClick.AddListener(ExitGame);
        }

        // ── PAUSE ──────────────────────────────────────
        public void TogglePause()
        {
            if (levelEnded) return;
            if (isPaused) ResumeGame();
            else PauseGame();
        }

        void PauseGame()
        {
            isPaused = true;
            Time.timeScale = 0f;
            ShowPanel(pausePanel);
        }

        public void ResumeGame()
        {
            isPaused = false;
            Time.timeScale = 1f;
            HidePanel(pausePanel);
        }

        // ── VICTORY ────────────────────────────────────
        public void ShowVictory()
        {
            if (levelEnded) return;
            levelEnded = true;
            Time.timeScale = 0f;
            ShowPanel(victoryPanel);
            Debug.Log("[LevelUIManager] VICTORY!");
        }

        // ── GAME OVER ──────────────────────────────────
        public void ShowGameOver()
        {
            if (levelEnded) return;
            levelEnded = true;
            Debug.Log("[LevelUIManager] GAME OVER!");
            // ✅ FIX: Invoke ki jagah Coroutine — real seconds mein kaam karta hai
            // timeScale 0 ho ya 1 — dono par chalega
            StartCoroutine(GameOverDelay(1.5f));
        }

        IEnumerator GameOverDelay(float seconds)
        {
            // ✅ WaitForSecondsRealtime — timeScale se independent
            yield return new WaitForSecondsRealtime(seconds);
            Time.timeScale = 0f;
            ShowPanel(gameOverPanel);
        }

        // ── BUTTON ACTIONS ─────────────────────────────
        void RestartLevel()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        void GoToMainMenu()
        {
            Time.timeScale = 1f;
            GameProgressManager.SaveLastLevel(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("MainMenu");
        }

        void LoadNextLevel()
        {
            Time.timeScale = 1f;
            string current = SceneManager.GetActiveScene().name;
            if (current == "Level1_Prison") SceneManager.LoadScene("Level2_Graveyard");
            else if (current == "Level2_Graveyard") SceneManager.LoadScene("Level3_Dungeon");
            else SceneManager.LoadScene("MainMenu");
        }

        void ExitGame()
        {
            Time.timeScale = 1f;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        // ── PANEL HELPERS ──────────────────────────────
        void ForceHidePanel(GameObject panel)
        {
            if (panel == null) return;
            panel.SetActive(false);
            CanvasGroup cg = panel.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        }

        void ShowPanel(GameObject panel)
        {
            if (panel == null) return;
            CanvasGroup cg = panel.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
            panel.SetActive(true);

            // PopupBox bounce
            if (panel.transform.childCount > 1)
            {
                Transform popupBox = panel.transform.GetChild(1);
                popupBox.localScale = Vector3.one * 0.8f;
                popupBox.DOScale(1f, 0.3f)
                        .SetEase(Ease.OutBack)
                        .SetUpdate(true);
            }
        }

        void HidePanel(GameObject panel)
        {
            if (panel == null) return;
            CanvasGroup cg = panel.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.interactable = false;
                cg.blocksRaycasts = false;
                cg.DOFade(0f, 0.2f)
                  .SetUpdate(true)
                  .OnComplete(() => panel.SetActive(false));
            }
            else panel.SetActive(false);
        }
    }
}