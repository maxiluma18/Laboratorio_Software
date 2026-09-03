using UnityEngine;

public class DetectarVictoriaDerrotaSonido : MonoBehaviour
{

    public AudioSource audioSource;
    private bool sonidoReproducido = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!sonidoReproducido && other.CompareTag("Player"))
        {
            Debug.Log("Sonido de victoria reproducido " + Time.time);
            audioSource.Play();
            sonidoReproducido = true;
        }
    }
}
