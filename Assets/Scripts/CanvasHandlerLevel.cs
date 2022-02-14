using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasHandlerLevel : MonoBehaviour
{
    public GameObject WinMenu, SettingsMenu, DeathMenu, SettingsButton, InputManager;
    bool settingsOn = false;
    public AudioSource ClickSound;

    public void flipInput()
    {
        if (InputManager.activeSelf)
            InputManager.SetActive(false);
        else
            InputManager.SetActive(true);
    }

    public void playerDied()
    {
        DeathMenu.SetActive(true);
        flipInput();
    }

    public void ActivateWinMenu()
    {
        WinMenu.SetActive(true);
        SettingsButton.SetActive(false);
        flipInput();
    }

    public void SettingsButtonClicked()
    {
        if (settingsOn)
        {
            flipInput();
            SettingsMenu.SetActive(false);
            settingsOn = false;
        }
        else
        {
            flipInput();
            SettingsMenu.SetActive(true);
            settingsOn = true;
        }
        ClickSound.Play();
    }
}
