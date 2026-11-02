using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RadialMenuUI : MonoBehaviour
{
    [Header("Layout")]
    [SerializeField] private float wheelRadius = 0.12f;      // meters, world-space canvas scale
    [SerializeField] private float segmentSize = 0.09f;       // diameter of each segment button
    [SerializeField] private float canvasScale = 0.001f;      // world units per UI unit

    [Header("Colors")]
    [SerializeField] private Color normalColor = new Color(0.15f, 0.15f, 0.15f, 0.85f);
    [SerializeField] private Color highlightColor = new Color(0.25f, 0.55f, 0.95f, 0.95f);
    [SerializeField] private Color textColor = Color.white;

    [Header("Starting Labels")]
    [SerializeField] private string[] startingLabels = new string[] { "Option 1", "Option 2", "Option 3" };

    public const int OptionCount = 3;

    public event Action<int> OnOptionConfirmed;

    private Canvas canvas;
    private Image[] segmentBackgrounds = new Image[OptionCount];
    private TextMeshProUGUI[] segmentLabels = new TextMeshProUGUI[OptionCount];
    private int currentHighlight = -1;

    private void Awake()
    {
        BuildWheel();
        Hide();
    }

    private void BuildWheel()
    {
        GameObject canvasObj = new GameObject("RadialMenuCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasObj.transform.SetParent(transform, false);
        canvas = canvasObj.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvasObj.transform.localScale = Vector3.one * canvasScale;

        RectTransform canvasRect = canvasObj.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(1000, 1000);
        canvasRect.localPosition = Vector3.zero;

        for (int i = 0; i < OptionCount; i++)
        {
            float angleDeg = 90f - (360f / OptionCount) * i; // segment 0 at top, going clockwise
            float angleRad = angleDeg * Mathf.Deg2Rad;
            Vector2 pos = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * (wheelRadius / canvasScale);

            GameObject segObj = new GameObject($"Segment_{i}", typeof(Image));
            segObj.transform.SetParent(canvasObj.transform, false);
            RectTransform segRect = segObj.GetComponent<RectTransform>();
            segRect.sizeDelta = Vector2.one * (segmentSize / canvasScale);
            segRect.anchoredPosition = pos;

            Image bg = segObj.GetComponent<Image>();
            bg.color = normalColor;
            segmentBackgrounds[i] = bg;

            GameObject labelObj = new GameObject("Label", typeof(TextMeshProUGUI));
            labelObj.transform.SetParent(segObj.transform, false);
            RectTransform labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            TextMeshProUGUI tmp = labelObj.GetComponent<TextMeshProUGUI>();
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = 28;
            tmp.color = textColor;
            tmp.text = i < startingLabels.Length ? startingLabels[i] : $"Option {i + 1}";
            tmp.enableAutoSizing = true;
            tmp.fontSizeMin = 10;
            tmp.fontSizeMax = 36;
            segmentLabels[i] = tmp;
        }
    }

    /// Updates the visible text for one segment (0-2). Safe to call at any time, including while the menu is hidden.</summary>
    public void SetOptionText(int index, string text)
    {
        if (index < 0 || index >= OptionCount)
        {
            Debug.LogWarning($"[RadialMenuUI] SetOptionText index {index} out of range (0-{OptionCount - 1}).");
            return;
        }
        segmentLabels[index].text = text;
    }

    /// Highlights segment `index`, or clears highlight if index is -1 or out of range.</summary>
    public void SetHighlighted(int index)
    {
        if (index == currentHighlight) return;

        if (currentHighlight >= 0 && currentHighlight < OptionCount)
            segmentBackgrounds[currentHighlight].color = normalColor;

        currentHighlight = index;

        if (currentHighlight >= 0 && currentHighlight < OptionCount)
            segmentBackgrounds[currentHighlight].color = highlightColor;
    }

    public void Show()
    {
        canvas.gameObject.SetActive(true);
        SetHighlighted(-1);
    }

    public void Hide()
    {
        canvas.gameObject.SetActive(false);
    }

    /// Call this when the open button is released to commit whatever is currently highlighted.</summary>
    public void ConfirmCurrentHighlight()
    {
        if (currentHighlight >= 0 && currentHighlight < OptionCount)
            OnOptionConfirmed?.Invoke(currentHighlight);
    }
}