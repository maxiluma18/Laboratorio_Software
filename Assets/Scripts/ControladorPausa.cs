using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine.InputSystem;
using TMPro;
public class ControladorPausa : MonoBehaviour
{
    [Header("UI de Pausa")]
    public GameObject panelPausa;
    public TMP_Text txtBotonMute;

    private bool panelActivo = false;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            AlternarPanel();
        }
    }

    public void AlternarPanel()
    {
        panelActivo = !panelActivo;
        panelPausa.SetActive(panelActivo);

        if (panelActivo)
        {
            // Muestra y libera el cursor para poder clickear el botón "Salir"
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (txtBotonMute != null)
            {
                if (AudioListener.volume > 0f)
                {
                    txtBotonMute.text = "Silenciar Música";
                }
                else
                {
                    txtBotonMute.text = "Activar Música";
                }
            }
        }
        else
        {
            // Oculta y bloquea el cursor al volver al juego
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void BotonSalirAlMenu()
    {
        
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject); 
        }

        SceneManager.LoadScene("MainMenu"); 
    }

    public void ModificarVolumenMusica()
    {
        if (AudioListener.volume > 0f)
        {
            AudioListener.volume = 0f;
            if (txtBotonMute != null) txtBotonMute.text = "Activar Música";
        }
        else
        {
            AudioListener.volume = 1f;
            if (txtBotonMute != null) txtBotonMute.text = "Silenciar Música";
        }
    }
}