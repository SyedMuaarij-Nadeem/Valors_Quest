using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using System.Collections;

// =====================================================
//  ATTACH THIS TO: Canvas object in MainMenu scene
//
//  CHARACTER PREVIEW SYSTEM HATAYA GAYA HAI:
//  - Koi 3D prefab spawn nahi hoga
//  - Koi RawImage / RenderTexture nahi
//  - Sirf CharNameBadge update hoga
// =====================================================
public class MainMenuManager : MonoBehaviour
{
    // ─────────────────────────────────────────────────
    //  CHARACTER DATA
    // ─────────────────────────────────────────────────
    [Header("=== CHARACTER DATA ===")]
    public string[] charNames = { "THE PLAYER", "WIZARD BLUE", "WIZARD GREEN", "WIZARD RED" };
    private int currentChar = 0;

    // ─────────────────────────────────────────────────
    //  CHARACTER UI — SIRF NAAM
    // ─────────────────────────────────────────────────
    [Header("=== CHARACTER UI ===")]
    public TextMeshProUGUI charNameBadge;   // CharNameBadge TMP — Inspector mein drag karo
    public Button prevCharBtn;
    public Button nextCharBtn;

    // ─────────────────────────────────────────────────
    //  MAIN MENU BUTTONS
    // ─────────────────────────────────────────────────
    [Header("=== MAIN MENU BUTTONS ===")]
    public Button btnSelectLevel;
    public Button btnSelectHero;
    public Button btnResumeGame;
    public Button btnExitGame;

    // ─────────────────────────────────────────────────
    //  PANELS
    // ─────────────────────────────────────────────────
    [Header("=== PANELS ===")]
    public GameObject levelPanel;
    public GameObject heroPanel;
    public GameObject exitPanel;

    // ─────────────────────────────────────────────────
    //  LEVEL PANEL
    // ─────────────────────────────────────────────────
    [Header("=== LEVEL PANEL ===")]
    public Button closeLevelBtn;
    public Button levelCard1Btn;
    public Button levelCard2Btn;
    public Button levelCard3Btn;

    // ─────────────────────────────────────────────────
    //  HERO PANEL
    // ─────────────────────────────────────────────────
    [Header("=== HERO PANEL ===")]
    public Button closeHeroBtn;
    public Button charCard0Btn;
    public Button charCard1Btn;
    public Button charCard2Btn;
    public Button charCard3Btn;
    public Image charCard0BG;
    public Image charCard1BG;
    public Image charCard2BG;
    public Image charCard3BG;

    // ─────────────────────────────────────────────────
    //  EXIT PANEL
    // ─────────────────────────────────────────────────
    [Header("=== EXIT PANEL ===")]
    public Button stayBtn;
    public Button exitConfirmBtn;

    // ─────────────────────────────────────────────────
    //  FADE
    // ─────────────────────────────────────────────────
    [Header("=== FADE OVERLAY ===")]
    public CanvasGroup fadeOverlay;

    // ─────────────────────────────────────────────────
    //  SCENE NAMES
    // ─────────────────────────────────────────────────
    [Header("=== SCENE NAMES ===")]
    public string level1Scene = "Level1_Prison";
    public string level2Scene = "Level2_Graveyard";
    public string level3Scene = "Level3_Dungeon";

    // ─────────────────────────────────────────────────
    //  PRIVATE
    // ─────────────────────────────────────────────────
    private Color selectedCardColor = new Color(0.35f, 0.20f, 0.08f, 1f);
    private Color normalCardColor = new Color(0.10f, 0.06f, 0.04f, 1f);
    private bool isTransitioning = false;

