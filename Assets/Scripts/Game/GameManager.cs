using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Hazard> possibleHazards;
    public List<string> verses;
    public int startingLives = 6;

    private int _currentLevel = 1;
    public int CurrentLevel {
        get { return _currentLevel; }
        set { _currentLevel = value; }
    }

    private List<Hazard> _currentHazards;
    public List<Hazard> CurrentHazards
    {
        get { return _currentHazards; }
    }

    private int _lives;
    public int Lives
    {
        get { return _lives; }
        private set { _lives = value; }
    }

    private int _score = 0;
    public int Score
    {
        get { return _score; }
        set { _score = value; }
    }

    private int _highScore = 0;
    public int HighScore
    {
        get { return _highScore; }
        set { _highScore = value; }
    }

    private int _scorePerJump = 5;
    public int ScorePerJump
    {
        get { return _scorePerJump; }
        set { _scorePerJump = value; }
    }

    private string _currentVerse;
    public string CurrentVerse
    {
        get { return _currentVerse; }
        set { _currentVerse = value; }
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        if(_currentHazards == null) _currentHazards = new List<Hazard>();

        Lives = startingLives;
    }

    public void NextLevel()
    {
        CurrentLevel += 1;
        CurrentHazards.Add(possibleHazards[Random.Range(0, possibleHazards.Count)]);
        ScorePerJump += 5;
        CurrentVerse = verses[CurrentLevel - 2];
    }

    public void BackToLevel1()
    {
        CurrentLevel = 1;
        CurrentHazards.Clear();
        ScorePerJump = 5;
        Score = 0;
        Lives = startingLives;
    }

    public void LoseLife()
    {
        Lives -= 1;
    }
}
