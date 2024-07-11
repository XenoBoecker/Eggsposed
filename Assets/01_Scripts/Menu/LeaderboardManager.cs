using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    public Leaderboard leaderboard;
    public TMP_InputField playerNameInput;
    public Button submitScoreButton;
    public Button resetScoresButton;
    public TMP_Text leaderboardText;
    private int lastGameScore = 0; // Assume this is set when the game ends

    void Start()
    {
        // Assuming the Leaderboard script is attached to a GameObject named "Leaderboard"
        leaderboard = GameObject.Find("Leaderboard").GetComponent<Leaderboard>();
        leaderboard.LoadLeaderboard();
        UpdateLeaderboardUI();

        submitScoreButton.onClick.AddListener(OnSubmitScore);
        resetScoresButton.onClick.AddListener(ResetScores);

        GameOver(Random.Range(5, 20));
    }

    public void GameOver(int score)
    {
        lastGameScore = score;
        playerNameInput.gameObject.SetActive(true);
        submitScoreButton.gameObject.SetActive(true);
    }

    public void OnSubmitScore()
    {
        string playerName = playerNameInput.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            leaderboard.AddPlayer(playerName, lastGameScore);
            playerNameInput.gameObject.SetActive(false);
            submitScoreButton.gameObject.SetActive(false);
            UpdateLeaderboardUI();
        }
    }

    public void ResetScores()
    {
        leaderboard.ResetLeaderboard();
        UpdateLeaderboardUI();
    }

    public void UpdateLeaderboardUI()
    {
        List<string> currentLeaderboard = leaderboard.GetLeaderboard();
        leaderboardText.text = "Leaderboard:\n";
        foreach (string entry in currentLeaderboard)
        {
            leaderboardText.text += entry + "\n";
        }
    }

    void OnApplicationQuit()
    {
        // Save the leaderboard when the application quits
        leaderboard.SaveLeaderboard();
    }
}
