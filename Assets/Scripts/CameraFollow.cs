using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CamaraRotation : MonoBehaviour
{
    public Transform player;
    private float xMouse;
    private float yMouse;
    private float xRotation;
    public float speed = 70f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        xMouse = Input.GetAxis("Mouse X") * speed * Time.deltaTime;
        yMouse = Input.GetAxis("Mouse Y") * speed * Time.deltaTime;
        xRotation -= yMouse;
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        player.Rotate(Vector3.up * xMouse);

        // Update the camera position to follow the player
        transform.position = player.position;
    }
}
