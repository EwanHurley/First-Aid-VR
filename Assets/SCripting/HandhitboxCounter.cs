using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Attach to a GameObject with a Collider set to "Is Trigger".
/// Increments a counter each time a hand ENTERS the hitbox.
/// Exiting the hitbox does nothing.
/// </summary>
[RequireComponent(typeof(Collider))]
public class HandHitboxCounter : MonoBehaviour
{
    [Header("Detection")]
    [Tooltip("Any collider with one of these tags counts as a hand.")]
    [SerializeField] private string[] handTags = { "LeftHand", "RightHand", "PlayerHand" };

    [Header("Output")]
    [Tooltip("Optional: a TextMeshPro label to display the count.")]
    [SerializeField] private TMP_Text counterLabel;
    [SerializeField] private string labelFormat = "Count: {0}";
    public UnityEvent<int> onCountChanged;

    public int Count { get; private set; }

    // Tracks how many colliders of each hand are currently inside, so a hand
    // made of several colliders (fingers, palm, etc.) only counts once per entry.
    private readonly Dictionary<Transform, int> _collidersInside = new();

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void Start()
    {
        UpdateDisplay();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsHand(other)) return;

        Transform hand = GetHandRoot(other);

        _collidersInside.TryGetValue(hand, out int inside);
        _collidersInside[hand] = inside + 1;

        // Only count when the hand goes from "not inside" to "inside".
        if (inside == 0)
        {
            Count++;
            UpdateDisplay();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsHand(other)) return;

        Transform hand = GetHandRoot(other);

        // Bookkeeping only; the counter is never decremented.
        if (_collidersInside.TryGetValue(hand, out int inside))
        {
            if (inside <= 1) _collidersInside.Remove(hand);
            else _collidersInside[hand] = inside - 1;
        }
    }

    private bool IsHand(Collider col)
    {
        foreach (string t in handTags)
            if (col.CompareTag(t)) return true;
        return false;
    }

    // Use the Rigidbody's transform if there is one, otherwise the collider's own.
    private static Transform GetHandRoot(Collider col)
    {
        return col.attachedRigidbody != null ? col.attachedRigidbody.transform : col.transform;
    }

    private void UpdateDisplay()
    {
        if (counterLabel != null)
            counterLabel.text = string.Format(labelFormat, Count);
            Debug.Log($"Count is now {Count}");

        onCountChanged?.Invoke(Count);
    }

    public void ResetCount()
    {
        Count = 0;
        UpdateDisplay();
    }
}