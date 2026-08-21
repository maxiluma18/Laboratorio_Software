using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    public float speed = 5f;
    private CharacterController controller;
    private Vector2 moveInput;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        controller.Move(move * speed * Time.deltaTime);
    }
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
