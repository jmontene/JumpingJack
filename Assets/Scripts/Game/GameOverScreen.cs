using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public Text reportText;
    public GameObject highScoreObject;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.HighScore < GameManager.Instance.Score)
        {
            GameManager.Instance.HighScore = GameManager.Instance.Score;
        }
        else
        {
            highScoreObject.SetActive(false);
        }

        int hazards = GameManager.Instance.CurrentLevel - 1;
        string scoreText = GameManager.Instance.Score.ToString("D5");
        reportText.text = "FINAL SCORE   " + scoreText + "\nWITH   " + hazards + "   HAZARDS";
        GameManager.Instance.BackToLevel1();
    }

    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            SceneManager.LoadScene("Game");
        }
    }
}
