using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerHP : NetworkBehaviour
{
    public NetworkVariable<float> maxHealth = new NetworkVariable<float>(
        100f, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);
    
    public NetworkVariable<float> health = new NetworkVariable<float>(
        100f, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);
    
    [SerializeField] private Image healthBar;
    
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
    
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = Mathf.Clamp(health.Value / maxHealth.Value, 0, 1);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Let only the server process damage
        if (!IsServer) return;
        
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage();
        }
    }
    
    public void TakeDamage()
    {
        // Server-only method
        if (!IsServer) return;
        
        health.Value -= 1;
        Debug.Log("Player hit! Lives remaining: " + health.Value);
        if (health.Value <= 0)
        {
            Die();
        }
    }
    
    public void Die()
    {
        Debug.Log("Player has died!");
        
        if (IsServer)
        {
            // Consider respawning players instead of despawning
            // For now, just destroy
            NetworkObject.Despawn();
        }
    }
}