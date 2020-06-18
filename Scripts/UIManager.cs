using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _bestScoreText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _LivesImage;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartLevelText;
    private bool isGameOver = false;
    private int _currentScore = 0;
    private int _bestScore = 0;
    private string currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: 0";
        currentLevel = PlayerPrefs.GetString("CurrentLevel");

        if (currentLevel == "EASY")
        {
            _bestScore = PlayerPrefs.GetInt("BestScoreEasy");
        }
        else if (currentLevel == "MEDIUM")
        {
            _bestScore = PlayerPrefs.GetInt("BestScoreMedium");
        }
        else if (currentLevel == "HARD")
        {
            _bestScore = PlayerPrefs.GetInt("BestScoreHard");
        }
        
        _bestScoreText.text = "Best Score: " + _bestScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1); //game scene
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CheckBestScore();
            SceneManager.LoadScene(0); //main menu
        }
    }

    public void updateScore(int points)
    {
        _currentScore += points;
        _scoreText.text = "Score: " + _currentScore.ToString();
    }

    public void CheckBestScore()
    {
        if (_currentScore > _bestScore)
        {
            _bestScore = _currentScore;

            _bestScoreText.text = "Best Score: " + _bestScore.ToString();

            if (currentLevel == "EASY")
            {
                PlayerPrefs.SetInt("BestScoreEasy", _bestScore);
            }
            else if (currentLevel == "MEDIUM")
            {
                PlayerPrefs.SetInt("BestScoreMedium", _bestScore);
            }
            else if (currentLevel == "HARD")
            {
                PlayerPrefs.SetInt("BestScoreHard", _bestScore);
            }
            
        }
    }

    public void UpdateLives(int lives)
    {
        _LivesImage.sprite = _liveSprites[lives];

        if (lives < 1)
        {
            _gameOverText.gameObject.SetActive(true);
            _restartLevelText.gameObject.SetActive(true);
            isGameOver = true;
            StartCoroutine(DisplayGameOver());
        }
    }

    IEnumerator DisplayGameOver()
    {
        while(isGameOver)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
