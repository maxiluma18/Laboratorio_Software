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
        
        // Le mandamos la orden al backend de red
        gestorDeRed.CrearSalaRelay(); 
    }

    public void MostrarUnirse()
    {
        OcultarTodos();
        panelUnirse.SetActive(true);
    }

    public void BotonConectarse()
    {
        string codigo = inputCodigo.text.Trim().ToUpper();
        if (!string.IsNullOrEmpty(codigo))
        {
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
}