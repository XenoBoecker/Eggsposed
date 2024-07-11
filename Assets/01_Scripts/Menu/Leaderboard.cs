using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Leaderboard : MonoBehaviour
{
    // Define a player structure
    [System.Serializable]
    public class Player
    {
        public string name;
        public int score;

        public Player(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }

    // List to hold players
    public List<Player> players = new List<Player>();

    // Method to add a player
    public void AddPlayer(string name, int score)
    {
        Player player = new Player(name, score);
        players.Add(player);
        players.Sort((x, y) => y.score.CompareTo(x.score)); // Sort by score in descending order
        SaveLeaderboard(); // Save the leaderboard whenever a new player is added
    }

    // Method to update a player's score
    public void UpdatePlayerScore(string name, int newScore)
    {
        foreach (Player player in players)
        {
            if (player.name == name)
            {
                player.score = newScore;
                break;
            }
        }
        players.Sort((x, y) => y.score.CompareTo(x.score)); // Sort by score in descending order
    }

    // Method to get the leaderboard as a list of strings
    public List<string> GetLeaderboard()
    {
        List<string> leaderboard = new List<string>();
        foreach (Player player in players)
        {
            leaderboard.Add(player.name + ": " + player.score);
        }
        return leaderboard;
    }

    // Method to save the leaderboard to a file
    public void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(this);
        File.WriteAllText(Application.persistentDataPath + "/leaderboard.json", json);
    }

    // Method to load the leaderboard from a file
    public void LoadLeaderboard()
    {
        string path = Application.persistentDataPath + "/leaderboard.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, this);
            players.Sort((x, y) => y.score.CompareTo(x.score)); // Sort by score in descending order
        }
    }

    internal void ResetLeaderboard()
    {
        players.Clear();
    }
}
