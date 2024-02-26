using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    // Setting that shows tutorials
    public bool tutorialOn = true;

    MenuPageHandeler MenuPageHandeler;

    // These will store each tutorial image in order, if multiple images per level, use 2D array
    // Ex: Level1 Tutorial, tutorials[0][0], tutorials[0][1], etc...
    GameObject[][] tutorials;

    void Start()
    {

    }

    public bool checkTutorial() // Checks to see if a tutorial should be played on the given level. (DOES NOT ACTUALLY RUN THE TUTORIAL)
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") // Make sure we are not on MainMenu
            return false;

        string sceneNum = (SceneManager.GetActiveScene().name.Substring(5));

        if (PlayerPrefs.GetString(sceneNum) == "T") // If we have already showed the tutorial on this level
            return false;

        PlayerPrefs.SetString(sceneNum, "T"); // Set to true now that they have seen the tutorial
        return true;
    }

    public void replayTutorial() // Restarts the level with the tutorial
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") // Make sure we are not on MainMenu
            return;

        string sceneNum = (SceneManager.GetActiveScene().name.Substring(5));

        PlayerPrefs.SetString(sceneNum, "F"); // to replay tutorial we just re do the scene

        MenuPageHandeler = GameObject.FindObjectOfType<MenuPageHandeler>();
        MenuPageHandeler.RestartLevel();
    }
}
