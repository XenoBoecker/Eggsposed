using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using static Leaderboard;

public class LeaderboardManager : MonoBehaviour
{
    GameOver gameOverManager;


    [SerializeField] GameObject leaderboardPanel;


    [SerializeField] Transform playerScoreShowParent;

    [SerializeField] PlayerScoreShow playerScoreShowPrefab;

    List<PlayerScoreShow> playerScoreShowObjects = new List<PlayerScoreShow>();

    public Leaderboard leaderboard;
    public TMP_InputField playerNameInput;
    public Button submitScoreButton;
    public Button resetScoresButton;
    public Button menuButton;
    private int lastGameScore = 0; // Assume this is set when the game ends

    private void Awake()
    {
        leaderboardPanel.SetActive(false);
    }

    void Start()
    {
        gameOverManager = FindObjectOfType<GameOver>();
        gameOverManager.OnShowLeaderboard += ShowLeaderboardCanvas;
        
        // Assuming the Leaderboard script is attached to a GameObject named "Leaderboard"
        leaderboard = GetComponent<Leaderboard>();
        leaderboard.LoadLeaderboard();
        UpdateLeaderboardUI();

        submitScoreButton.onClick.AddListener(OnSubmitScore);
        resetScoresButton.onClick.AddListener(ResetScores);
        menuButton.onClick.AddListener(GoToMenu);

        GameOver(GameOverInfo.GetBredChickens().Count);
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void ShowLeaderboardCanvas()
    {
        leaderboardPanel.SetActive(true);
        if (PlayerPrefs.GetInt("OnlyShowLeaderboard") == 1)
        {
            playerNameInput.gameObject.SetActive(false);
            submitScoreButton.gameObject.SetActive(false);
        }
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
        List<Leaderboard.Player> currentLeaderboard = leaderboard.GetPlayerScores();

        foreach (PlayerScoreShow playerScoreShow in playerScoreShowObjects)
        {
            Destroy(playerScoreShow.gameObject);
        }

        playerScoreShowObjects.Clear();
        for (int i = 0; i < currentLeaderboard.Count; i++)
        {
            PlayerScoreShow playerScoreShow = Instantiate(playerScoreShowPrefab, playerScoreShowParent);
            playerScoreShow.SetPlayer(currentLeaderboard[i], i+1);
            playerScoreShowObjects.Add(playerScoreShow);
        }
    }

    void OnApplicationQuit()
    {
        // Save the leaderboard when the application quits
        leaderboard.SaveLeaderboard();
    }
}
