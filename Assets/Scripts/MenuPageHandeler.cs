using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MenuPageHandeler : MonoBehaviour
{
    public Animator transition;

    public CanvasHandlerLevel CanvasHandlerLevel;

    public Sprite ActivatedSprite;
    public Sprite DeActivatedSprite;

    GameObject TopButton;
    GameObject MiddleButton;
    GameObject BottomButton;

    GameObject HomePage;
    GameObject SettingsPage;
    GameObject HelpPage;

    // Change these to the X value of the buttons, the sprites are misaligned slighty and this is a (bad) fix
    private float ActivatedUIXValue = 464.2f;
    private float DeactivatedUIXValue = 466.4f;

    void Start()
    {
        TopButton = FindInActiveObjectByName("TopBackboard");
        MiddleButton = FindInActiveObjectByName("MiddleBackboard");
        BottomButton = FindInActiveObjectByName("BottomBackboard");

        HomePage = FindInActiveObjectByName("HomePage");
        SettingsPage = FindInActiveObjectByName("SettingsPage");
        HelpPage = FindInActiveObjectByName("HelpPage");
    }

    public void HelpButton()
    {
        HomePage.SetActive(false);
        SettingsPage.SetActive(false);
        HelpPage.SetActive(true);
        TopButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(DeactivatedUIXValue, 207f, 0f);
        MiddleButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(DeactivatedUIXValue, 132f, 0f);
        BottomButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(ActivatedUIXValue, 57f, 0f);
        TopButton.GetComponent<Image>().sprite = DeActivatedSprite;
        MiddleButton.GetComponent<Image>().sprite = DeActivatedSprite;
        BottomButton.GetComponent<Image>().sprite = ActivatedSprite;
        CanvasHandlerLevel.PlayClickSound();
    }

    public void HomeButton()
    {
        HomePage.SetActive(true);
        SettingsPage.SetActive(false);
        HelpPage.SetActive(false);
        TopButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(ActivatedUIXValue, 207f, 0f);
        MiddleButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(DeactivatedUIXValue, 132f, 0f);
        BottomButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(DeactivatedUIXValue, 57f, 0f);
        TopButton.GetComponent<Image>().sprite = ActivatedSprite;
        MiddleButton.GetComponent<Image>().sprite = DeActivatedSprite;
        BottomButton.GetComponent<Image>().sprite = DeActivatedSprite;
        CanvasHandlerLevel.PlayClickSound();
    }

    public void OptionsButton()
    {
        HomePage.SetActive(false);
        SettingsPage.SetActive(true);
        HelpPage.SetActive(false);
        TopButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(DeactivatedUIXValue, 207f, 0f);
        MiddleButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(ActivatedUIXValue, 132f, 0f);
        BottomButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(DeactivatedUIXValue, 57f, 0f);
        TopButton.GetComponent<Image>().sprite = DeActivatedSprite;
        MiddleButton.GetComponent<Image>().sprite = ActivatedSprite;
        BottomButton.GetComponent<Image>().sprite = DeActivatedSprite;
        CanvasHandlerLevel.PlayClickSound();
    }

    // Finds inactive GameObject, unity for some reason doesnt let GameObject.Find work on inactive gameObjects
    GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }

    public void GoMainMenu()
    {
        StartCoroutine(AnimationLoad("Main Menu"));
    }

    // Reloads current Scene, very simple
    public void RestartLevel()
    {
        StartCoroutine(AnimationLoad(SceneManager.GetActiveScene().name));
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
