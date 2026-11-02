using UnityEngine;
using UnityEngine.XR;

public class VRControllerAxisReader : MonoBehaviour
{
    public XRNode controllerNode = XRNode.RightHand;

    private InputDevice device;

    // Threshold ranges
    public float threshold = 0.2f;

    private Vector3 controllerPosition;

    void Start()
    {
        InitializeDevice();
    }

    void InitializeDevice()
    {
        device = InputDevices.GetDeviceAtXRNode(controllerNode);
    }

    void Update()
    {
        if (!device.isValid)
        {
            InitializeDevice();
        }

        if (device.TryGetFeatureValue(CommonUsages.devicePosition, out controllerPosition))
        {
            float xValue = EvaluateAxis(controllerPosition.x);
            float yValue = EvaluateAxis(controllerPosition.y);
            float zValue = EvaluateAxis(controllerPosition.z);

            Debug.Log($"X: {xValue}, Y: {yValue}, Z: {zValue}");
        }
    }

    float EvaluateAxis(float value)
    {
        if (value > threshold)
            return 1f;
        else if (value < -threshold)
            return -1f;
        else
            return 0f;
    }
}