using UnityEngine;

public class NewEmptyCSharpScript : MonoBehaviour
{
    public AudioSource sonidoCaida;
    private float lastPlayTime = -10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Time.time - lastPlayTime > 3f)
        {
            sonidoCaida.Play();
            lastPlayTime = Time.time;
        }
    }
}
