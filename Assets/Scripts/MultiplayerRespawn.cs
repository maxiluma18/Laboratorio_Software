using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class multiplayerRespawn : NetworkBehaviour
{
    public Transform currentCheckpoint;
    public float limiteDeCaida = -10f;

    private CharacterController controller;

    void Awake()
    {
        // Acá solo guardamos el controller. Ya no buscamos el mapa porque estamos en el menú.
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Si este clon no es mi personaje, cortamos acá.
        if (!IsOwner) return;

        // 1. EL TRUCO: Si no tenemos Checkpoint, lo buscamos.
        // Esto va a fallar mientras estemos en el Menú, pero apenas cargue 
        // la escena Multiplayer, lo va a encontrar y guardar para siempre.
        if (currentCheckpoint == null)
        {
            GameObject objCheckpoint = GameObject.FindWithTag("Checkpoint");
            if (objCheckpoint != null)
            {
                currentCheckpoint = objCheckpoint.transform;
            }
            return; // Cortamos el Update acá hasta que tengamos un Checkpoint
        }

        // 2. Lógica de caída original (Solo funciona si ya encontró el Checkpoint)
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
            transform.position = currentCheckpoint.position;

            var networkTransform = GetComponent<Unity.Netcode.Components.NetworkTransform>();
            if (networkTransform != null)
            {
                networkTransform.Teleport(transform.position, transform.rotation, transform.localScale);
            }

            controller.enabled = true;
            Debug.Log("Caída detectada. Reapareciendo...");
        }
    }
}