using UnityEngine;
using UnityEngine.SceneManagement; // <-- NECESARIO PARA CAMBIAR DE ESCENA

public class PlayerRespawn : MonoBehaviour
{
    public Transform currentCheckpoint; 
    public float limiteDeCaida = -10f;

    [Header("Sistema de Vidas")]
    public int vidasActuales; // Puedes ver cuántas vidas le quedan en el Inspector

    [Header("UI de Game Over")]
    public GameObject panelGameOver; // Arrastraremos el panel aquí

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        ConfigurarVidasIniciales();

        // Nos aseguramos de que el panel de Game Over empiece apagado
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(false);
        }
    }

    private void ConfigurarVidasIniciales()
    {
        // Leemos la dificultad que guardó tu otro script. 
        // Si no encuentra nada, pone "2" (Medio) por defecto.
        int dificultad = PlayerPrefs.GetInt("DificultadSeleccionada", 2);

        switch (dificultad)
        {
            case 1: // Fácil
                vidasActuales = 10;
                break;
            case 2: // Medio
                vidasActuales = 5;
                break;
            case 3: // Difícil
                vidasActuales = 3;
                break;
        }

        Debug.Log($"{gameObject.name} inicia con {vidasActuales} vidas (Dificultad: {dificultad})");
    }

    void Update()
    {
        if (transform.position.y < limiteDeCaida)
        {
            PerderVida();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("trampa"))
        {
            PerderVida();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("trampa"))
        {
            PerderVida();
        }
    }

    // --- NUEVA LÓGICA DE VIDAS ---
    private void PerderVida()
    {
        vidasActuales--; // Restamos 1 a las vidas
        Debug.Log($"¡{gameObject.name} perdió una vida! Le quedan {vidasActuales}");

        if (vidasActuales > 0)
        {
            // Si aún le quedan vidas, reaparece normal
            Respawn();
        }
        else
        {
            // Si llegó a 0, pierde
            MuerteDefinitiva();
        }
    }

    private void MuerteDefinitiva()
    {
        Debug.Log($"¡GAME OVER para {gameObject.name}!");

        // 1. Mostramos el panel de Game Over
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }

        // 2. Apagamos el CharacterController para que no se pueda mover más
        if (controller != null)
        {
            controller.enabled = false;
        }

        // 3. Opcional: Pausamos el tiempo del juego para que todo se detenga
        Time.timeScale = 0f;
    }

    // --- NUEVA FUNCIÓN PARA EL BOTÓN ---
    public void VolverAlMenu()
    {
        // Restauramos el tiempo a la normalidad antes de cambiar de escena
        Time.timeScale = 1f;

        // Cargamos la escena del menú principal (asegúrate de que se llame exactamente así)
        SceneManager.LoadScene("MainMenu");
    }
    // -----------------------------

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
