using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class multiplayerRespawn : NetworkBehaviour
{
    public float limiteDeCaida = -10f;

    private CharacterController controller;


    [Header("UI de Game Over")]
    public GameObject panelGameOver;
    public GameObject panelVictoria;

    public AudioSource sfxMuerte;
    public AudioSource sfxVictoria;
    public AudioSource sfxDerrota;

    // Guardamos la posición exacta donde debe reaparecer el jugador
    private Vector3 currentRespawnPos;
    private float miOffsetEnX = 0f;


    // Bandera local para evitar que el jugador siga interactuando tras terminar
    private bool carreraTerminada = false;

    // Bandera estática en el servidor para garantizar que solo haya un ganador
    private static bool metaAlcanzadaServidor = false;

    // Usamos OnNetworkSpawn en lugar de Start
    public override void OnNetworkSpawn()
    {
        controller = GetComponent<CharacterController>();

        if (panelVictoria != null) panelVictoria.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(false);

        // Reiniciamos la variable estática del servidor al spawnear en una nueva partida
        if (IsServer) metaAlcanzadaServidor = false;

        // Solo el dueño calcula su posición inicial
        if (IsOwner)
        {
            int playerId = (int)OwnerClientId % 4;
            float posX = 0f;

            switch (playerId)
            {
                case 0: posX = -1.5f;
                miOffsetEnX = posX;
                break;
                case 1: posX = 0f;
                miOffsetEnX = posX; 
                break;
                case 2: posX = 1.5f; 
                miOffsetEnX = posX;
                break;
                case 3: posX = 3f; 
                miOffsetEnX = posX;
                break;
            }

            // Configuramos la posición inicial como el primer "checkpoint"
            currentRespawnPos = new Vector3(miOffsetEnX, 2f, -15f);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Multiplayer" && IsOwner)
        {
            controller.enabled = false;
            transform.position = currentRespawnPos; // Lo ubicamos en su checkpoint
            controller.enabled = true; // Reactivamos físicas
        }
    }
    public override void OnNetworkDespawn()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (!IsOwner || carreraTerminada) return;

        if (SceneManager.GetActiveScene().name != "Multiplayer") return;
        
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
        if (sfxMuerte != null) sfxMuerte.Play();

    }

    // Detectamos cuando el jugador pisa un nuevo checkpoint o una trampa (si es Trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner || carreraTerminada) return;

        if (other.CompareTag("Checkpoint"))
        {
            currentRespawnPos = other.transform.position + new Vector3(miOffsetEnX, 2f, 0f);

        }

        else if (other.CompareTag("trampa"))
        {
            Respawn();
        }
        else if (other.CompareTag("meta")) 
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

        // Frenamos al jugador
        if (controller != null)
        {
            controller.enabled = false;
        }

        // SOLO afectamos la pantalla y el sonido si esta es mi propia pantalla
        if (IsOwner)
        {
            GameObject ambienteObj = GameObject.Find("AudioAmbiente");
            if (ambienteObj != null)
            {
                AudioSource fuenteAudio = ambienteObj.GetComponent<AudioSource>();
                if (fuenteAudio != null) fuenteAudio.Stop();
            }
            // Verificamos si existe nuestro Manager en la escena
            if (GameManagerUI.Instance != null)
            {
                if (NetworkManager.Singleton.LocalClientId == ganadorId)
                {
                    GameManagerUI.Instance.MostrarVictoria(); // Llamamos a la UI de la escena
                    if (sfxVictoria != null) sfxVictoria.Play();
                }
                else
                {
                    GameManagerUI.Instance.MostrarDerrota(); // Llamamos a la UI de la escena
                    if (sfxDerrota != null) sfxDerrota.Play();
                }
            }
            else
            {
                Debug.LogWarning("Falta el GameManagerUI en la escena.");
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