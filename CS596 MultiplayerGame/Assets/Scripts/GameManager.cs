using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    [SerializeField] public GameObject WinScreen; // reference to the win screen canvas
    [SerializeField] public GameObject LoseScreen; // reference to the lose screen canvas
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ulong playerAClientId;  
    public ulong playerBClientId;

    private void Awake()
    {
        // Basic singleton pattern so you can access this from other scripts
        Instance = this;
    }
    private void Start()
    {

        if (IsServer)
        {
            // Subscribe to the callback. This only works if the server/host is the 
            // one that starts the game session, obviously.
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (playerAClientId == 0)
        {
            playerAClientId = clientId;
        }
        else if (playerBClientId == 1)
        {
            playerBClientId = clientId;
        }
    }

    public void OnPlayerDied(ulong loserId)
    {
        // determine using ownerid which is the winner
        ulong winnerId;
        if (playerAClientId == loserId)
        {
            winnerId = playerBClientId;
        }
        else
        {
            winnerId = playerAClientId;
        }

        GameOverClientRpc(winnerId);
    }

    [ClientRpc]
    private void GameOverClientRpc(ulong winnerId)
    {
        if (NetworkManager.Singleton.LocalClientId == winnerId)
        {
            // show winner screen
            WinScreen.SetActive(true);
        }
        else
        {
            // show loser screen
            LoseScreen.SetActive(true);
        }
    }
}
