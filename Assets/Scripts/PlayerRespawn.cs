using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    public Transform currentCheckpoint; 
    public float limiteDeCaida = -10f;

    [Header("Sistema de Vidas")]
    public int vidasActuales; 

    [Header("UI de Game Over")]
    public GameObject panelGameOver;
    public GameObject panelVictoria;

    public AudioSource sfxMuerte;
    public AudioSource sfxVictoria;
    public AudioSource sfxDerrota;
    public AudioSource musicaAmbiente;

    private CharacterController controller;
    private bool juegoTerminado = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
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

        Debug.Log($"{gameObject.name} inicia con {vidasActuales} vidas (Dificultad: {dificultad})");
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
        vidasActuales--;
        Debug.Log($"¡{gameObject.name} perdió una vida! Le quedan {vidasActuales}");

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

    private void MuerteDefinitiva()
    {
        juegoTerminado = true;
        Debug.Log($"¡GAME OVER para {gameObject.name}!");
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
            panelGameOver.SetActive(true);
        }

        if (controller != null)
        {
            controller.enabled = false;
        }
    }

    private void Victoria()
    {
        juegoTerminado = true;
        Debug.Log("¡Llegaste a la meta!");
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
            Debug.Log("Reapareciendo separado...");
        }
    }
}
