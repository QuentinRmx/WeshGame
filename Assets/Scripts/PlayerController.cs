using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = System.Numerics.Vector2;

public class PlayerController : NetworkBehaviour
{
    public float MoveSpeed = 3.5f;

    public float RotationSpeed = 1.5f;

    public Vector2 DefaultInitialPlanePosition = new(-4, 4);

    public NetworkVariable<Vector3> NetworkPositionDirection = new();

    public NetworkVariable<Vector3> NetworkRotationDirection = new();

    public NetworkVariable<PlayerState> NetworkPlayerState = new();

    [SerializeField] private Vector3 OldInputPosition;

    [SerializeField] private Vector3 OldInputRotation;

    // Animation Management.

    private CharacterController _characterController;

    private Animator _animator;
    private static readonly int Walk = Animator.StringToHash("Walk");

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (IsClient && IsOwner)
        {
            transform.position = new Vector3(
                Random.Range(DefaultInitialPlanePosition.X, DefaultInitialPlanePosition.Y),
                0,
                Random.Range(DefaultInitialPlanePosition.X, DefaultInitialPlanePosition.Y));
        }
    }

    private void Update()
    {
        if (IsClient && IsOwner)
        {
            ClientInput();
        }

        ClientMoveAndRotate();
        ClientVisuals();
    }

    private void ClientInput()
    {
        // Player position and rotation update.
        var inputRotation = new Vector3(0, Input.GetAxis("Horizontal"), 0);
        Vector3 direction = transform.TransformDirection(Vector3.forward);
        float forwardInput = Input.GetAxis("Vertical");
        Vector3 inputPosition = direction * forwardInput;

        if (OldInputPosition != inputPosition || OldInputRotation != inputRotation)
        {
            OldInputPosition = inputPosition;
            OldInputRotation = inputRotation;
            UpdateClientPositionAndRotationServerRpc(inputPosition * MoveSpeed, inputRotation * RotationSpeed);
        }

        // Player state update.
        if (forwardInput > 0)
        {
            UpdatePlayerStateServerRpc(PlayerState.Walk);
        }
        else if (forwardInput < 0)
        {
            UpdatePlayerStateServerRpc(PlayerState.ReverseWalk);
        }
        else
        {
            UpdatePlayerStateServerRpc(PlayerState.Idle);
        }
    }

    private void ClientMoveAndRotate()
    {
        if (NetworkPositionDirection.Value != Vector3.zero)
        {
            _characterController.SimpleMove(NetworkPositionDirection.Value);
        }

        if (NetworkRotationDirection.Value != Vector3.zero)
        {
            transform.Rotate(NetworkRotationDirection.Value);
        }
    }

    private void ClientVisuals()
    {
        switch (NetworkPlayerState.Value)
        {
            case PlayerState.Walk:
                _animator.SetFloat(Walk, 1);
                break;
            case PlayerState.ReverseWalk:
                _animator.SetFloat(Walk, -1);
                break;
            case PlayerState.Idle:
            default:
                _animator.SetFloat(Walk, 0);
                break;
        }
    }

    private void UpdateClient()
    {
    }

    [ServerRpc]
    public void UpdateClientPositionAndRotationServerRpc(Vector3 newPositionDirection, Vector3 newRotationDirection)
    {
        NetworkPositionDirection.Value = newPositionDirection;
        NetworkRotationDirection.Value = newRotationDirection;
    }

    [ServerRpc]
    public void UpdatePlayerStateServerRpc(PlayerState newState)
    {
        NetworkPlayerState.Value = newState;
    }
}