    // =====================================================
    void Start()
    {
        // Panels hide
        if (levelPanel != null) levelPanel.SetActive(false);
        if (heroPanel != null) heroPanel.SetActive(false);
        if (exitPanel != null) exitPanel.SetActive(false);

        // Saved hero load karo
        currentChar = GameProgressManager.GetSelectedHero();

        // Sirf naam aur card highlight update karo
        UpdateNameBadge();
        UpdateCardHighlight();

        // Fade in
        StartCoroutine(FadeIn());

        // ── Main buttons ──
        btnSelectLevel?.onClick.AddListener(() => OpenPanel(levelPanel));
        btnSelectHero?.onClick.AddListener(() => OpenPanel(heroPanel));
        btnExitGame?.onClick.AddListener(() => OpenPanel(exitPanel));
        btnResumeGame?.onClick.AddListener(() =>
        {
            string lastLevel = GameProgressManager.GetLastLevel();
            LoadLevel(lastLevel);
        });

        // ── Level panel ──
        closeLevelBtn?.onClick.AddListener(() => ClosePanel(levelPanel));
        levelCard1Btn?.onClick.AddListener(() => { ClosePanel(levelPanel); LoadLevel(level1Scene); });
        levelCard2Btn?.onClick.AddListener(() => { ClosePanel(levelPanel); LoadLevel(level2Scene); });
        levelCard3Btn?.onClick.AddListener(() => { ClosePanel(levelPanel); LoadLevel(level3Scene); });

        // ── Hero panel ──
        closeHeroBtn?.onClick.AddListener(() => ClosePanel(heroPanel));
        charCard0Btn?.onClick.AddListener(() => SelectCharacter(0));
        charCard1Btn?.onClick.AddListener(() => SelectCharacter(1));
        charCard2Btn?.onClick.AddListener(() => SelectCharacter(2));
        charCard3Btn?.onClick.AddListener(() => SelectCharacter(3));

        // ── Exit panel ──
        stayBtn?.onClick.AddListener(() => ClosePanel(exitPanel));
        exitConfirmBtn?.onClick.AddListener(QuitGame);

        // ── Arrows ──
        prevCharBtn?.onClick.AddListener(PrevChar);
        nextCharBtn?.onClick.AddListener(NextChar);
    }

    // =====================================================
    //  CHARACTER SYSTEM — SIRF NAAM, KOI SPAWN NAHI
    // =====================================================
    void PrevChar()
    {
        currentChar = (currentChar - 1 + charNames.Length) % charNames.Length;
        UpdateNameBadge();
        UpdateCardHighlight();
    }

    void NextChar()
    {
        currentChar = (currentChar + 1) % charNames.Length;
        UpdateNameBadge();
        UpdateCardHighlight();
    }

    void SelectCharacter(int idx)
    {
        currentChar = idx;
        // Hero save karo — level scene mein HeroSpawner yahi use karega
        GameProgressManager.SaveSelectedHero(idx);
        UpdateNameBadge();
        UpdateCardHighlight();
        ClosePanel(heroPanel);
    }

    // Sirf naam update — koi 3D / RawImage nahi
    void UpdateNameBadge()
    {
        if (charNameBadge != null)
            charNameBadge.text = charNames[currentChar];
    }

    void UpdateCardHighlight()
    {
        Image[] cards = { charCard0BG, charCard1BG, charCard2BG, charCard3BG };
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != null)
                cards[i].DOColor(
                    i == currentChar ? selectedCardColor : normalCardColor, 0.2f);
        }
    }

    // =====================================================
    //  PANEL SYSTEM
    // =====================================================
    void OpenPanel(GameObject panel)
    {
        if (panel == null) return;
        panel.SetActive(true);

        if (panel.transform.childCount > 0)
        {
            Transform box = panel.transform.GetChild(0);
            box.localScale = Vector3.one * 0.85f;
            box.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
        }

        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 0f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
            cg.DOFade(1f, 0.22f);
        }
    }

    void ClosePanel(GameObject panel)
    {
        if (panel == null) return;
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
            cg.DOFade(0f, 0.18f).OnComplete(() => panel.SetActive(false));
        }
        else panel.SetActive(false);
    }

    // =====================================================
    //  SCENE LOADING
    // =====================================================
    public void LoadLevel(string sceneName)
    {
        if (isTransitioning) return;
        isTransitioning = true;
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // =====================================================
    //  FADE
    // =====================================================
    IEnumerator FadeIn()
    {
        if (fadeOverlay == null) yield break;
        fadeOverlay.alpha = 1f;
        fadeOverlay.blocksRaycasts = true;
        float t = 0f;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            fadeOverlay.alpha = 1f - Mathf.Clamp01(t / 0.5f);
            yield return null;
        }
        fadeOverlay.alpha = 0f;
        fadeOverlay.blocksRaycasts = false;
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        if (fadeOverlay != null)
        {
            fadeOverlay.blocksRaycasts = true;
            float t = 0f;
            while (t < 0.4f)
            {
                t += Time.deltaTime;
                fadeOverlay.alpha = Mathf.Clamp01(t / 0.4f);
                yield return null;
            }
            fadeOverlay.alpha = 1f;
        }
        SceneManager.LoadScene(sceneName);
    }
}