using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform player; // Player reference
    private Camera _mainCamera;
    private Bounds _cameraBounds;
    
    public float smoothSpeed = 0.1f; // Smoothness of following
    public Vector3 offset; // Camera offset from the player

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        CalculateCameraBounds();
        StartCoroutine(FindPlayerRoutine());
    }

    private void LateUpdate()
    {
        if (player == null) return;

        // Desired position with smooth following
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Clamp position within world bounds
        transform.position = GetBoundedPosition(smoothedPosition);
    }

    private void CalculateCameraBounds()
    {
        float height = _mainCamera.orthographicSize;
        float width = height * _mainCamera.aspect;

        float minX = Globals.WorldBounds.min.x + width;
        float maxX = Globals.WorldBounds.max.x - width;

        float minY = Globals.WorldBounds.min.y + height;
        float maxY = Globals.WorldBounds.max.y - height;

        _cameraBounds = new Bounds();
        _cameraBounds.SetMinMax(
            new Vector3(minX, minY, 0.0f),
            new Vector3(maxX, maxY, 0.0f)
        );
    }

    private Vector3 GetBoundedPosition(Vector3 targetPosition)
    {
        return new Vector3(
            Mathf.Clamp(targetPosition.x, _cameraBounds.min.x, _cameraBounds.max.x),
            Mathf.Clamp(targetPosition.y, _cameraBounds.min.y, _cameraBounds.max.y),
            transform.position.z
        );
    }

    private System.Collections.IEnumerator FindPlayerRoutine()
    {
        while (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            yield return new WaitForSeconds(0.5f); // Check every 0.5 seconds
        }
    }
}
