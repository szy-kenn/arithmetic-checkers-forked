using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.Netcode;
using UnityEngine;

namespace Damath
{
    public class LobbyRequestData : INetworkSerializable
    {
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            
        }
    }
    /// <summary>
    /// Lobby before the match begins.
    /// </summary>
    public class Lobby
    {
        public int Id { get ; private set; }
        public int MaximumPlayers = 2;
        public bool IsPrivate { get; private set; }
        public bool IsFull { get; private set; }
        public bool AllowExcess = true;
        public string Password { get; private set; }
        public List<ulong> ConnectedClients = new();
        public ulong Host;
        public ulong Opponent;
        public bool OpponentIsReady;
        public Ruleset Ruleset { get; private set; }

        public Lobby(int id, bool isPrivate)
        {
            Id = id;
            IsPrivate = isPrivate;
            IsFull = false;
        }

        // public LobbyRequestData GetLobbyInfo()
        // {
        //     LobbyRequestData lobbyRequestData = new()
        //     {

        //     };         
        // }

        public void SetPrivacy(bool value)
        {
            IsPrivate = value;
        }

        public void SetHost(ulong clientId)
        {
            Host = clientId;
        }

        public void ConnectPlayer(ulong clientId)
        {
            ConnectedClients.Add(clientId);
            if (ConnectedClients.Count == MaximumPlayers) IsFull = true;
            Game.Events.LobbyJoin(this);
        }
        
        public bool HasPlayer(ulong clientId)
        {
            if (ConnectedClients.Contains(clientId)) return true;
            return false;
        }

        public void SetOpponentReady(bool isReady)
        {
            OpponentIsReady = isReady;
        }

        public void Start()
        {
            Game.Events.LobbyStart(this);
        }

        public void SetRuleset(Ruleset.Type type)
        {
            Ruleset = new(type);
        }
    }
}
