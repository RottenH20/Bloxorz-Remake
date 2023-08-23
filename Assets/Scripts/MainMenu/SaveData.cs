using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveData : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject level;
    private Text text;
    private Button button;
    private int counter = -1;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            for (int i = 1; i <= 30; i++)
            {
                if (PlayerPrefs.HasKey(i.ToString()) && PlayerPrefs.GetInt(i.ToString()) != -1)
                {
                    level = GameObject.Find("Level" + i.ToString());
                    text = level.GetComponentInChildren<Text>();
                    text.color = Color.green;
                }
                else if (PlayerPrefs.HasKey(i.ToString()) && PlayerPrefs.GetInt(i.ToString()) == -1)
                {
                    if (counter == -1)
                        counter = i;
                    level = GameObject.Find("Level" + i.ToString());
                    text = level.GetComponentInChildren<Text>();
                    button = level.GetComponentInChildren<Button>();
                    button.enabled = false;
                    text.color = Color.red;
                }
                else
                {
                    counter = 1;
                    PlayerPrefs.SetInt(i.ToString(), -1);
                    PlayerPrefs.SetString(i.ToString(), "F"); // Used for tutorialCheck
                    level = GameObject.Find("Level" + i.ToString());
                    text = level.GetComponentInChildren<Text>();
                    button = level.GetComponentInChildren<Button>();
                    button.enabled = false;
                    text.color = Color.red;
                }
            }

            level = GameObject.Find("Level" + counter.ToString());
            text = level.GetComponentInChildren<Text>();
            text.color = Color.yellow;
            button = level.GetComponentInChildren<Button>();
            button.enabled = true;
        }

        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            // Check tutorial status here
        }
    }

    public void SaveLevel(int levelNumber, int totalMoves) // Save level
    {
        if (PlayerPrefs.GetInt(levelNumber.ToString()) < totalMoves || PlayerPrefs.GetInt(levelNumber.ToString()) == -1)
        {
            PlayerPrefs.SetInt(levelNumber.ToString(), totalMoves);
        }
    }

    public int LoadSpecificLevelData(int levelNumber) // Used for SaveLevel method, gets specific features of a level
    {
        return PlayerPrefs.GetInt(levelNumber.ToString());
    }
}