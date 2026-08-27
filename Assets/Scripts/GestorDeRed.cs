using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class GestorDeRed : MonoBehaviour
{
    [Header("Interfaz Gráfica")]
    public MenuUI menuUI; // <-- Enchufe a la pantalla

    private async void Start()
    {
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
            Debug.LogError("Error Unity Services: " + e.Message);
        }

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += AlDesconectarse;
        }
    }

    private void Update()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening && NetworkManager.Singleton.IsServer)
        {
            ActualizarListaJugadores();
        }
    }

    private void ActualizarListaJugadores()
    {
        int cantidad = NetworkManager.Singleton.ConnectedClientsIds.Count;
        string lista = "Jugadores conectados (" + cantidad + "/4):\n";

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            lista += "- Jugador ID: " + clientId + "\n";
        }
        
        // Le pasamos los datos procesados a la UI
        menuUI.ActualizarLobby(lista, true);
    }

    public async void CrearSalaRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData
            );

            // Le decimos a la UI que muestre el código
            menuUI.MostrarCodigoGenerado(joinCode);
            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError("Error Relay: " + e.Message);
            menuUI.MostrarCodigoGenerado("Error al crear sala");
        }
    }

    public async void ConectarseASala(string codigo)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(codigo);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAllocation.RelayServer.IpV4, (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes, joinAllocation.Key,
                joinAllocation.ConnectionData, joinAllocation.HostConnectionData
            );

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError("Error al unirse: " + e.Message);
        }
    }

    public void IniciarPartidaHost()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
    }

    private void AlDesconectarse(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.Shutdown();
                Destroy(NetworkManager.Singleton.gameObject);
            }
            SceneManager.LoadScene("MainMenu");
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