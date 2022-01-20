using Common;
using Unity.Netcode;
using UnityEngine;
using Logger = Common.Logger;

public class PlayersManager : NetworkSingleton<PlayersManager>
{
  private readonly NetworkVariable<int> _playersInGame = new();

  public int PlayersInGame => _playersInGame.Value;

  private void Start()
  {
    NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
    {
      if (!IsServer) return;
      _playersInGame.Value++;
      Logger.Instance.LogWarning($"Player {id} just connected...");
    };
    
    NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
    {
      if (!IsServer) return;
      _playersInGame.Value--;
      Logger.Instance.LogWarning($"Player {id} just disconnected...");
    };
  }
}