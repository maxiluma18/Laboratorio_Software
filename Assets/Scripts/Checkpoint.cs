using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool progresoGuardado = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!progresoGuardado && other.CompareTag("Player"))
        {
            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
            
            if (respawn != null)
            {
                // Guardamos el checkpoint para este jugador en específico
                respawn.currentCheckpoint = this.transform;
                Debug.Log("¡Progreso guardado para " + other.gameObject.name + "!");
                progresoGuardado = true;
            }
        }
    }
}