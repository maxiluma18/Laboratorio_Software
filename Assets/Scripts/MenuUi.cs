using UnityEngine;
using TMPro;
using Unity.Netcode;               
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [Header("Paneles UI")]
    public GameObject panelMenuPrincipal;
    public GameObject panelMultijugador;
    public GameObject panelCrearSala;
    public GameObject panelUnirse;
    public GameObject panelDificultad;

    [Header("Componentes de Sala y Lobby")]
    public TMP_InputField inputCodigo;
    public TMP_Text txtCodigoGenerado;
    public TMP_Text txtListaJugadores;
    public GameObject btnIniciarPartida;

    [Header("Avisos y Errores")]
    public GameObject txtMensajeError;

    [Header("Lógica de Red")]
    public GestorDeRed gestorDeRed;

    [Header("Configuración de Audio")]
    public TMP_Text txtBotonMute;
    
    private void Start()
    {
        Debug.Log("Cerrando el juego...");
        MostrarMenuPrincipal();
    }
    
    public void CerrarJuego()
    {
        Application.Quit(); // Se cierra el juego por completo (antes era alt+f4)
    }

    // --- Navegación de Paneles ---
    public void MostrarMenuPrincipal()
    { 
        // Si hay una conexión fantasma o a medias, reiniciamos la escena de raíz
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject);
            SceneManager.LoadScene("MainMenu");
            return;
        }

        OcultarTodos(); 
        panelMenuPrincipal.SetActive(true);
    }
    public void MostrarMultijugador()
    { 
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject);
            SceneManager.LoadScene("MainMenu");
            return;
        }

        OcultarTodos(); 
        panelMultijugador.SetActive(true);
    }

    public void MostrarCrearSala()
    {
        OcultarTodos();
        panelCrearSala.SetActive(true);
        txtCodigoGenerado.text = "CÓDIGO:\nGenerando...";
        btnIniciarPartida.SetActive(false);
        gestorDeRed.CrearSalaRelay();
    }

    public void MostrarUnirse()
    {
        OcultarTodos();
        panelUnirse.SetActive(true);
        if (txtMensajeError != null) txtMensajeError.SetActive(false); // Oculta al entrar
    }

    public void BotonConectarse()
    {
        string codigo = inputCodigo.text.Trim().ToUpper();

        if (string.IsNullOrEmpty(codigo))
        {
            CambiarTextoEstado("¡El código no puede estar vacío!");
        }
        else
        {
            CambiarTextoEstado("Conectando...");
            gestorDeRed.ConectarseASala(codigo);
        }
    }

    private void OcultarTodos()
    {
        panelMenuPrincipal.SetActive(false);
        panelMultijugador.SetActive(false);
        panelCrearSala.SetActive(false);
        panelUnirse.SetActive(false);
        panelDificultad.SetActive(false);
    }

    // --- Funciones para que la Red actualice la UI ---
    public void MostrarCodigoGenerado(string codigo)
    {
        txtCodigoGenerado.text = "CÓDIGO:\n" + codigo;
    }

    public void ActualizarLobby(string lista, bool mostrarBotonPlay)
    {
        txtListaJugadores.text = lista;
        if (mostrarBotonPlay) btnIniciarPartida.SetActive(true);
    }

    public void CambiarTextoEstado(string mensaje)
    {
        if (txtMensajeError != null)
        {
            TMP_Text textoComp = txtMensajeError.GetComponent<TMP_Text>();
            if(textoComp!=null){
                textoComp.text=mensaje;
            }
            txtMensajeError.SetActive(true);
        }
    }

    public void BotonJugarDos()
    { 
        gestorDeRed.CargarJuegoLocal(); 
    }
    public void BotonJugarSolo() 
    { 
        gestorDeRed.CargarJuegoSolo(); 
    }
    // Se ejecuta al tocar "Un jugador" en el menú principal
    public void MostrarPanelDificultad()
    {
        OcultarTodos();
        panelDificultad.SetActive(true);
    }

    // Método para guardar la dificultad elegida (1=Facil, 2=Medio, 3=Dificil) e iniciar el juego
    public void SeleccionarDificultadYJugar(int nivelDificultad)
    {
        PlayerPrefs.SetInt("DificultadSeleccionada", nivelDificultad);
        PlayerPrefs.Save();

        BotonJugarSolo();
    }

    public void BotonIniciarPartidaRed() 
    { 
        gestorDeRed.IniciarPartidaHost(); 
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