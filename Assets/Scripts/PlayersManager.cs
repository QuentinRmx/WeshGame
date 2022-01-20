using Common;
using Unity.Netcode;
using UnityEngine;

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
      Debug.Log($"Player {id} just connected...");
    };
    
    NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
    {
      if (!IsServer) return;
      _playersInGame.Value--;
      Debug.Log($"Player {id} just disconnected...");
    };
  }
}