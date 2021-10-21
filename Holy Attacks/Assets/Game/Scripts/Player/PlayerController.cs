using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  //  [SerializeField] CharacterController controller;
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] SpriteRenderer playerRadius;
    [SerializeField] Animator animator;

    [SerializeField] GameObject majicShoo;
    [SerializeField] float projectileForce = 10f;
    [SerializeField] Transform projectilePos;
    [SerializeField] float waitTimeBeforeAttack = 0.4f;

    [SerializeField] float touchSensitivity = 0.3f;

    //[SerializeField] float rotationSpeed = 1.0f;
   // [SerializeField] float rotationStep = 10f;

    Joystick joystick;
    float x, z;
    float singleStep;
    bool isPlayerMoving = false;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        timer = waitTimeBeforeAttack;
    }


    // Update is called once per frame
    void Update()
    {
      
        if(!isPlayerMoving)
        {
            timer -= Time.deltaTime;
        }

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
            animator.SetBool("Run", false);
        }

        else
        {
            isPlayerMoving = true;
            playerRadius.enabled = false;
            timer = waitTimeBeforeAttack;
            animator.SetBool("Run", true);
        }

        

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (isPlayerMoving)
          transform.rotation = Quaternion.LookRotation(movement);


        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
    }

    public bool GetIsPlayerMoving()
    {
        if (timer <= 0f && !isPlayerMoving)
            return false;

        else
            return true;
    }

    public void AttackEnemy(Transform enemy)
    {
        GameObject magicAttack = Instantiate(majicShoo, projectilePos.position, Quaternion.identity);
        Vector3 dir = enemy.position - magicAttack.transform.position;
        magicAttack.GetComponent<Rigidbody>().AddForce(dir.normalized * projectileForce, ForceMode.Impulse);
    }

    
}
