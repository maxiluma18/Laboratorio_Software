using UnityEngine;
using UnityEngine.Audio;

public class ControlarVolumenAmbiente : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource sonidoVictoria;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Reducir el volumen del ambiente a -80dB (casi mute)
            mixer.SetFloat("VolumenAmbiente", -80f);

            
        }
    }
}
