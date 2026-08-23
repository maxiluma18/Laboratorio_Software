using UnityEngine;

public class PelotaInteractiva : MonoBehaviour
{
    private Vector3 puntoDeReaparicion;
    private Rigidbody rb;

    void Start()
    {
        // Guardamos su posición X y Z originales, pero forzamos que la Y sea 0 
        puntoDeReaparicion = new Vector3(transform.position.x, 0f, transform.position.z);

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Si cae al vacío (más abajo de -50)
        if (transform.position.y < -50f)
        {
            // 1. La teletransportamos al punto con Y = 0
            transform.position = puntoDeReaparicion;

            // 2. Le matamos la velocidad para que no siga cayendo como un meteorito
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
