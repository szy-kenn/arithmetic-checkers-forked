using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public GameObject botScore;
    public GameObject topScore;
    public GameObject timer;
    public GameObject gamemode;

    TextMeshProUGUI botScoreText;
    TextMeshProUGUI topScoreText;

    void Awake()
    {
        botScoreText = botScore.GetComponent<TextMeshProUGUI>();
        topScoreText = topScore.GetComponent<TextMeshProUGUI>();
    }

    public void Init()
    {
        foreach (Player player in Game.Main.players)
        {
            if (player != null)
            {
                player.score = 0f;
            }
        }
        Refresh();
    }

    public void Refresh()
    {
        botScoreText.text = Game.Main.players[0].score.ToString();
        topScoreText.text = Game.Main.players[1].score.ToString();
    }

    public void Compute(Move move)
    {
        float score = 0;
        float x = float.Parse(move.capturingPiece.value);
        float y = float.Parse(move.capturedPiece.value);

        switch (move.destinationCell.operation)
        {
            case Operation.Add:
                score = x + y;
                break;
            case Operation.Sub:
                score = x - y;
                break;
            case Operation.Mul:
                score = x * y;
                break;
            case Operation.Div:
                score = x / y;
                break;
        }

        Debug.Log($"[ACTION]: {move.capturingPiece} captured {move.capturedPiece} for {score}");
        move.score = score;
        Add(move.capturingPiece.owner, score);
        Refresh();
    }

    public void Add(Player player, float value)
    {
        player.score += value;
    }

    public void Remove(Player player, float value)
    {
        player.score -= value;
    }

    public float GetScore(Player player)
    {
        return player.score;
    }
}
