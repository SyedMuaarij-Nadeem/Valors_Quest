using UnityEngine;

// =========================================================
//  ATTACH THIS TO: The CharSymbol RectTransform (left side)
// =========================================================
public class CharacterFloat : MonoBehaviour
{
    [Header("Float")]
    public float floatHeight = 10f;    // pixels up/down
    public float floatPeriod = 3f;     // seconds for one full cycle

    private RectTransform rt;
    private Vector2 originPos;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        originPos = rt.anchoredPosition;
    }

    void Update()
    {
        // Smooth sine wave — matches HTML: animation: charFloat 3s ease-in-out infinite
        float raw = Mathf.Sin((Time.time / floatPeriod) * Mathf.PI * 2f);
        rt.anchoredPosition = new Vector2(originPos.x, originPos.y + raw * floatHeight);
    }
}
