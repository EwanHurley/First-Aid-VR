using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Hands;

/// <summary>
/// Moves this object (a kinematic Rigidbody + collider) to a tracked hand joint.
/// Make one per hand and set the Handedness. Collider is disabled while the hand is untracked.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class HandJointFollower : MonoBehaviour
{
    [SerializeField] private Handedness handedness = Handedness.Right;
    [SerializeField] private XRHandJointID joint = XRHandJointID.Palm;
    [Tooltip("Leave empty to auto-find the XR Origin in a parent.")]
    [SerializeField] private XROrigin xrOrigin;
    [SerializeField] private bool followRotation = false;

    private Rigidbody _rb;
    private Collider[] _colliders;
    private XRHandSubsystem _subsystem;
    private Pose _targetPose;
    private bool _hasTarget;

    private static readonly List<XRHandSubsystem> s_Subsystems = new();

    private void Reset()
    {
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
        if (xrOrigin == null) xrOrigin = GetComponentInParent<XROrigin>();
        SetCollidersEnabled(false);
    }

    private void Update()
    {
        // The hand subsystem may not be running yet at startup, so keep looking until it is.
        if (_subsystem == null || !_subsystem.running)
        {
            SubsystemManager.GetSubsystems(s_Subsystems);
            _subsystem = null;
            foreach (var s in s_Subsystems)
            {
                if (s.running) { _subsystem = s; break; }
            }
            if (_subsystem == null) { _hasTarget = false; SetCollidersEnabled(false); return; }
        }

        XRHand hand = handedness == Handedness.Left ? _subsystem.leftHand : _subsystem.rightHand;

        if (!hand.isTracked || !hand.GetJoint(joint).TryGetPose(out Pose local))
        {
            _hasTarget = false;
            SetCollidersEnabled(false);
            return;
        }

        // Joint poses are relative to the XR Origin, so convert to world space.
        Transform origin = xrOrigin != null ? xrOrigin.Origin.transform : transform.parent;
        _targetPose = new Pose(
            origin.TransformPoint(local.position),
            origin.rotation * local.rotation);

        _hasTarget = true;
        SetCollidersEnabled(true);
    }

    // Move the kinematic body in the physics step so triggers fire reliably.
    private void FixedUpdate()
    {
        if (!_hasTarget) return;

        _rb.MovePosition(_targetPose.position);
        if (followRotation) _rb.MoveRotation(_targetPose.rotation);
    }

    private void SetCollidersEnabled(bool value)
    {
        foreach (var c in _colliders)
            if (c.enabled != value) c.enabled = value;
    }
}