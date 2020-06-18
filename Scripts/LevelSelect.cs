using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0); //main menu
        }
    }

    public void EasyButtonClick()
    {
        PlayerPrefs.SetString("CurrentLevel", "EASY");
        SceneManager.LoadScene(0); //main menu
    }

    public void MediumButtonClick()
    {
            PlayerPrefs.SetString("CurrentLevel", "MEDIUM");
            SceneManager.LoadScene(0); //main menu
    }

    public void HardButtonClick()
    {
        PlayerPrefs.SetString("CurrentLevel", "HARD");
        SceneManager.LoadScene(0); //main menu
    }
}
