using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button menuButtonWinner;
    [SerializeField] private Button menuButtonLoser;
    [SerializeField] private GameObject menuCanvas; // Reference to the menu canvas
    [SerializeField] private AudioClip buttonFX;

    private void Awake() 
    {
        instance = this;
        hostButton.onClick.AddListener(HandleHostButtonClicked);
        clientButton.onClick.AddListener(HandleClientButtonClicked);
        quitButton.onClick.AddListener(HandleQuitButtonClicked);
    }
    
    private void HandleHostButtonClicked()
    {
        SoundManager.instance.PlaySound(buttonFX, transform, 1f);
        NetworkManager.Singleton.StartHost();
        menuCanvas.SetActive(false); // Disable the menu canvas after starting the host
    }

    private void HandleClientButtonClicked()
    {
        SoundManager.instance.PlaySound(buttonFX, transform, 1f);
        NetworkManager.Singleton.StartClient();
        menuCanvas.SetActive(false); // Disable the menu canvas after starting the client
    }

    private void HandleQuitButtonClicked()
    {
        SoundManager.instance.PlaySound(buttonFX, transform, 1f);
        Application.Quit();
    }
}
