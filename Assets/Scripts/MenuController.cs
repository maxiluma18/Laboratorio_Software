using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class MenuController : MonoBehaviour
{
    [Header("Paneles UI")]
    public GameObject panelMenuPrincipal;
    public GameObject panelMultijugador;
    public GameObject panelCrearSala;
    public GameObject panelUnirse;

    [Header("Componentes de Sala y Lobby")]
    public TMP_InputField inputCodigo;
    public TMP_Text txtCodigoGenerado;
    public TMP_Text txtListaJugadores;    // <--- Texto para mostrar quiénes entraron
    public GameObject btnIniciarPartida;  // <--- El botón de Play del Host

    private async void Start()
    {
        MostrarMenuPrincipal();

        try
        {
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al inicializar Unity Services: " + e.Message);
        }
    }

    private void Update()
    {
        // Si estamos en red y somos el Host, actualizamos la lista de jugadores conectados en tiempo real
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening && NetworkManager.Singleton.IsServer)
        {
            ActualizarListaJugadoresUI();
        }
    }

    private void ActualizarListaJugadoresUI()
    {
        if (txtListaJugadores != null)
        {
            int cantidadJugadores = NetworkManager.Singleton.ConnectedClientsIds.Count;
            string lista = "Jugadores conectados (" + cantidadJugadores + "/4):\n";

            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                lista += "- Jugador ID: " + clientId + "\n";
            }
            txtListaJugadores.text = lista;
        }

        // Mostrar el botón de iniciar partida solo para el Host
        if (btnIniciarPartida != null)
        {
            btnIniciarPartida.SetActive(true);
        }
    }

    // --- Navegación ---
    public void MostrarMenuPrincipal() { OcultarTodos(); panelMenuPrincipal.SetActive(true); }
    public void MostrarMultijugador() { OcultarTodos(); panelMultijugador.SetActive(true); }

    public void MostrarCrearSala()
    {
        OcultarTodos();
        panelCrearSala.SetActive(true);
        if (txtCodigoGenerado != null) txtCodigoGenerado.text = "CÓDIGO: Generando...";
        if (btnIniciarPartida != null) btnIniciarPartida.SetActive(false); // Oculto hasta ser Host activo
        CrearSalaRelay();
    }

    public void MostrarUnirse()
    {
        OcultarTodos();
        panelUnirse.SetActive(true);
    }

    private void OcultarTodos()
    {
        panelMenuPrincipal.SetActive(false);
        panelMultijugador.SetActive(false);
        panelCrearSala.SetActive(false);
        panelUnirse.SetActive(false);
    }

    // --- Crear Sala (Host) ---
    private async void CrearSalaRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
            );

            if (txtCodigoGenerado != null) txtCodigoGenerado.text = "CÓDIGO:\n" + joinCode;

            // Arrancamos el Host en Netcode para que empiece a escuchar conexiones de Relay
            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError("Error al crear Relay: " + e.Message);
            if (txtCodigoGenerado != null) txtCodigoGenerado.text = "Error al generar código";
        }
    }

    // --- Unirse a Sala (Cliente) ---
    public async void ConectarseASala()
    {
        string codigo = inputCodigo.text.Trim().ToUpper();
        if (string.IsNullOrEmpty(codigo)) return;

        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(codigo);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
            );

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError("Error al unirse: " + e.Message);
        }
    }

    // --- Iniciar Partida (Solo Host) ---
    public void IniciarPartidaHost()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            // Agregamos LoadSceneMode.Single al final para cumplir con lo que exige Netcode
            NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
    }

    public void CargarJuegoSolo()
    {
        // Si la red quedó activa por alguna prueba previa, la apagamos para modo solitario
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.Shutdown();
        }
        SceneManager.LoadScene("SampleScene");
    }
}