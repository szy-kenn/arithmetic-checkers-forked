using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

namespace Damath
{
    public class LobbyManager : NetworkBehaviour
    {
        public Lobby Lobby;
        [SerializeField] private Image readyBotImage;
        [SerializeField] private Image readyTopImage;
        [SerializeField] private ToggleButton button;

        void Awake()
        {
            Game.Events.OnLobbyHost += JoinHost;
            Game.Events.OnLobbyJoin += GiveLobby;
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsServer)
            {
                button.SetText("Start");
                button.Tooltip.SetText("Start match when opponent is ready.");
                Game.Network.OnClientConnectedCallback += OnClientConnectedCallback;
            } else
            {
                button.SetText("Ready");
                button.Tooltip.SetText("Ready to let host begin match.");
            }
        }

        public void JoinHost(Lobby lobby)
        {
            Lobby = lobby;
            Lobby.ConnectPlayer(Game.Network.LocalClientId);
            Lobby.SetHost(Game.Network.LocalClientId);
        }

        public void GiveLobby(Lobby lobby)
        {
            if (IsServer) return;
            if (lobby == null) return;

            ReceiveLobbyClientRpc((int)lobby.Ruleset.Mode);
        }

        [ClientRpc]
        public void ReceiveLobbyClientRpc(int mode, ClientRpcParams clientRpcParams = default)
        {
            if (IsServer) return;
            Game.Console.Log($"Received lobby with mode {(Ruleset.Type)mode}");
        }

        private void OnClientConnectedCallback(ulong clientId)
        {
            if (Lobby == null) return;
            if (Lobby.Host == clientId) return;

            if (IsServer)
            {
                if (Lobby.HasPlayer(clientId)) return;
                
                Lobby.ConnectPlayer(clientId);
                Game.Console.Log("Client joined with id " + clientId);
                
                ClientRpcParams clientRpcParams = new()
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[]{clientId}
                    }
                };

                if (Lobby.Ruleset.Mode != Ruleset.Type.Custom)
                {
                    ReceiveLobbyInfoClientRpc((int)Lobby.Ruleset.Mode, clientRpcParams);
                } else
                {
                    ReceiveLobbyInfoClientRpc(Lobby.Ruleset, clientRpcParams);
                }
            }
        }
        
        [ClientRpc]
        public void ReceiveLobbyInfoClientRpc(int mode, ClientRpcParams clientRpcParams)
        {
            if (IsOwner) return;
            Lobby.SetRuleset((Ruleset.Type)mode);
        }

        /// <summary>
        /// For custom matches.
        /// </summary>
        [ClientRpc]
        public void ReceiveLobbyInfoClientRpc(Ruleset ruleset, ClientRpcParams clientRpcParams)
        {
            //
        }

        public void ToggleReady(bool value)
        {
            if (IsServer)
            {
                CheckOpponentReady();
            } else
            {
                readyBotImage.gameObject.SetActive(value);
                SendOpponentReadyStatusServerRpc(value);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SendOpponentReadyStatusServerRpc(bool value, ServerRpcParams serverRpcParams = default)
        {
            var clientId = serverRpcParams.Receive.SenderClientId;

            if (Lobby.HasPlayer(clientId))
            {
                Lobby.SetOpponentReady(value);

                if (IsServer)
                {
                    readyTopImage.gameObject.SetActive(value);
                    Game.Console.Log($"Client id {clientId} ready: {value}");
                }
            }
        }

        public void CheckOpponentReady()
        {
            if (Lobby == null) return;

            if (Lobby.OpponentIsReady)
            {
                Game.Network.OnClientConnectedCallback -= OnClientConnectedCallback;
                BeginMatchClientRpc();
            }
        }

        [ClientRpc]
        public void BeginMatchClientRpc(ClientRpcParams clientRpcParams = default)
        {
            button.gameObject.SetActive(false);
            readyBotImage.gameObject.SetActive(false);
            readyTopImage.gameObject.SetActive(false);
            Game.Console.Log("Starting match");
            Game.Main.StartMatch();
        }

        [ServerRpc]
        public void HadJoinedLobbyServerRpc(ServerRpcParams serverRpcParams = default)
        {

        }
    }
}
