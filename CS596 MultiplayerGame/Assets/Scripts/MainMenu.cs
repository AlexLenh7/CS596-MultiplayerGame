using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject menuCanvas; // Reference to the menu canvas

    private void Awake() 
    {
        hostButton.onClick.AddListener(HandleHostButtonClicked);
        clientButton.onClick.AddListener(HandleClientButtonClicked);
        quitButton.onClick.AddListener(HandleQuitButtonClicked);
    }
    
    private void HandleHostButtonClicked()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager.Singleton is null! Ensure a NetworkManager is present in the scene.");
            return;
        }
        NetworkManager.Singleton.StartHost();
        menuCanvas.SetActive(false); // Disable the menu canvas after starting the host
    }

    private void HandleClientButtonClicked()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager.Singleton is null! Ensure a NetworkManager is present in the scene.");
            return;
        }
        NetworkManager.Singleton.StartClient();
        menuCanvas.SetActive(false); // Disable the menu canvas after starting the client
    }

    private void HandleQuitButtonClicked()
    {
        Application.Quit();
    }
}
