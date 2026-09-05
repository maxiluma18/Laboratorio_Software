using UnityEngine;

public class GameManagerUI : MonoBehaviour
{
    // Patrón Singleton para acceder desde cualquier otro script
    public static GameManagerUI Instance { get; private set; }

    [Header("Paneles de UI")]
    public GameObject panelVictoria;
    public GameObject panelDerrota; // Lo que antes llamabas panelGameOver

    [Header("Audios")]
    public AudioSource musicaAmbiente;
    public AudioSource sfxVictoria;
    public AudioSource sfxDerrota;

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

    // Funciones que llamará el jugador cuando termine la carrera
    public void MostrarVictoria()
    {
        if (musicaAmbiente != null) musicaAmbiente.Stop();
        if (sfxVictoria != null) sfxVictoria.Play();
        if (panelVictoria != null) panelVictoria.SetActive(true);
    }

    public void MostrarDerrota()
    {
        if (musicaAmbiente != null) musicaAmbiente.Stop();
        if (sfxDerrota != null) sfxDerrota.Play();
        if (panelDerrota != null) panelDerrota.SetActive(true);
    }
}
