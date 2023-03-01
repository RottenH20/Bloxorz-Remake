using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasHandlerLevel : MonoBehaviour
{
    public GameObject SettingsMenu, SettingsButton, InputManager;
    private TextMeshProUGUI NumberOfMovesUI;
    bool settingsOn = false;
    public AudioSource ClickSound;
    public CharacterController CharacterController;

    bool vertical = false;

    private GameObject mainPlayer;
    private int size = 7; // Change size of camera when vertical here


    // Add camera Vertical button, moves camera into a top down view

    void Start()
    {
        NumberOfMovesUI = GameObject.Find("NumberOfMovesUI").GetComponent<TextMeshProUGUI>();
        mainPlayer = GameObject.FindWithTag("Player");
    }

    public void swapCamera()
    {
        vertical = !vertical;
        if (!vertical)
        {
            Camera.main.transform.position = new Vector3(15, 16, -15);
            Camera.main.transform.rotation = Quaternion.Euler(35, -45, 0);
            Camera.main.orthographicSize = 5;
            //swap camera normally (15, 16, -15) (35, -45, 0) size 5
        }
    }

    void Update()  // Swap camera top down (y = 5) Have camera "follow above player"
    {
        if (vertical)
        {
            Vector3 playerPosition = mainPlayer.transform.position;
            Camera.main.transform.position = new Vector3(playerPosition.x, 15, playerPosition.z);
            Camera.main.orthographicSize = size;
            Camera.main.transform.rotation = Quaternion.Euler(90, -90, 0);
        }
    }

    public void flipInput()
    {
        if (InputManager.activeSelf)
            InputManager.SetActive(false);
        else
            InputManager.SetActive(true);
    }

    public void updateMoveCounter()
    {
        NumberOfMovesUI.text = "# Of Moves = " + CharacterController.numberOfMoves.ToString();
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
