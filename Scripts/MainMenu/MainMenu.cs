using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Text _bestScoreText;
    [SerializeField]
    private Text _currentLevelText;

    void Start()
    {
        //PlayerPrefs.DeleteAll(); 

        string currentLevel = PlayerPrefs.GetString("CurrentLevel");
        int bestScore = 0;

        if (string.IsNullOrEmpty(currentLevel))
        {
            currentLevel = "EASY";
            PlayerPrefs.SetString("CurrentLevel", "EASY");
        }

        _currentLevelText.text = "Current Level: " + currentLevel;

        if (currentLevel == "EASY")
        {
            bestScore = PlayerPrefs.GetInt("BestScoreEasy");
        }
        else if (currentLevel == "MEDIUM")
        {
            bestScore = PlayerPrefs.GetInt("BestScoreMedium");
        }
        else if (currentLevel == "HARD")
        {
            bestScore = PlayerPrefs.GetInt("BestScoreHard");
        }

        _bestScoreText.text = "Best Score: " + bestScore.ToString();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1); //game scene
    }

    public void SelectLevel()
    {
        SceneManager.LoadScene(2); //level select scene
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
