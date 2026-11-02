using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation; // Unity 6 / XRI 3.x namespace

public class FixedTeleporter : MonoBehaviour
{
    [Header("References")]
    // Assign the TeleportationProvider component that shows the "Mediator" field
    [SerializeField] private TeleportationProvider teleportationProvider;
    
    [Header("Destination Settings")]
    [SerializeField] private Transform targetDestination;

    public void TeleportToFixedPosition()
    {
        if (teleportationProvider == null || targetDestination == null)
        {
            Debug.LogWarning("FixedTeleporter: Missing references in the Inspector.");
            return;
        }

        // Build a standard TeleportRequest
        TeleportRequest request = new TeleportRequest
        {
            destinationPosition = targetDestination.position,
            destinationRotation = targetDestination.rotation,
            matchOrientation = MatchOrientation.TargetUpAndForward // Directs the player to face the target's forward vector
        };

        // Pass the request to the provider; it will automatically pass it up to the Mediator safely
        teleportationProvider.QueueTeleportRequest(request);
    }
}
