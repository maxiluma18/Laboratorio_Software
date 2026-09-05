using UnityEngine;
using UnityEngine.Audio;

public class NewEmptyCSharpScript : MonoBehaviour
{
    private float lastPlayTime = -10f;
    public AudioMixer mixer;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player Local"))
        {
            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();

            // Si le script existe, incrémenter le compteur et jouer le son de derrota
            if (respawn != null)
            {

                if (Time.time - lastPlayTime > 1.5f)
                {
                    respawn.caidas++; // Incrémenter le compteur (variable publique)

                    // Si le joueur est tombé 4 fois, jouer le son de derrota
                    if (respawn.caidas >= 4)
                    {
                        mixer.SetFloat("VolumenAmbiente", -80f);
                        respawn.sonidoDerrota.Play();
                    }
                    else
                    {
                        respawn.sonidoCaida.Play();
                    }
                    lastPlayTime = Time.time;
                }
            }
        }
        if(other.CompareTag("Player Local2"))
        {
            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();

            // Si le script existe, incrémenter le compteur et jouer le son de derrota
            if (respawn != null)
            {

                if (Time.time - lastPlayTime > 1.5f)
                {
                    respawn.caidas++; // Incrémenter le compteur (variable publique)

                    // Si le joueur est tombé 4 fois, jouer le son de derrota
                    if (respawn.caidas >= 4)
                    {
                        mixer.SetFloat("VolumenAmbiente", -80f);
                        respawn.sonidoDerrota.Play();
                    }
                    else
                    {
                        respawn.sonidoCaida.Play();
                    }
                    lastPlayTime = Time.time;
                }
            }
        }

    }
}
