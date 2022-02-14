using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneHandler : MonoBehaviour
{
    public GameObject MainMenu, LevelSelect, AboutTheGameMenu, SettingsMainMenu;
    bool LevelSelectOn, AboutTheGameOn, MenuSettingsOn = false;
    string temp;
    public AudioSource ClickSound;

    /*
    Yes, this is the worst code I have ever written.
    There is 0 orginization within this code.
    Maybe I will get around to orginizing it.
    I am so sorry for anyone reading this.
    */

    public void PlayButtonPressed()
    {
        if (LevelSelectOn)
        {
            MainMenu.SetActive(true);
            LevelSelect.SetActive(false);
        }
        else
        {
            MainMenu.SetActive(false);
            LevelSelect.SetActive(true);
        }
        ClickSound.Play();
        LevelSelectOn = !LevelSelectOn;
    }

    public void reLoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void LevelSelectLevelOptionClicked()
    {
        temp = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.name;
        temp.Substring(4);
        SceneManager.LoadScene(temp);
    }

    public void AboutTheGameButtonPressed()
    {
        if (AboutTheGameOn)
        {
            MainMenu.SetActive(true);
            AboutTheGameMenu.SetActive(false);
        }
        else
        {
            MainMenu.SetActive(false);
            AboutTheGameMenu.SetActive(true);
        }
        ClickSound.Play();
        AboutTheGameOn = !AboutTheGameOn;
    }

    public void SettingsButtonPressed()
    {
        if (MenuSettingsOn)
        {
            MainMenu.SetActive(true);
            SettingsMainMenu.SetActive(false);
        }
        else
        {
            MainMenu.SetActive(false);
            SettingsMainMenu.SetActive(true);
        }
        ClickSound.Play();
        MenuSettingsOn = !MenuSettingsOn;
    }
}
