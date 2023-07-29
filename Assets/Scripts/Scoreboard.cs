using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Damath
{
    public class Scoreboard : MonoBehaviour
    {
        public Ruleset Rules;
        public Dictionary<Side, Player> Players = new Dictionary<Side, Player>();
        public TextMeshProUGUI blueScore;
        public TextMeshProUGUI orangeScore;

        void OnEnable()
        {
            Game.Events.OnMatchBegin += Init;
            Game.Events.OnPlayerCreate += AddPlayer;
            Game.Events.OnPieceCapture += Compute;
        }

        void OnDisable()
        {
            Game.Events.OnMatchBegin -= Init;
            Game.Events.OnPlayerCreate -= AddPlayer;
            Game.Events.OnPieceCapture -= Compute;
        }

        public void Init(Ruleset rules)
        {
            this.Rules = rules;
            blueScore = UIHandler.Main.ScoreboardUI.scoreBlue.GetComponent<TextMeshProUGUI>();
            orangeScore = UIHandler.Main.ScoreboardUI.scoreOrange.GetComponent<TextMeshProUGUI>();

            foreach (var kv in Players)
            {
                Player player = kv.Value;
                if (player != null)
                {
                    player.SetScore(0f);
                }
            }
            Refresh();
        }

        public void AddPlayer(Player player)
        {
            Console.Log($"[SCOREBOARD]: Created {player}");
            Players.Add(player.side, player);
        }


        public void Refresh()
        {
            blueScore.text = Players[Side.Bot].score.ToString();
            orangeScore.text = Players[Side.Top].score.ToString();
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

            Console.Log($"[ACTION]: {move.capturingPiece} captured {move.capturedPiece} for {score}");
            move.SetScoreValue(score);
            AddScore(move.capturingPiece.owner, score);
            Refresh();
        }

        public void AddScore(Player player, float value)
        {
            player.score += value;
        }

        public void RemoveScore(Player player, float value)
        {
            player.score -= value;
        }

        public float GetScore(Player player)
        {
            return player.score;
        }
    }
}