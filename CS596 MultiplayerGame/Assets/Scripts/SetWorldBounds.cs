using UnityEngine;
using UnityEngine.Tilemaps;

public class SetWorldBounds : MonoBehaviour
{
    private void Awake()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        if (tilemap != null)
        {
            Globals.WorldBounds = tilemap.localBounds; // Get tilemap bounds
        }
        else
        {
            Debug.LogError("SetWorldBounds: No Tilemap component found on this GameObject!");
        }
    }
}