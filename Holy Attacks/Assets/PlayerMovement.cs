using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  //  [SerializeField] CharacterController controller;
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] SpriteRenderer playerRadius;

    [SerializeField] GameObject majicShoo;
    [SerializeField] float projectileForce = 10f;
    [SerializeField] Transform projectilePos;

    [SerializeField] float touchSensitivity = 0.3f;
    //[SerializeField] float rotationSpeed = 1.0f;
   // [SerializeField] float rotationStep = 10f;

    Joystick joystick;
    float x, z;
    float singleStep;
    bool isPlayerMoving = false;
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

        

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (Input.touchCount > 0)
        {
            moveHorizontal = Input.touches[0].deltaPosition.x;
            moveVertical = Input.touches[0].deltaPosition.y;

            moveHorizontal *= touchSensitivity;
            moveVertical *= touchSensitivity;
        }

        if (moveHorizontal == 0 && moveVertical == 0)
        {
            isPlayerMoving = false;
            playerRadius.enabled = true;
        }

        else
        {
            isPlayerMoving = true;
            playerRadius.enabled = false;
        }

        

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.rotation = Quaternion.LookRotation(movement);


        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
    }

    public bool GetIsPlayerMoving()
    {
        return isPlayerMoving;
    }

    public void AttackEnemy(Transform enemy)
    {
        GameObject magicAttack = Instantiate(majicShoo, projectilePos.position, Quaternion.identity);
        Vector3 dir = enemy.position - magicAttack.transform.position;
        magicAttack.GetComponent<Rigidbody>().AddForce(dir.normalized * projectileForce, ForceMode.Impulse);
    }

    
}
