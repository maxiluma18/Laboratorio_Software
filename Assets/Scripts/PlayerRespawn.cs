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
        if (transform.position.y < limiteDeCaida)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        if (currentCheckpoint != null)
        {
            controller.enabled = false;
            
            // Posición base del checkpoint
            Vector3 spawnPos = currentCheckpoint.position;

            // Si es el Jugador 2, lo desplazamos un poquito a la derecha (1.5 metros) para que no se choquen
            if (gameObject.name.Contains("2") || gameObject.name.Contains("Jugador2"))
            {
                spawnPos += new Vector3(1.5f, 0f, 0f);
            }
            else
            {
                // Al Jugador 1 lo corremos un poquito a la izquierda
                spawnPos += new Vector3(-1.5f, 0f, 0f);
            }

            transform.position = spawnPos;
            controller.enabled = true;
            Debug.Log("Reapareciendo separado...");
        }
    }
}