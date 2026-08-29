using UnityEngine;
using TMPro;

public class MenuUI : MonoBehaviour
{
    [Header("Paneles UI")]
    public GameObject panelMenuPrincipal;
    public GameObject panelMultijugador;
    public GameObject panelCrearSala;
    public GameObject panelUnirse;

    [Header("Componentes de Sala y Lobby")]
    public TMP_InputField inputCodigo;
    public TMP_Text txtCodigoGenerado;
    public TMP_Text txtListaJugadores;
    public GameObject btnIniciarPartida;

    [Header("Avisos y Errores")]
    public GameObject txtMensajeError; // Arrastrá acá tu txt_InputVacio

    [Header("Lógica de Red")]
    public GestorDeRed gestorDeRed;

    private void Start()
    {
        MostrarMenuPrincipal();
    }

    // --- Navegación de Paneles ---
    public void MostrarMenuPrincipal() { OcultarTodos(); panelMenuPrincipal.SetActive(true); }
    public void MostrarMultijugador() { OcultarTodos(); panelMultijugador.SetActive(true); }

    public void MostrarCrearSala()
    {
        OcultarTodos();
        panelCrearSala.SetActive(true);
        txtCodigoGenerado.text = "CÓDIGO: Generando...";
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
            if (txtMensajeError != null)
            {
                // Si querés asegurar el texto exacto:
                var textoComp = txtMensajeError.GetComponent<TMP_Text>();
                if (textoComp != null) textoComp.text = "Debe ingresar un código";

                txtMensajeError.SetActive(true);
            }
        }
        else
        {
            if (txtMensajeError != null) txtMensajeError.SetActive(false);
            gestorDeRed.ConectarseASala(codigo);
        }
    }

    private void OcultarTodos()
    {
        panelMenuPrincipal.SetActive(false);
        panelMultijugador.SetActive(false);
        panelCrearSala.SetActive(false);
        panelUnirse.SetActive(false);
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

    public void BotonJugarDos() { gestorDeRed.CargarJuegoLocal(); }
    public void BotonJugarSolo() { gestorDeRed.CargarJuegoSolo(); }
    public void BotonIniciarPartidaRed() { gestorDeRed.IniciarPartidaHost(); }
}