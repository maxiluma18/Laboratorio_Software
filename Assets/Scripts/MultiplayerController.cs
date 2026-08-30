using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class multiplayerController : NetworkBehaviour
{
    public float speed = 5f;
    public float gravity = -20f;
    public float jumpHeight = 2f;

    public Camera playerCamera;
    public AudioListener audioListener;

    [Header("Asignación de Teclas")]
    public InputActionReference accionMover;
    public InputActionReference accionSaltar;

    private CharacterController controller;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            if (playerCamera != null) playerCamera.enabled = true;
            if (audioListener != null) audioListener.enabled = true;

            // Habilitamos las teclas SOLO para el dueño de este personaje
            if (accionMover != null) accionMover.action.Enable();
            if (accionSaltar != null) accionSaltar.action.Enable();
        }
        else
        {
            if (playerCamera != null) playerCamera.enabled = false;
            if (audioListener != null) audioListener.enabled = false;
        }
    }

    void Update()
    {
        if (!IsOwner) return; // Si no es tuyo, no se mueve

        bool isGrounded = controller.isGrounded;
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        if (accionSaltar != null && accionSaltar.action.WasPressedThisFrame() && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector2 moveInput = Vector2.zero;
        if (accionMover != null)
        {
            moveInput = accionMover.action.ReadValue<Vector2>();
        }

        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        move.y = verticalVelocity / speed;

        controller.Move(move * speed * Time.deltaTime);
    }
}