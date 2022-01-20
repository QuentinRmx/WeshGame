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

    public Button EnablePhysicsButton;

    private bool _hasServerStarted;

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

        NetworkManager.Singleton.OnServerStarted += () => _hasServerStarted = true;
        EnablePhysicsButton.onClick.AddListener(() =>
        {
            if (!_hasServerStarted)
            {
                Debug.Log("Server has not started, physics objects spawning is impossible...");
                return;
            }

            SpawnerController.Instance.SpawnObjects();
        });
    }

    private void EnableButtons(bool isEnabled)
    {
        Debug.Log("Disabled");
        PanelButtons.gameObject.SetActive(isEnabled);
    }
}