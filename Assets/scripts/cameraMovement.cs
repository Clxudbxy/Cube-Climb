using UnityEngine;

public class cameraMovement : MonoBehaviour
{

    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float yOffset = 2f;

    private Transform playerTransform;
    private float highestY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
            highestY = transform.position.y;
        }
    
    }

    // Update is called once per frame
void LateUpdate()
    {
        if (playerTransform == null) return;

        float targetY = playerTransform.position.y + yOffset;

        Vector3 desiredPosition = new Vector3(transform.position.x, targetY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
