using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Damath
{
    public class Scoreboard : MonoBehaviour
    {
        public Ruleset Rules  { get; private set; }
        public Dictionary<Side, Player> Players = new Dictionary<Side, Player>();
        public TextMeshProUGUI blueScore;
        public TextMeshProUGUI orangeScore;

        void OnEnable()
        {
            Game.Events.OnMatchBegin += Init;
            Game.Events.OnRulesetCreate -= ReceiveRuleset;
            Game.Events.OnPlayerCreate += AddPlayer;
            Game.Events.OnPieceCapture += Compute;
        }

        void OnDisable()
        {
            Game.Events.OnMatchBegin -= Init;
            Game.Events.OnRulesetCreate -= ReceiveRuleset;
            Game.Events.OnPlayerCreate -= AddPlayer;
            Game.Events.OnPieceCapture -= Compute;
        }

        public void Init(MatchController match)
        {
            foreach (var kv in Players)
            {
                Player player = kv.Value;
                if (player != null)
                {
                    player.SetScore(0f);
                }
            }
            
            if (Settings.EnableDebugMode)
            {
                Game.Console.Log($"[SCOREBOARD]: Done initialization " + this);
            }
        }

        public void ReceiveRuleset(Ruleset rules)
        {
            Rules = rules;
        }        

        public void AddPlayer(Player player)
        {
            Players.Add(player.Side, player);
        }


        public void Refresh()
        {
            blueScore.text = Players[Side.Bot].Score.ToString();
            orangeScore.text = Players[Side.Top].Score.ToString();
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

            Game.Console.Log($"[ACTION]: {move.capturingPiece} captured {move.capturedPiece} for {score}");
            move.SetScoreValue(score);
            AddScore(move.capturingPiece.owner, score);
            Refresh();
        }

        public void AddScore(Player player, float value)
        {
            player.Score += value;
        }

        public void RemoveScore(Player player, float value)
        {
            player.Score -= value;
        }

        public float GetScore(Player player)
        {
            return player.Score;
        }
    }
}