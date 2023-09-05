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
        public TextMeshProUGUI BlueScore;
        public TextMeshProUGUI OrangeScore;

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
                Game.Console.Log($"[DEBUG]: Scoreboard initialized ");
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
            BlueScore.text = Players[Side.Bot].Score.ToString();
            OrangeScore.text = Players[Side.Top].Score.ToString();
        }

        public void Compute(Move move)
        {
            float score;
            char operation;
            float x = float.Parse(move.capturingPiece.Value);
            float y = float.Parse(move.capturedPiece.Value);

            (score, operation) = move.destinationCell.Operation switch
            {
                Operation.Add => (x + y, '+'),
                Operation.Sub => (x - y, '-'),
                Operation.Mul => (x * y, '×'),
                Operation.Div => (x / y, '÷'),
                _ => throw new NotImplementedException(),
            };

            Game.Console.Log($"[ACTION]: {move.capturingPiece.Value} {operation} {move.capturedPiece.Value} = {score}");

            move.SetScoreValue(score);
            AddScore(move.capturingPiece.Owner, score);
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