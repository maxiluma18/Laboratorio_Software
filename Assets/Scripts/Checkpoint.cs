using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool yaGuardado = false; 

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos que sea el jugador y que no hayamos guardado ya en este mismo plano
        if (other.CompareTag("Player") && !yaGuardado)
        {
            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
            
            if (respawn != null)
            {
                // Le pasamos la posición de este plano azul al script del jugador
                respawn.currentCheckpoint = this.transform;
                yaGuardado = true;
                Debug.Log("¡Progreso guardado en el plano azul!");
            }
        }
    }
}