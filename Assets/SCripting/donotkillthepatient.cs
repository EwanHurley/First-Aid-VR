using UnityEngine;

public class donotkillthepatient : MonoBehaviour
{
    [Header("Light Settings")]
    public Light attachedLight;

    private Color defaultColor = Color.white;
    private Color activeColor = Color.red;

    private void Start()
    {
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