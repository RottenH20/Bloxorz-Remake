using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasHandlerLevel : MonoBehaviour
{
    FloorComponent FloorComponent;

    [SerializeField] GameObject SettingsMenu = null;
    private GameObject SettingsButton, InputManager;
    private TextMeshProUGUI NumberOfMovesUI;
    bool settingsOn = false;

    AudioSource ClickSound;
    CharacterController CharacterController;

    bool vertical = false;

    private GameObject mainPlayer;
    private int size = 7; // Change size of camera when vertical here

    [Range(1, 5)]
    public short numberOfFloors = 1;
    GameObject[] floors = new GameObject[6]; // 0 won't be used
    List<MeshRenderer> barriers = new List<MeshRenderer>(); // Always 1 less than numberOfFloors

    int currentFloor;

    void Start()
    {
        ClickSound = this.transform.GetComponent<AudioSource>();
        FloorComponent = GameObject.FindObjectOfType<FloorComponent>();
        NumberOfMovesUI = GameObject.Find("NumberOfMovesUI").GetComponent<TextMeshProUGUI>();
        mainPlayer = GameObject.FindWithTag("Player");
        SettingsButton = this.transform.Find("SettingsButtonBack").gameObject;
        InputManager = GameObject.FindObjectOfType<InputManager>().gameObject;
        CharacterController = GameObject.FindObjectOfType<CharacterController>();
        for (int i = 1; i <= numberOfFloors; i++)
        {
            floors[i] = GameObject.Find("Floor" + i.ToString());
        }
        if (numberOfFloors != 1)
            foreach (MeshRenderer barrierObj in GameObject.FindGameObjectWithTag("Barrier").transform.GetComponentsInChildren<MeshRenderer>())
            {
                barriers.Add(barrierObj);
            }
    }

    public void movingPlatformCameraBug()
    {
        if (vertical)
            swapCamera();
    }

    public void swapCamera()
    {
        vertical = !vertical;
        if (vertical)
        {
            getCurrentFloor();
            for (int i = 1; i <= numberOfFloors; i++)
            {
                if (currentFloor < i)
                    floors[i].SetActive(false);
                if (currentFloor > i)
                {
                    barriers[i-1].enabled = true;
                }
            }
        }
        else
        {
            for (int i = 1; i <= numberOfFloors; i++)
            {
                floors[i].SetActive(true);
            }
            for (int i = 1; i < numberOfFloors; i++)
            {
                if (numberOfFloors > 1)
                    barriers[i - 1].enabled = false;
            }
            switch (FloorComponent.startingFloor)
            {
                case 1:
                    Camera.main.transform.position = new Vector3(15, 16, -15);
                    break;
                case 2:
                    Camera.main.transform.position = new Vector3(15, 26, -15);
                    break;
                case 3:
                    Camera.main.transform.position = new Vector3(15, 36, -15);
                    break;
                default:
                    Debug.Log("SHOULD NEVER DISPLAY!!!");
                    break;
            }
            Camera.main.transform.rotation = Quaternion.Euler(35, -45, 0);
            Camera.main.orthographicSize = 5;
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

    void getCurrentFloor() // Returns what floor the current player is on
    {
        currentFloor = Mathf.FloorToInt((int)mainPlayer.transform.position.y / 10) + 1;
    }
}
