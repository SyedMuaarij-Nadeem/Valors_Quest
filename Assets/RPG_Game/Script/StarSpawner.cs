using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// =====================================================
//  ATTACH THIS TO: StarField object (Canvas ka child)
// =====================================================
public class StarSpawner : MonoBehaviour
{
    public int starCount = 40;

    void Start()
    {
        for (int i = 0; i < starCount; i++)
        {
            GameObject star = new GameObject("Star_" + i);
            star.transform.SetParent(transform, false);

            Image img = star.AddComponent<Image>();
            img.color = new Color(0.788f, 0.635f, 0.153f, 0.6f);

            RectTransform rt = star.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(1.5f, 1.5f);
            rt.anchoredPosition = new Vector2(
                Random.Range(-960f, 960f),
                Random.Range(-540f, 540f));

            float duration = Random.Range(2f, 6f);
            float delay    = Random.Range(0f, 4f);

            img.DOFade(0.9f, duration)
               .SetLoops(-1, LoopType.Yoyo)
               .SetEase(Ease.InOutSine)
               .SetDelay(delay);
        }
    }
}
