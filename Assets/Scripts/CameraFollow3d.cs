using UnityEngine;

public class CameraFollow3D : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    public Transform jugador;

    [Header("Configuración de la Cámara")]
    // Distancia respecto al jugador (0 centrado, 3 unidades arriba, -6 unidades atrás)
    public Vector3 offset = new Vector3(0f, 3f, -6f);

    // Velocidad con la que la cámara sigue al jugador (para que no sea un movimiento rígido)
    public float suavizado = 10f;

    void LateUpdate()
    {
        if (jugador == null) return;

        // 1. Calculamos la posición exacta detrás del jugador.
        // Al multiplicar la rotación del jugador por el offset, aseguramos que el offset rote junto con él.
        Vector3 posicionDeseada = jugador.position + (jugador.rotation * offset);

        // 2. Movemos la cámara fluidamente desde donde está hacia la posiciónDeseada
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);

        // 3. Forzamos a la lente de la cámara a apuntar siempre al centro del jugador
        transform.LookAt(jugador);
    }
}
