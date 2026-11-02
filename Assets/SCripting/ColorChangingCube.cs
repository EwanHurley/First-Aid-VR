using UnityEngine;

public class ColorChangingCube : MonoBehaviour
{
    [Header("Light Settings")]
    public Light attachedLight;

    private Color defaultColor = Color.blue;
    private Color activeColor = Color.green;

    private void Start()
    {
        if (attachedLight == null)
            attachedLight = GetComponentInChildren<Light>();

        if (attachedLight != null)
            attachedLight.color = defaultColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
            attachedLight.color = activeColor;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
            attachedLight.color = defaultColor;
    }
}