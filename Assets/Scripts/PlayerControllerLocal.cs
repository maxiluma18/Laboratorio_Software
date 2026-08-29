using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerLocal : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -20f;
    public float jumpHeight = 2f;

    [Header("Asignación de Teclas")]
    public InputActionReference accionMover;
    public InputActionReference accionSaltar;

    private CharacterController controller;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Habilitamos las acciones para que escuchen el teclado
        if (accionMover != null) accionMover.action.Enable();
        if (accionSaltar != null) accionSaltar.action.Enable();
    }

    void Update()
    {
        bool isGrounded = controller.isGrounded;
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        // Si la tecla de este jugador se apretó, salta
        if (accionSaltar != null && accionSaltar.action.WasPressedThisFrame() && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        // Leemos hacia dónde se quiere mover este jugador
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