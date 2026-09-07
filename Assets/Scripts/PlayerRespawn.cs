using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerRespawn : MonoBehaviour
{
    public Transform currentCheckpoint; 
    public float limiteDeCaida = -10f;

    [Header("Sistema de Vidas")]
    public int vidasActuales;
    public TMP_Text textoVidas;

    [Header("UI de Game Over")]
    public GameObject panelGameOver;
    public GameObject panelVictoria;

    public AudioSource sfxMuerte;
    public AudioSource sfxVictoria;
    public AudioSource sfxDerrota;
    public AudioSource musicaAmbiente;

    private CharacterController controller;
    private bool juegoTerminado = false;


    // Variable para saber en qué modo estamos
    private bool esMultijugador = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Si no existe la clave, por defecto asume 0 (Un solo jugador)
        esMultijugador = PlayerPrefs.GetInt("ModoMultijugador", 0) == 1;

        ConfigurarVidasIniciales();
        controller.enabled = true;

        // Nos aseguramos de que el panel de Game Over y victoria empiece apagado
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(false);
        }
        if (panelVictoria != null) panelVictoria.SetActive(false);
    }

    private void ConfigurarVidasIniciales()
    {

        if (esMultijugador)
        {
            // En multijugador, las vidas son infinitas
            if (textoVidas != null) textoVidas.text = "Vidas: ∞";
            return; // Salimos para no aplicar la lógica de dificultad
        }

        // Leemos la dificultad que guardó tu otro script. 
        // Si no encuentra nada, pone "2" (Medio) por defecto.
        int dificultad = PlayerPrefs.GetInt("DificultadSeleccionada", 2);

        switch (dificultad)
        {
            case 1: 
                vidasActuales = 10;
                break;
            case 2: 
                vidasActuales = 5;
                break;
            case 3: 
                vidasActuales = 3;
                break;
        }

        ActualizarTextoVidas();
    }

    void Update()
    {
        if (transform.position.y < limiteDeCaida && !juegoTerminado)
        {
            PerderVida();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (juegoTerminado) return;
        if (other.CompareTag("trampa"))
        {
            PerderVida();
        }

        else if (other.CompareTag("meta"))
        {
            Victoria();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (juegoTerminado) return;
        if (hit.gameObject.CompareTag("trampa"))
        {
            PerderVida();
        }

        else if (hit.gameObject.CompareTag("meta"))
        {
            Victoria();
        }
    }

    
    private void PerderVida()
    {
        // Si es multijugador, solo reaparece sin restar vidas
        if (esMultijugador)
        {
            
            if (sfxMuerte != null) sfxMuerte.Play();
            Respawn();
            return;
        }

        vidasActuales--;
        ActualizarTextoVidas();

        if (vidasActuales > 0)
        {
            if (sfxMuerte != null) sfxMuerte.Play();
            Respawn();
        }
        else
        {
            if (sfxMuerte != null) sfxMuerte.Stop();
            MuerteDefinitiva();
        }
    }

    private void ActualizarTextoVidas()
    {
        if (textoVidas != null)
        {
            textoVidas.text = "Vida/s: " + vidasActuales;
        }
    }

    public void MuerteDefinitiva()
    {
        if (juegoTerminado) return; // Evita que se ejecute dos veces

        juegoTerminado = true;
        if (musicaAmbiente != null) 
        {
            musicaAmbiente.Stop();
        }
        if (sfxDerrota != null) 
        {
            sfxDerrota.Play();
        }


        if (panelGameOver != null)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            panelGameOver.SetActive(true);
        }

        if (controller != null)
        {
            controller.enabled = false;
        }

        if (esMultijugador)
        {
            PlayerRespawn[] todosLosJugadores = FindObjectsByType<PlayerRespawn>();
            foreach (PlayerRespawn jugador in todosLosJugadores)
            {
                // Si el jugador encontrado NO es este jugador, lo hacemos perder
                if (jugador != this)
                {
                    jugador.MuerteDefinitiva();
                }
            }
        }
    }

    private void Victoria()
    {
        juegoTerminado = true;
        if (musicaAmbiente != null) 
        {
            musicaAmbiente.Stop();
        }
        if (sfxVictoria != null) 
        {
            sfxVictoria.Play();
        }

        // Mostramos el panel de victoria
        if (panelVictoria != null)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            panelVictoria.SetActive(true);
        }

        // Apagamos el movimiento y pausamos el tiempo
        if (controller != null) controller.enabled = false;
    }

    public void Respawn()
    {
        if (currentCheckpoint != null)
        {
            controller.enabled = false;

            Vector3 spawnPos = currentCheckpoint.position;

            if (gameObject.name.Contains("2") || gameObject.name.Contains("Jugador2"))
            {
                spawnPos += new Vector3(1.5f, 0f, 0f);
            }
            else
            {
                spawnPos += new Vector3(-1.5f, 0f, 0f);
            }

            transform.position = spawnPos;
            controller.enabled = true;
        }
    }
}
