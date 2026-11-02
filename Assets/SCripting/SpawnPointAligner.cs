using UnityEngine;
using Unity.XR.CoreUtils;

//This should stop the player from spawning out of the main area.
public class SpawnPointAligner : MonoBehaviour
{
    [Header("Target Spawn Position")]
    [Tooltip("The world position the player's HEAD/CAMERA should end up at on spawn.")]
    [SerializeField] private Vector3 spawnPosition = new Vector3(1f, 1.8f, -3f);

    [Tooltip("The world rotation (Euler angles) the player's HEAD/CAMERA should face on spawn. Only Y (yaw) is typically used.")]
    [SerializeField] private Vector3 spawnRotationEuler = Vector3.zero;

    private XROrigin xrOrigin;

    private void Awake()
    {
        xrOrigin = GetComponent<XROrigin>();

        if (xrOrigin == null)
        {
            Debug.LogError("[SpawnPointAligner] No XROrigin component found on this GameObject. " +
                            "Attach this script to the XR Origin (XR Rig) object.", this);
        }
    }

    private void Start()
    {
        AlignToSpawnPoint();
    }
    public void AlignToSpawnPoint()
    {
        if (xrOrigin == null)
        {
            Debug.LogWarning("[SpawnPointAligner] Cannot align, XROrigin reference is missing.", this);
            return;
        }

        Quaternion targetRotation = Quaternion.Euler(spawnRotationEuler);
        xrOrigin.MoveCameraToWorldLocation(spawnPosition);
        xrOrigin.MatchOriginUpCameraForward(Vector3.up, targetRotation * Vector3.forward);
    }
}