using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public float cameraFollowSpeed = 2f; // Speed at which the camera follows the player
    public float distanceFromPlayer = 5f; // Distance between the camera and the player
    public float heightAbovePlayer = 2f; // Height above the player

    private void LateUpdate()
    {
        if (target != null)
        {
            // Calculate desired position of the camera
            Vector3 desiredCameraPosition = target.position - target.forward * distanceFromPlayer + Vector3.up * heightAbovePlayer;

            // Smoothly interpolate between current camera position and desired position
            Vector3 smoothedCameraPosition = Vector3.Lerp(transform.position, desiredCameraPosition, cameraFollowSpeed * Time.deltaTime);

            // Update camera position
            transform.position = smoothedCameraPosition;

            // Make the camera look at the player
            transform.LookAt(target.position);
        }
    }
}
