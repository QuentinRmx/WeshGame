using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
  public Button StartServerButton;

  public Button StartHostButton;

  public Button StartClientButton;

  public TextMeshProUGUI PlayersInGameTMP;

  public GameObject PanelButtons;

  private void Awake()
  {
    Cursor.visible = true;
  }

  private void Update()
  {
    PlayersInGameTMP.text = $"Players in game: {PlayersManager.Instance.PlayersInGame}";
  }

  private void Start()
  {
    StartHostButton.onClick.AddListener(() =>
    {
      if (NetworkManager.Singleton.StartHost())
      {
        Debug.Log("Host started...");
        EnableButtons(false);
      }
      else
      {
        Debug.Log("Host could not be started...");
      }
    });
    
    StartServerButton.onClick.AddListener(() =>
    {
      if (NetworkManager.Singleton.StartServer())
      {
        Debug.Log("Server started...");
        EnableButtons(false);
      }
      else
      {
        Debug.Log("Server could not be started...");
      }
    });
    
    StartClientButton.onClick.AddListener(() =>
    {
      if (NetworkManager.Singleton.StartClient())
      {
        Debug.Log("Client started...");
        EnableButtons(false);
      }
      else
      {
        Debug.Log("Client could not be started...");
      }
    });
  }

  private void EnableButtons(bool isEnabled)
  {
    // TODO: disable buttons for the client.
    Debug.Log("Disabled");
    PanelButtons.gameObject.SetActive(isEnabled);
    // StartClientButton.enabled = isEnabled;
    // StartHostButton.enabled = isEnabled;
    // StartServerButton.enabled = isEnabled;
  }

}