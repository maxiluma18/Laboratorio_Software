using UnityEngine;

public class MultiplayerDetectarVictoriaDerrota : MonoBehaviour
{
    public AudioSource audioSourceVictoria;

    private bool sonidoReproducidoVictoria = false;

    private void OnTriggerEnter(Collider other)
    {
        // DETECTAR AL PRIMER JUGADOR (EL GANADOR) - Tiene el tag "Player"
        if (other.CompareTag("Player"))
        {
            // Si el sonido de victoria NO ha sido reproducido, se reproduce
            if (!sonidoReproducidoVictoria)
            {
                audioSourceVictoria.Play();
                sonidoReproducidoVictoria = true;
                Debug.Log("Sonido de victoria reproducido " + Time.time);
            }
        }
    }
}
