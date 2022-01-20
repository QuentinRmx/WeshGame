using Common;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;
using Logger = Common.Logger;

public class UIManager : Singleton<UIManager>
{
    public Button StartServerButton;

    public Button StartHostButton;

    public Button StartClientButton;

    public TextMeshProUGUI PlayersInGameTMP;

    public GameObject PanelButtons;

    public Button EnablePhysicsButton;

    public TMP_InputField TxtFieldRoomCode;

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
        StartHostButton.onClick.AddListener(async () =>
        {
            if (RelayManager.Instance.IsRelayEnabled)
            {
                await RelayManager.Instance.SetupRelay();
            }
            
            if (NetworkManager.Singleton.StartHost())
            {
                Logger.Instance.LogInfo("Host started...");
                EnableButtons(false);
            }
            else
            {
                Logger.Instance.LogError("Host could not be started...");
            }
        });

        StartServerButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartServer())
            {
                Logger.Instance.LogInfo("Server started...");
                EnableButtons(false);
            }
            else
            {
                Logger.Instance.LogError("Server could not be started...");
            }
        });

        StartClientButton.onClick.AddListener(async () =>
        {
            if (RelayManager.Instance.IsRelayEnabled && !string.IsNullOrEmpty(TxtFieldRoomCode.text))
            {
                await RelayManager.Instance.JoinRelay(TxtFieldRoomCode.text);
            }
            if (NetworkManager.Singleton.StartClient())
            {
                Logger.Instance.LogInfo("Client started...");
                EnableButtons(false);
            }
            else
            {
                Logger.Instance.LogError("Client could not be started...");
            }
        });

        NetworkManager.Singleton.OnServerStarted += () => _hasServerStarted = true;
        EnablePhysicsButton.onClick.AddListener(() =>
        {
            if (!_hasServerStarted)
            {
                Logger.Instance.LogInfo("Server has not started, physics objects spawning is impossible...");
                return;
            }

            SpawnerController.Instance.SpawnObjects();
        });
    }

    private void EnableButtons(bool isEnabled)
    {
        PanelButtons.gameObject.SetActive(isEnabled);
    }
}