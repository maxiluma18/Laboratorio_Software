using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform currentCheckpoint; 
    public float limiteDeCaida = -10f; 
    
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Si el jugador cae por debajo de -10 en el eje Y, se activa el respawn
        if (transform.position.y < limiteDeCaida)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        if (currentCheckpoint != null)
        {
            // Apagamos, movemos y prendemos el motor del personaje con controller.enabled = false; para evitar problemas de colisión al moverlo
            controller.enabled = false;
            transform.position = currentCheckpoint.position;
            controller.enabled = true;
            Debug.Log("Caída detectada. Reapareciendo...");
        }
    }
}