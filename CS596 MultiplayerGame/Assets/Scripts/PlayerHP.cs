using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerHP : NetworkBehaviour
{
    public NetworkVariable<float> maxHealth = new NetworkVariable<float>(100f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    
    public NetworkVariable<float> health = new NetworkVariable<float>(100f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    
    [SerializeField] private Image healthBar;
    [SerializeField] public AudioClip deathFX;
    [SerializeField] private AudioClip hurtFX;
    
    void Start()
    {
        if (IsServer)
        {
            health.Value = maxHealth.Value;
        }
        
        health.OnValueChanged += OnHealthChanged;
    }
    
    void OnDestroy()
    {
        health.OnValueChanged -= OnHealthChanged;
    }
    
    private void OnHealthChanged(float previousValue, float newValue)
    {
        UpdateHealthBar();
    }
    
    // Update the healthbar 
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = Mathf.Clamp(health.Value / maxHealth.Value, 0, 1);
        }
    }
    
    // Call TakeDamage() on collision
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Let only the server process damage
        if (!IsServer) return;
        
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage();
        }
    }
    
    // Take damager method process only on the server
    public void TakeDamage()
    {
        // Server-only method
        if (!IsServer) return;
        
        health.Value -= 1;
        SoundManager.instance.PlaySound(hurtFX, transform, 1f);
        Debug.Log("Player hit! Lives remaining: " + health.Value);
        if (health.Value <= 0)
        {
            Die();
        }
    }
    
    // Die despawns the network object
    public void Die()
    {
        Debug.Log("Player has died!");
        
        if (IsServer)
        {
            // Consider respawning players instead of despawning
            NetworkObject.Despawn();
            SoundManager.instance.PlaySound(deathFX, transform, 1f);
            GameManager.Instance.OnPlayerDied(OwnerClientId);
        }
    }
}