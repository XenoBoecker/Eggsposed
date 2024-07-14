using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    public Leaderboard leaderboard;
    public TMP_Text leaderboardText;

    void Start()
    {
        // Assuming the Leaderboard script is attached to a GameObject named "Leaderboard"
        leaderboard = GameObject.Find("Leaderboard").GetComponent<Leaderboard>();
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
}
