
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject scenarioPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    public AudioClip buttonClickSound;
    public AudioSource audioSource;

    public void PlayButtonClick()
    {
        Debug.Log("PlayButtonClick was called!");

        if (buttonClickSound != null && audioSource != null)
        {
            Debug.Log("Playing click sound: " + buttonClickSound.name);
            audioSource.PlayOneShot(buttonClickSound);
        }
        else
        {
            Debug.LogWarning("Click sound or Audio Source is missing!");
        }
    }

    public void OpenScenarios()
    {
        mainMenuPanel.SetActive(false);
        scenarioPanel.SetActive(true);
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        scenarioPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting First Aid VR...");
        Application.Quit();
    }
}