using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class multiplayerRespawn : NetworkBehaviour
{
    public float limiteDeCaida = -10f;

    private CharacterController controller;

    // Aquí guardaremos la posición exacta donde debe reaparecer el jugador
    private Vector3 currentRespawnPos;

    // Usamos OnNetworkSpawn en lugar de Start cuando trabajamos con Netcode
    public override void OnNetworkSpawn()
    {
        controller = GetComponent<CharacterController>();

        // Solo el dueño calcula su posición inicial
        if (IsOwner)
        {
            int playerId = (int)OwnerClientId % 4;
            float posX = 0f;

            switch (playerId)
            {
                case 0: posX = -1f; break;
                case 1: posX = 0f; break;
                case 2: posX = 1.5f; break;
                case 3: posX = 3f; break;
            }

            // Configuramos la posición inicial como el primer "checkpoint"
            currentRespawnPos = new Vector3(posX, 2f, -15f);
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        if (transform.position.y < limiteDeCaida)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        controller.enabled = false;

        // Lo movemos a la posición guardada (la inicial o la del último checkpoint)
        transform.position = currentRespawnPos;

        controller.enabled = true;

        Debug.Log($"Reapareciendo en la posición {currentRespawnPos}");
    }

    // Detectamos cuando el jugador pisa un nuevo checkpoint o una trampa (si es Trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner) return;

        // Verifica si el objeto contra el que chocamos tiene la etiqueta "Checkpoint"
        if (other.CompareTag("Checkpoint"))
        {
            // Actualizamos la posición de reaparición. 
            // Le sumamos 2 en Y para asegurarnos de que no aparezca atascado en el piso
            currentRespawnPos = other.transform.position + new Vector3(0f, 2f, 0f);

            Debug.Log($"¡Checkpoint alcanzado! Nueva posición guardada: {currentRespawnPos}");
        }
        // Verifica si tocamos una trampa
        else if (other.CompareTag("trampa"))
        {
            Respawn();
        }
    }

    // Detectamos si el jugador choca contra una trampa (si es un objeto sólido normal)
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!IsOwner) return;

        if (hit.gameObject.CompareTag("trampa"))
        {
            Respawn();
        }
    }
}