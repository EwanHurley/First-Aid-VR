using UnityEngine;

public class MenuSoundController : MonoBehaviour
{
    public AudioClip buttonClickSound;
    public AudioSource audioSource;

    public void PlayButtonClick()
    {
        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}