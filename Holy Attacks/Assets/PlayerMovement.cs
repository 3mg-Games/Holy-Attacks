using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  //  [SerializeField] CharacterController controller;
    [SerializeField] float movementSpeed = 10f;
    //[SerializeField] float rotationSpeed = 1.0f;
   // [SerializeField] float rotationStep = 10f;

    Joystick joystick;
    float x, z;
    float singleStep;
    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //x = joystick.Horizontal;
      //  z = joystick.Vertical;

        //Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * movementSpeed * Time.deltaTime);


        //transform.rot
        //transform.rotation = Quaternion.LookRotation((Vector3.up * z);


        Vector3 targetDir = move - transform.position; //change move
        singleStep = rotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, singleStep, 0f);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);
        */

        float moveHorizontal = joystick.Horizontal;
        float moveVertical = joystick.Vertical;

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.rotation = Quaternion.LookRotation(movement);


        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
    }
}
