using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] TMP_Text papersReleasedText;
    [SerializeField] TMP_Text timeSurvivedText;

    [SerializeField] TMP_Text foundsGainedText;

    [SerializeField] float timeToGetFunds = 1f;

    int foundsGained = 0;
    int paperCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        papersReleasedText.text = PlayerPrefs.GetInt("ReleasedPapers").ToString();
        timeSurvivedText.text = PlayerPrefs.GetFloat("TimeSurvived").ToString("F0") + "s";

        foundsGained = 0;

        StartCoroutine(GainFounds());
    }

    IEnumerator GainFounds()
    {
        paperCount = PlayerPrefs.GetInt("ReleasedPapers");

        foundsGainedText.text = foundsGained.ToString();

        for (int i = 0; i < paperCount; i++)
        {
            int rand = Random.Range(-5, 6);

            foundsGained += 20 + rand;

            foundsGainedText.text = foundsGained.ToString();

            yield return new WaitForSeconds(timeToGetFunds / paperCount);
        }

        PlayerPrefs.SetInt("ReleasedPapers", 0);
        PlayerPrefs.SetInt("FoundsGained", foundsGained);
    }

    public void GoToMenu()
    {
        PlayerPrefs.SetInt("ReleasedPapers", 0);
        PlayerPrefs.SetInt("FoundsGained", foundsGained + paperCount * 20);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
