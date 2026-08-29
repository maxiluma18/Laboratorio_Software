using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class multiplayerController : NetworkBehaviour
{
    public float speed = 5f;
    public float gravity = -20f;
    public float jumpHeight = 2f;

    // NUEVO: Variables para controlar la cámara y el audio
    public Camera playerCamera;
    public AudioListener audioListener;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private float verticalVelocity;
    private bool jumpPressed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            // Activamos cámara, audio e input para el jugador local
            if (playerInput != null) playerInput.enabled = true;
            if (playerCamera != null) playerCamera.enabled = true;
            if (audioListener != null) audioListener.enabled = true;
        }
        else
        {
            // Desactivamos todo eso para los jugadores remotos
            if (playerInput != null) playerInput.enabled = false;
            if (playerCamera != null) playerCamera.enabled = false;
            if (audioListener != null) audioListener.enabled = false;
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        bool isGrounded = controller.isGrounded;
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        if (jumpPressed && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpPressed = false;
        }
        if (!isGrounded)
        {
            jumpPressed = false;
        }
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        move.y = verticalVelocity / speed;

        controller.Move(move * speed * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        if (!IsOwner) return;
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (!IsOwner) return;
        if (value.isPressed)
        {
            jumpPressed = true;
        }
    }
}