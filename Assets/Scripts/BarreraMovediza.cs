using System.Collections;
using UnityEngine;

public class ObstaculoVertical : MonoBehaviour
{
    [Header("Tiempos de la Secuencia")]
    public float tiempoBajar = 2f;
    public float tiempoEsperaAbajo = 1f;
    public float tiempoSubir = 2f;
    public float tiempoEsperaArriba = 3f;

    [Header("Distancia")]
    // Cuántos metros hacia abajo se va a mover el cilindro
    public float distanciaCaida = 4f;

    private Vector3 posicionArriba;
    private Vector3 posicionAbajo;

    void Start()
    {
        // Guardamos la posición inicial como el punto más alto
        posicionArriba = transform.position;

        // Calculamos cuál será el punto más bajo
        posicionAbajo = posicionArriba - new Vector3(0, distanciaCaida, 0);

        StartCoroutine(CicloVertical());
    }

    IEnumerator CicloVertical()
    {
        while (true)
        {
            // 1. Bajar
            yield return StartCoroutine(MoverCilindro(posicionAbajo, tiempoBajar));

            // 2. Esperar abajo
            yield return new WaitForSeconds(tiempoEsperaAbajo);

            // 3. Subir
            yield return StartCoroutine(MoverCilindro(posicionArriba, tiempoSubir));

            // 4. Esperar arriba
            yield return new WaitForSeconds(tiempoEsperaArriba);
        }
    }

    IEnumerator MoverCilindro(Vector3 destino, float duracion)
    {
        Vector3 inicio = transform.position;
        float tiempoPasado = 0f;

        while (tiempoPasado < duracion)
        {
            tiempoPasado += Time.deltaTime;
            float porcentaje = tiempoPasado / duracion;

            // SmoothStep hace que el movimiento no sea robótico, frena suave al llegar
            float curva = Mathf.SmoothStep(0f, 1f, porcentaje);

            transform.position = Vector3.Lerp(inicio, destino, curva);

            yield return null;
        }

        transform.position = destino;
    }
}
