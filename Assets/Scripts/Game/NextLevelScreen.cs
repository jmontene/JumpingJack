using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextLevelScreen : MonoBehaviour
{
    public Text hazardsText;
    public Text verseText;
    public GameObject extraLifeObject;

    public float textSpeed = 0.3f;
    public float timeToWaitForLevel = 2f;
    public string nextLevelName;
    public bool isWinScreen;

    void Start()
    {
        GameManager.Instance.NextLevel();
        if (!isWinScreen)
        {
            int hazards = GameManager.Instance.CurrentLevel - 1;
            string finalText = "NEXT LEVEL      --     " + hazards + "    HAZARD";
            if (hazards > 1) finalText += "S";
            hazardsText.text = finalText;

            int curLevel = GameManager.Instance.CurrentLevel;
            if (curLevel >= 7 && (curLevel - 2) % 5 == 0)
            {
                GameManager.Instance.Lives += 1;
                extraLifeObject.SetActive(true);
            }
            else
            {
                extraLifeObject.SetActive(false);
            }
        }
        else
        {
            GameManager.Instance.CurrentLevel -= 1;
        }
        
        verseText.text = "";
        StartCoroutine(ShowVerse());
    }

    IEnumerator ShowVerse()
    {
        string targetText = GameManager.Instance.CurrentVerse;
        for(int i = 1; i <= targetText.Length; ++i)
        {
            verseText.text = targetText.Substring(0, i);
            yield return new WaitForSeconds(textSpeed);
        }
        yield return new WaitForSeconds(timeToWaitForLevel);
        SceneManager.LoadScene(nextLevelName);
    }
}
