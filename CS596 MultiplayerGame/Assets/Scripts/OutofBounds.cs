using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutofBounds : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) // Check if player enters the zone
        {
            PlayerHP player = collision.GetComponent<PlayerHP>(); // Call the player's Die() method
            player.Die();
        }
    }
}
