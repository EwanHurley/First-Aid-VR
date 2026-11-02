using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RadialMenuUI))]
public class RadialMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RadialMenuUI menuUI;

    [Header("Selection Feel")]
    [Tooltip("Minimum stick deflection before a segment counts as highlighted. Below this, releasing Y selects nothing.")]
    [SerializeField, Range(0.1f, 0.9f)] private float stickDeadzone = 0.35f;

    [Header("Events")]
    public UnityEvent OnOpened;
    public UnityEvent OnClosed;
    [Tooltip("Fires with the selected index (0-2) when the player confirms a choice.")]
    public UnityEvent<int> OnOptionSelected;

    private InputAction openAction;
    private InputAction stickAction;
    private bool isOpen;

    private void Awake()
    {
        if (menuUI == null) menuUI = GetComponent<RadialMenuUI>();

        // Left controller Y button = "secondaryButton" on the left hand in the XR Input Layout.
        openAction = new InputAction("RadialMenuOpen", InputActionType.Button, "<XRController>{LeftHand}/secondaryButton");

        // Right thumbstick drives selection. Can be changed to LeftHand later if we decide that's not the move
        stickAction = new InputAction("RadialMenuStick", InputActionType.Value, "<XRController>{RightHand}/thumbstick", expectedControlType: "Vector2");
    }

    private void OnEnable()
    {
        openAction.Enable();
        stickAction.Enable();
        openAction.started += HandleOpenPressed;
        openAction.canceled += HandleOpenReleased;
        menuUI.OnOptionConfirmed += ForwardSelection;
    }

    private void OnDisable()
    {
        openAction.started -= HandleOpenPressed;
        openAction.canceled -= HandleOpenReleased;
        menuUI.OnOptionConfirmed -= ForwardSelection;
        openAction.Disable();
        stickAction.Disable();
    }

    private void HandleOpenPressed(InputAction.CallbackContext ctx)
    {
        isOpen = true;
        menuUI.Show();
        OnOpened?.Invoke();
    }

    private void HandleOpenReleased(InputAction.CallbackContext ctx)
    {
        if (!isOpen) return;
        isOpen = false;

        menuUI.ConfirmCurrentHighlight();
        menuUI.Hide();
        OnClosed?.Invoke();
    }

    private void ForwardSelection(int index)
    {
        OnOptionSelected?.Invoke(index);
    }

    private void Update()
    {
        if (!isOpen) return;

        Vector2 stick = stickAction.ReadValue<Vector2>();

        if (stick.magnitude < stickDeadzone)
        {
            menuUI.SetHighlighted(-1);
            return;
        }

        float angleDeg = Mathf.Atan2(stick.y, stick.x) * Mathf.Rad2Deg; // 0 = right, 90 = up
        // Convert to the same clockwise-from-top scheme used in RadialMenuUI's layout.
        float fromTopClockwise = (90f - angleDeg + 360f) % 360f;

        int segment = Mathf.RoundToInt(fromTopClockwise / (360f / RadialMenuUI.OptionCount)) % RadialMenuUI.OptionCount;
        menuUI.SetHighlighted(segment);
    }
}