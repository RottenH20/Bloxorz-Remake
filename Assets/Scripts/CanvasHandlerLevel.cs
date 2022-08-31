using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHandlerLevel : MonoBehaviour
{
    public GameObject SettingsMenu, SettingsButton, InputManager;
    bool settingsOn = false;
    public AudioSource ClickSound;
    public CharacterController CharacterController;

    public void flipInput()
    {
        if (InputManager.activeSelf)
            InputManager.SetActive(false);
        else
            InputManager.SetActive(true);
    }

    public void playerDied()
    {
        SettingsButton.SetActive(false);
        SettingsMenu.SetActive(true);
        flipInput();

        //NumberOfMovesUIWin.text = NumberOfMovesUIWin.text + CharacterController.numberOfMoves.ToString();
        //NumberOfMovesUIDeath.text = NumberOfMovesUIDeath.text + CharacterController.numberOfMoves.ToString();
    }

    public void ActivateWinMenu()
    {
        SettingsButton.SetActive(false);
        SettingsMenu.SetActive(true);
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
        PlayClickSound();
    }

    public void toggleSound()
    {
        // Toggle sound here
        // soundOn = !soundOn;
    }

    // This is in own method for use in Menupagehandeler.cs
    public void PlayClickSound()
    {
        ClickSound.Play();
    }
}
