using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using UnityEngine.Windows;
public class JoinGameUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField ipAddressInput; // optional
    [SerializeField] private Button joinButton;
    [SerializeField] private Button hostButton;

    private void Start()
    {
        hostButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartHost);
        joinButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartClient);
    }


    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    private void StartClient()
    {
        string ip = ipAddressInput.text;
        string password = passwordInput.text;

        // Set IP address
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = ip;

        // Set password as payload
        byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = passwordBytes;

        NetworkManager.Singleton.StartClient();
    }
}