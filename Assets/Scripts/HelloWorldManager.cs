using System;
using Unity.Netcode;
using UnityEngine;

public class HelloWorldManager : MonoBehaviour
{
  private void OnGUI()
  {
    GUILayout.BeginArea(new Rect(10, 10, 300, 300));
    if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
    {
      StartButtons();
    }
    else
    {
      StatusLabels();
      SubmitNewPosition();
    }
  }

  static void StartButtons()
  {
    if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
    if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
    if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
  }

  static void StatusLabels()
  {
    string mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
    
    GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
    GUILayout.Label("Mode: " + mode);
  }

  static void SubmitNewPosition()
  {
    if (!GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change")) return;
    
    if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
    {
      foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
      {
        NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<HelloWorldPlayer>().Move();
      }
    }
    else
    {
      NetworkObject playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
      var player = playerObject.GetComponent<HelloWorldPlayer>();
      player.Move();
    }
  }
}