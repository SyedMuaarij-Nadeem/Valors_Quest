using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LogoScreen : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform logoContainer;
    public Image swordIcon;
    public Image glowImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;
    public TextMeshProUGUI tapText;
    public Image divider;
    public ParticleSystem particles;

    [Header("Timing")]
    public float autoAdvanceTime = 3.5f;
    public float blinkSpeed = 0.9f;

    [Header("Colors")]
    public Color goldColor = new Color(0.788f, 0.635f, 0.153f, 1f);
    public Color goldBright = new Color(0.941f, 0.816f, 0.376f, 1f);
    public Color subtitleColor = new Color(0.541f, 0.416f, 0.188f, 1f);
    public Color tapColor = new Color(0.353f, 0.251f, 0.125f, 1f);

    private bool hasAdvanced = false;
    private CanvasGroup canvasGroup;

    void Start()
    {
        // Canvas Group setup for full screen fade
        canvasGroup = logoContainer.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = logoContainer.gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        // Sword hidden + small scale
        if (swordIcon != null)
        {
            swordIcon.color = new Color(goldColor.r, goldColor.g, goldColor.b, 0f);
            swordIcon.transform.localScale = Vector3.one * 0.6f;
        }

        // Glow hidden
        if (glowImage != null)
            glowImage.color = new Color(glowImage.color.r,
                                        glowImage.color.g,
                                        glowImage.color.b, 0f);

        // Title hidden — POSITION MAT BADLO
        if (titleText != null)
            titleText.color = new Color(goldColor.r, goldColor.g, goldColor.b, 0f);

        // Subtitle hidden
        if (subtitleText != null)
            subtitleText.color = new Color(subtitleColor.r,
                                           subtitleColor.g,
                                           subtitleColor.b, 0f);

        // Divider — wipe effect setup
        if (divider != null)
        {
            divider.color = new Color(goldColor.r, goldColor.g, goldColor.b, 1f);
            divider.transform.localScale = new Vector3(0f, 1f, 1f);
        }

        // TapText hidden
        if (tapText != null)
            tapText.color = new Color(tapColor.r, tapColor.g, tapColor.b, 0f);

        // Particles off
        if (particles != null) particles.Stop();

        // Start sequence
        Sequence seq = DOTween.Sequence();

        // === A: Whole container fade in ===
        seq.Append(canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutCubic));

        // === B: Glow pulse shuru ===
        seq.AppendCallback(() =>
        {
            if (glowImage != null)
            {
                glowImage.DOFade(0.5f, 1.5f)
                         .SetEase(Ease.InOutSine)
                         .SetLoops(-1, LoopType.Yoyo);
            }
        });

        // === C: Sword scale up smoothly (OutBack removed — OutElastic light) ===
        seq.Append(
            swordIcon.transform.DOScale(1f, 0.8f).SetEase(Ease.OutElastic)
        );
        seq.Join(
            swordIcon.DOFade(1f, 0.4f)
        );

        // Sword glow pulse after appearing
        seq.AppendCallback(() =>
        {
            swordIcon.DOColor(goldBright, 1.2f)
                     .SetEase(Ease.InOutSine)
                     .SetLoops(-1, LoopType.Yoyo);
        });

        // === D: Title fade in only (no position shift) ===
        seq.AppendInterval(0.15f);
        seq.Append(
            titleText.DOFade(1f, 0.6f).SetEase(Ease.OutCubic)
        );

        // === E: Subtitle fade in ===
        seq.AppendInterval(0.1f);
        seq.Append(
            subtitleText.DOFade(1f, 0.5f).SetEase(Ease.OutCubic)
        );

        // === F: Divider wipe left to right ===
        seq.AppendInterval(0.1f);
        seq.Append(
            divider.transform.DOScaleX(1f, 0.6f).SetEase(Ease.OutCubic)
        );

        // === G: TapText fade in then blink ===
        seq.AppendInterval(0.2f);
        seq.Append(
            tapText.DOFade(1f, 0.4f).OnComplete(() =>
            {
                tapText.DOFade(0.15f, blinkSpeed)
                       .SetEase(Ease.InOutSine)
                       .SetLoops(-1, LoopType.Yoyo);
            })
        );

        // === H: Particles ===
        seq.AppendCallback(() =>
        {
            if (particles != null) particles.Play();
        });

        // === I: Auto advance ===
        seq.AppendInterval(autoAdvanceTime);
        seq.AppendCallback(() => GoToNext());
    }

    void Update()
    {
        if (!hasAdvanced &&
            (Input.GetMouseButtonDown(0) ||
             Input.GetKeyDown(KeyCode.Space) ||
             Input.GetKeyDown(KeyCode.Return)))
        {
            GoToNext();
        }
    }

    void GoToNext()
    {
        if (hasAdvanced) return;
        hasAdvanced = true;
        DOTween.KillAll();

        canvasGroup.DOFade(0f, 0.4f).OnComplete(() =>
        {
            SceneManager.LoadScene(1);
        });
    }
}