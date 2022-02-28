using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHandlerLevel : MonoBehaviour
{
    public GameObject WinMenu, SettingsMenu, DeathMenu, SettingsButton, InputManager;
    public Text NumberOfMovesUIWin, NumberOfMovesUIDeath;
    bool settingsOn = false;
    public AudioSource ClickSound;
    CharacterController CharacterController;

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
        SettingsButton.SetActive(false);
        flipInput();
        NumberOfMoves();
    }

    public void ActivateWinMenu()
    {
        WinMenu.SetActive(true);
        SettingsButton.SetActive(false);
        flipInput();
        NumberOfMoves();
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

    public void toggleSound()
    {
        // Toggle sound here
        // soundOn = !soundOn;
    }

    public void NumberOfMoves()
    {
        NumberOfMovesUIWin.text = NumberOfMovesUIWin.text + CharacterController.numberOfMoves.ToString();
        NumberOfMovesUIDeath.text = NumberOfMovesUIDeath.text + CharacterController.numberOfMoves.ToString();
    }
}
