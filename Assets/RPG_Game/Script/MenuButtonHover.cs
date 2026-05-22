using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

// =====================================================
//  ATTACH THIS TO: BtnSelectLevel, BtnSelectHero,
//                  BtnResumeGame, BtnExitGame
//  Charon par alag alag lagao
// =====================================================
public class MenuButtonHover : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Child References")]
    public Image leftBorder;              // LeftBorder Image
    public TextMeshProUGUI labelTMP;      // BtnLabel TMP
    public TextMeshProUGUI arrowTMP;      // BtnArrow TMP
    public Image btnBackground;           // Button ka Image component

    [Header("Is Primary Button?")]
    public bool isPrimary = false;        // Sirf BtnSelectLevel par check karo

    [Header("Animation Speed")]
    public float slidePixels = 4f;

    // Colors
    private readonly Color borderNormal  = new Color(0.545f, 0.102f, 0.102f);
    private readonly Color borderHover   = new Color(0.788f, 0.635f, 0.153f);
    private readonly Color borderPrimary = new Color(0.788f, 0.635f, 0.153f);
    private readonly Color labelNormal   = new Color(0.690f, 0.502f, 0.125f);
    private readonly Color labelHover    = new Color(0.910f, 0.753f, 0.251f);

    private RectTransform rt;
    private float origX;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        origX = rt.anchoredPosition.x;

        if (isPrimary && leftBorder != null)
            leftBorder.color = borderPrimary;
    }

    public void OnPointerEnter(PointerEventData e)
    {
        rt.DOAnchorPosX(origX + slidePixels, 0.2f).SetEase(Ease.OutCubic);
        leftBorder?.DOColor(borderHover, 0.2f);
        labelTMP?.DOColor(labelHover, 0.2f);
    }

    public void OnPointerExit(PointerEventData e)
    {
        rt.DOAnchorPosX(origX, 0.2f).SetEase(Ease.OutCubic);
        leftBorder?.DOColor(isPrimary ? borderPrimary : borderNormal, 0.2f);
        labelTMP?.DOColor(labelNormal, 0.2f);
    }

    public void OnPointerClick(PointerEventData e)
    {
        transform.DOPunchScale(Vector3.one * -0.04f, 0.12f, 5);
    }
}
