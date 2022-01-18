using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class PlayerController : NetworkBehaviour
{
    public float WalkSpeed = 3.5f;

    public Vector2 DefaultPositionRange = new(-4, 4);

    public NetworkVariable<float> ForwardBackPosition = new();

    public NetworkVariable<float> LeftRightPosition = new();

    [SerializeField]
    private float OldForwardBackPosition;

    [SerializeField]
    private float OldLeftRightPosition;

    private void Start()
    {
        transform.position = new Vector3(
            Random.Range(DefaultPositionRange.X, DefaultPositionRange.Y),
            0,
            Random.Range(DefaultPositionRange.X, DefaultPositionRange.Y));
    }

    private void Update()
    {
        if (IsServer)
        {
            UpdateServer();
        }

        if (IsClient && IsOwner)
        {
            UpdateClient();
        }
    }

    private void UpdateServer()
    {
        transform.position = new Vector3(
            transform.position.x + LeftRightPosition.Value,
            transform.position.y,
            transform.position.z + ForwardBackPosition.Value);
    }

    private void UpdateClient()
    {
        float forwardBack = 0;
        float leftRight = 0;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            forwardBack += WalkSpeed;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            forwardBack -= WalkSpeed;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            leftRight -= WalkSpeed;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            leftRight += WalkSpeed;
        }

        if (OldForwardBackPosition != forwardBack ||
            OldLeftRightPosition != leftRight)
        {
            OldForwardBackPosition = forwardBack;
            OldLeftRightPosition = leftRight;
        }

        // Update the server.
        UpdateClientPositionServerRpc(forwardBack, leftRight);
    }

    [ServerRpc]
    public void UpdateClientPositionServerRpc(float forwardBackward, float leftRight)
    {
        ForwardBackPosition.Value = forwardBackward;
        LeftRightPosition.Value = leftRight;
    }
}