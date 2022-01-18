using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerUI : NetworkBehaviour
{
    private readonly NetworkVariable<NetworkString> _playerName = new();

    private bool _isOverlaySet;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            _playerName.Value = $"Player {OwnerClientId}";
        }
    }

    public void SetOverlay()
    {
        var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        localPlayerOverlay.text = _playerName.Value;
        _isOverlaySet = true;
    }

    private void Update()
    {
        if (!_isOverlaySet && !string.IsNullOrEmpty(_playerName.Value))
        {
            SetOverlay();
        }
    }
}