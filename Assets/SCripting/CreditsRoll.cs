using UnityEngine;
using UnityEngine.InputSystem;

public class CreditsRoll : MonoBehaviour
{
    [Header("Credits Settings")]
    public float scrollSpeed = 40f;
    public float endPosition = 1200f;

    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;

    private RectTransform creditsRect;
    private Vector2 startPosition;
    private bool isRolling = false;

    void Awake()
    {
        creditsRect = GetComponent<RectTransform>();

        if (creditsRect != null)
        {
            startPosition = creditsRect.anchoredPosition;
        }
        else
        {
            Debug.LogError(
                "CreditsRoll must be attached to a UI object with a RectTransform."
            );
        }
    }

    void OnEnable()
    {
        if (creditsRect == null)
        {
            creditsRect = GetComponent<RectTransform>();
        }

        if (creditsRect == null)
        {
            return;
        }

        // Reset credits to their starting position.
        creditsRect.anchoredPosition = startPosition;
        isRolling = true;
    }

    void Update()
    {
        if (!isRolling || creditsRect == null)
        {
            return;
        }

        // Scroll the credits upward.
        creditsRect.anchoredPosition +=
            Vector2.up * scrollSpeed * Time.deltaTime;

        // Return to the main menu when the credits finish.
        if (creditsRect.anchoredPosition.y >= endPosition)
        {
            ReturnToMainMenu();
            return;
        }

        // Detect mouse clicks and keyboard presses.
        bool mouseClicked =
            Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame;

        bool keyboardPressed =
            Keyboard.current != null &&
            Keyboard.current.anyKey.wasPressedThisFrame;

        if (mouseClicked || keyboardPressed)
        {
            ReturnToMainMenu();
        }
    }

    public void ReturnToMainMenu()
    {
        if (!isRolling)
        {
            return;
        }

        isRolling = false;

        // Check that both panels have been assigned.
        if (mainMenuPanel == null || creditsPanel == null)
        {
            Debug.LogError(
                "CreditsRoll: Assign Main Menu Panel and Credits Panel in the Inspector."
            );
            return;
        }

        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}