using UnityEngine;
using System.Collections;

// =========================================================
//  ATTACH THIS TO: The PanelBox Image inside each overlay panel
//  (LevelPanel > PanelBox, CharPanel > PanelBox, ExitPanel > ExitBox)
// =========================================================
public class PanelAnimator : MonoBehaviour
{
    [Header("Animation")]
    public float duration = 0.18f;

    private RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    // Called by MainMenuManager when panel opens
    public void PlayOpen()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleIn());
    }

    IEnumerator ScaleIn()
    {
        float t = 0f;
        rt.localScale = new Vector3(0.88f, 0.88f, 1f);

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duration);
            // Cubic ease-out: fast start → gentle finish
            float eased = 1f - Mathf.Pow(1f - p, 3f);
            float s = Mathf.Lerp(0.88f, 1f, eased);
            rt.localScale = new Vector3(s, s, 1f);
            yield return null;
        }

        rt.localScale = Vector3.one;
    }
}
