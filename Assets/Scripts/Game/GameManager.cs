using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Hazard> possibleHazards;

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

    void Awake()
    {
        if (Instance == null) Instance = this;
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        if(_currentHazards == null) _currentHazards = new List<Hazard>();

        NextLevel();
        SceneManager.LoadScene("Game");
    }

    public void NextLevel()
    {
        CurrentLevel += 1;
        CurrentHazards.Add(possibleHazards[Random.Range(0, possibleHazards.Count)]);
    }
}
