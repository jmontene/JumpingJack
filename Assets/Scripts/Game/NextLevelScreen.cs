using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextLevelScreen : MonoBehaviour
{
    public Text hazardsText;
    public Text verseText;

    public float textSpeed = 0.3f;
    public float timeToWaitForLevel = 2f;

    void Start()
    {
        GameManager.Instance.NextLevel();
        int hazards = GameManager.Instance.CurrentLevel - 1;
        string finalText = "NEXT LEVEL--     " + hazards + "    HAZARD";
        if (hazards > 1) finalText += "S";

        hazardsText.text = finalText;
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
        SceneManager.LoadScene("Game");
    }
}
