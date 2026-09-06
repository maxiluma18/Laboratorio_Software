using UnityEngine;

public class GameManagerUI : MonoBehaviour
{
    // Patrón Singleton para acceder desde cualquier otro script
    public static GameManagerUI Instance { get; private set; }

    [Header("Paneles de UI")]
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    private void Awake()
    {
        // Nos aseguramos de que solo exista un GameManagerUI en la escena
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Funciones que llamará el jugador (multiplayerRespawn) cuando termine la carrera
    public void MostrarVictoria()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (panelVictoria != null) panelVictoria.SetActive(true);
    }

    public void MostrarDerrota()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (panelDerrota != null) panelDerrota.SetActive(true);
    }
}