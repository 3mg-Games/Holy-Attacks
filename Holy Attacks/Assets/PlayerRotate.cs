using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 10f;
   // [SerializeField] Vector3 force;
    [SerializeField] Transform playerBody;
  //  [SerializeField]

    Joystick joystick;
    //Rigidbody rb;
    //float yRotation = 0f;
   // Quaternion rotation;
    float mouseX, mouseY;
    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        /*rb.velocity = new Vector3(joystick.Horizontal * force.x,
            rb.velocity.y,
            joystick.Vertical * force.z);

        yRotation -= joystick.Horizontal;
        rotation = Quaternion.Euler(yRotation, 0f, 0f);

        rb.MoveRotation(rotation);
        */
        mouseX = joystick.Horizontal * mouseSensitivity;
        mouseY = joystick.Vertical * mouseSensitivity;
       
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
