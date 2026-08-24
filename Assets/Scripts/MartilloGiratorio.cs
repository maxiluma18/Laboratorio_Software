using UnityEngine;

public class MartilloGiratorio : MonoBehaviour
{
    [Header("Configuración del Martillo")]
    // Velocidad a la que gira el martillo.
    public float velocidadRotacion = 100f;

    // Elegimos el eje de rotación. Para que sea como un reloj (vertical), 
    // generalmente usamos el eje Z (0, 0, 1).
    public Vector3 ejeDeRotacion = new Vector3(0, 0, 1);

    void Update()
    {
        // transform.Rotate gira el objeto continuamente.
        // Multiplicamos por la velocidad y por Time.deltaTime para que 
        // gire suavemente sin importar a cuántos FPS vaya el juego.
        transform.Rotate(ejeDeRotacion * velocidadRotacion * Time.deltaTime);
    }
}
