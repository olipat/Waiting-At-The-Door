using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonHoverTriangle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RawImage triangleImage;
    public Transform targetToAnimate; // Assign what you want to bounce (can be the button or a child)
    public float punchStrength = 0.1f;
    public float punchDuration = 0.2f;
    public int punchVibrato = 10;
    public float punchElasticity = 1;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (triangleImage != null)
            triangleImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (triangleImage != null)
            triangleImage.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        if (targetToAnimate == null)
            targetToAnimate = transform; // fallback to self

        targetToAnimate.DOPunchScale(Vector3.one * punchStrength, punchDuration, punchVibrato, punchElasticity);
    }
}
