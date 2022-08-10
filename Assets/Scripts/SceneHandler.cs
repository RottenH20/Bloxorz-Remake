using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneHandler : MonoBehaviour
{
    public Animator transition;
    public GameObject MainMenu, LevelSelect, AboutTheGameMenu, SettingsMainMenu, playerBlock;
    bool LevelSelectOn, AboutTheGameOn, MenuSettingsOn = false;
    string temp;
    public AudioSource ClickSound;

    /*
    Yes, this is the worst code I have ever written.
    There is 0 orginization within this code.
    Maybe I will get around to orginizing it.
    I am so sorry for anyone reading this.

    2/28/22
    Code cleaned, no longer "that" bad
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
        StartCoroutine(AnimationLoad(SceneManager.GetActiveScene().name));
    }

    public void returnToMainMenu()
    {
        StartCoroutine(AnimationLoad("Main Menu"));
    }

    public void LevelSelectLevelOptionClicked() 
    {
        temp = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.name;
        temp.Substring(4);
        StartCoroutine(AnimationLoad(temp));
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

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            FindObjectOfType<AudioControl>().PlayMusic("MainMenuMusic");
        }
        else
        {
            FindObjectOfType<AudioControl>().PlayMusic("GameMusic");
        }
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

    IEnumerator AnimationLoad(string sceneName)
    {
        // Play animation
        transition.SetTrigger("Start");
        // Wait
        yield return new WaitForSeconds(1); // Change depending on transistion time
        //Load Scene
        SceneManager.LoadScene(sceneName);
    }
}
