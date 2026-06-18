using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.Netcode.Transports.UTP;
using System.Net;
using TMPro;

public class ConnectUIScript : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private string ipAddress = "192.168.0.5";
    [SerializeField] private Button clientButton;
    [SerializeField] private TMP_InputField passwordInput;
    private string joinPassword;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        hostButton.onClick.AddListener(HostButtonOnClick);
        clientButton.onClick.AddListener(ClientButtonOnClick);
    }

    private void HostButtonOnClick()
    {
        if (NetworkManager.Singleton.IsListening) return; 
        NetworkManager.Singleton.StartHost();
    }

    private void ClientButtonOnClick()
    {
        if (NetworkManager.Singleton.IsListening) return;

        // Set the IP address for LAN
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = ipAddress;

        // Encode the password to send
        joinPassword = passwordInput.text;  // get password from UI input
        byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(joinPassword);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = passwordBytes;
        NetworkManager.Singleton.StartClient();
    }
}
