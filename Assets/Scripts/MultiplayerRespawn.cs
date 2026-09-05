using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class multiplayerRespawn : NetworkBehaviour
{
    public float limiteDeCaida = -10f;

    private CharacterController controller;

    [Header("UI de Fin de Carrera")]
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    // Guardamos la posición exacta donde debe reaparecer el jugador
    private Vector3 currentRespawnPos;

    // Bandera local para evitar que el jugador siga interactuando tras terminar
    private bool carreraTerminada = false;

    // Bandera estática en el servidor para garantizar que solo haya un ganador
    private static bool metaAlcanzadaServidor = false;

    // Usamos OnNetworkSpawn en lugar de Start cuando trabajamos con Netcode
    public override void OnNetworkSpawn()
    {
        controller = GetComponent<CharacterController>();

        if (panelVictoria != null) panelVictoria.SetActive(false);
        if (panelDerrota != null) panelDerrota.SetActive(false);

        // Reiniciamos la variable estática del servidor al spawnear en una nueva partida
        if (IsServer) metaAlcanzadaServidor = false;

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
        if (!IsOwner || carreraTerminada) return;

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
        if (!IsOwner || carreraTerminada) return;

        
        if (other.CompareTag("Checkpoint"))
        {
            currentRespawnPos = other.transform.position + new Vector3(0f, 2f, 0f);

            Debug.Log($"¡Checkpoint alcanzado! Nueva posición guardada: {currentRespawnPos}");
        }

        else if (other.CompareTag("trampa"))
        {
            Respawn();
        }
        else if (other.CompareTag("meta")) // CA 1: Detecta el trigger
        {
            NotificarMetaServerRpc();
        }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!IsOwner) return;

        if (hit.gameObject.CompareTag("trampa"))
        {
            Respawn();
        }
    }

    [ServerRpc]
    private void NotificarMetaServerRpc(ServerRpcParams rpcParams = default)
    {
        if (metaAlcanzadaServidor) return;

        metaAlcanzadaServidor = true;
        ulong ganadorId = rpcParams.Receive.SenderClientId;

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.PlayerObject != null)
            {
                client.PlayerObject.GetComponent<multiplayerRespawn>().FinalizarCarreraClientRpc(ganadorId);
            }
        }
    }


    [ClientRpc]
    private void FinalizarCarreraClientRpc(ulong ganadorId)
    {
        carreraTerminada = true;

        
        if (controller != null) controller.enabled = false;


        if (IsOwner)
        {
            if (this.NetworkManager.LocalClientId == ganadorId)
            {
                if (panelVictoria != null) panelVictoria.SetActive(true);
            }
            else
            {
                if (panelDerrota != null) panelDerrota.SetActive(true);
            }
        }
    }

    public void VolverAlMenu()
    {
        // En multijugador es crucial apagar la conexión antes de cambiar de escena
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject);
        }
        SceneManager.LoadScene("MainMenu");
    }
}