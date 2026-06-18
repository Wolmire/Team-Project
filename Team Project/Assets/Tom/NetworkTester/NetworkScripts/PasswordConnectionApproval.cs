using UnityEngine;
using Unity.Netcode;

public class PasswordConnectionApproval : MonoBehaviour
{
    [SerializeField] private string requiredPassword = "letmein";

    private void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback = ApproveConnection;
        }
        else
        {
            Debug.LogError("❌ NetworkManager.Singleton is NULL! Make sure NetworkManager exists in the scene.");
        }
    }
    private void ApproveConnection(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (request.ClientNetworkId == NetworkManager.Singleton.LocalClientId)
        {
            response.Approved = true;
            response.CreatePlayerObject = true;
            response.Position = Vector3.zero;
            response.Rotation = Quaternion.identity;
            Debug.Log("✅ Host auto-approved itself.");
            return;
        }
        // Decode password from client
        string clientPassword = System.Text.Encoding.UTF8.GetString(request.Payload);

        // Check if it matches the expected one
        bool isApproved = clientPassword == requiredPassword;

        // Approve or reject connection
        response.Approved = isApproved;
        response.CreatePlayerObject = isApproved; // Don't spawn if not approved
        response.Position = Vector3.zero;         // Or your spawn point
        response.Rotation = Quaternion.identity;

        if (isApproved)
            Debug.Log($"✅ Client {request.ClientNetworkId} joined with correct password.");
        else
            Debug.LogWarning($"❌ Client {request.ClientNetworkId} tried to join with WRONG password.");
    }
}
