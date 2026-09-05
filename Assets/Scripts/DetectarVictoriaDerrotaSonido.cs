using UnityEngine;

public class DetectarVictoriaDerrotaSonido : MonoBehaviour
{

    public AudioSource audioSourceVictoria;
    public AudioSource audioSourceDerrota;

    private bool sonidoReproducidoVictoria = false;
    private bool sonidoReproducidoDerrota = false;
    private GameObject ganador;

    private void OnTriggerEnter(Collider other)
    {
        // PRIMERA CONDICIÓN: Detectar al primer jugador que llega (el ganador)
        if (!sonidoReproducidoVictoria && (other.CompareTag("Player Local") || other.CompareTag("Player Local2")))
        {
            ganador = other.gameObject; // Guardar quién fue el ganador
            Debug.Log("Sonido de victoria reproducido " + Time.time);
            audioSourceVictoria.Play();
            sonidoReproducidoVictoria = true;
        }

        // SEGUNDA CONDICIÓN: Detectar al segundo jugador que llega (la derrota)
        if (sonidoReproducidoVictoria && !sonidoReproducidoDerrota && other.gameObject != ganador && (other.CompareTag("Player Local") || other.CompareTag("Player Local2")))
        {
            Debug.Log("Sonido de derrota reproducido " + Time.time);
            audioSourceDerrota.Play();
            sonidoReproducidoDerrota = true;
            // Se puede markar el flag si se quiere, aunque ya es "inmutable"
        }
    }
}
