using TMPro;
using UnityEngine;

public class PlayerScoreShow : MonoBehaviour
{

    [SerializeField] NumberDisplay rankDisplay, scoreDisplay;

    [SerializeField] TMP_Text playerNameText, scoreText;

    internal void SetPlayer(Leaderboard.Player player, int rank)
    {
        playerNameText.text = player.name + GetNeededDashes(player.name);
        // scoreDisplay.SetNumber(player.score);
        scoreText.text = player.score.ToString();

        rankDisplay.SetNumber(rank);
    }

    private string GetNeededDashes(string playerName)
    {
        string originalText = playerName;
        string dashes = "";

        playerNameText.text = originalText;
        float maxWidth = playerNameText.rectTransform.rect.width;

        while (playerNameText.preferredWidth < maxWidth)
        {
            dashes += "-";
            playerNameText.text = originalText + dashes;

            if (playerNameText.preferredWidth >= maxWidth)
            {
                dashes = dashes.Remove(dashes.Length - 1);
                break;
            }
        }

        return dashes;
    }
}
