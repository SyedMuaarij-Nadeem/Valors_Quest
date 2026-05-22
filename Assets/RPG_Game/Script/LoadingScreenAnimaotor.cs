using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement; // Scene change k liyay ye add karna zaruri ha

public class LoadingScreenAnimator : MonoBehaviour
{
    [Header("UI References")]
    public Slider loadingBar;
    public TextMeshProUGUI percentageText;
    public TextMeshProUGUI tipText;
    public RectTransform blurGlow;
    

    [Header("Settings")]
    public float loadDuration = 5f;
    public string nextSceneName = "MainMenu"; // Yahan Inspector se bhi scene ka naam change kar sakty hain

    private string[] tips = {
        "Tip: Collect all keys to activate the portal",
        "Tip: Watch out for Skeleton Giants — they hit hard",
        "Tip: Timer turns red when under 30 seconds remain",
        "Tip: Sprint past enemies to reach the keys faster",
        "Tip: Each level has a different number of required keys"
    };

    void Start()
    {
        // 1. Setup Initial States
        if (loadingBar != null) loadingBar.value = 0;
        if (percentageText != null) percentageText.text = "0%";

        // 2. CSS Animation Replications (DOTween)

        // Blur Glow Pulse Effect (scale up and down infinitely)
        if (blurGlow != null)
        {
            blurGlow.DOScale(1.2f, 1.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }

        

        // 3. Start Loading Simulation
        StartCoroutine(SimulateLoadingProcess());
    }

    IEnumerator SimulateLoadingProcess()
    {
        float currentPct = 0;
        int tipIndex = 0;

        if (tipText != null) tipText.text = tips[tipIndex];

        while (currentPct < 100f)
        {
            // Random increments for realistic loading
            float increment = Random.Range(1f, 5f);
            currentPct += increment;
            if (currentPct > 100f) currentPct = 100f;

            // Animate slider fill smoothly
            if (loadingBar != null) loadingBar.DOValue(currentPct / 100f, 0.1f);
            if (percentageText != null) percentageText.text = Mathf.FloorToInt(currentPct) + "%";

            // Randomly change tip
            if (Random.value < 0.2f && tipText != null)
            {
                tipIndex = (tipIndex + 1) % tips.Length;
                tipText.DOFade(0, 0.2f).OnComplete(() => {
                    tipText.text = tips[tipIndex];
                    tipText.DOFade(1, 0.2f);
                });
            }

            // Random delay between jumps
            float delay = Random.Range(0.05f, 0.2f);
            yield return new WaitForSeconds(delay);
        }

        // Done Loading!
        if (percentageText != null) percentageText.text = "100%";

        // 0.8 seconds ka delay ta k user 100% dekh saky (Same as HTML)
        yield return new WaitForSeconds(0.8f);

        // Next Scene Load
        SceneManager.LoadScene(nextSceneName);
    }
}