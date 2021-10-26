using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    [SerializeField] Image[] joystickImages;
    [SerializeField] Color joystickInActionColor;   
    [SerializeField] Color joystickNotInActionColor;

    //[SerializeField] float rotationSpeed = 1.0f;
    // [SerializeField] float rotationStep = 10f;

    [SerializeField] Joystick joystick;
    //[SerializeField] Joystick fakeJoystick;

    // [SerializeField] Transform fakeJoystickOuterCircle;
    //[SerializeField] Transform fakeJoystickButton;

    public Transform circle;
    public Transform outerCircle;


    // Joystick joystick;
    float x, z;
    float singleStep;
    bool isPlayerMoving = false;
    float timer;

    bool isGamePlaying = true;
    private Vector2 pointA;
    private Vector2 pointB;

    private bool touchStart = false;

    //Vector3 fakeJoystickOuterCircleInitialPos, fakeJoystickButtonInitialPos;
    // Start is called before the first frame update
    void Start()
    {
       // joystick = FindObjectOfType<Joystick>();
        timer = waitTimeBeforeAttack;
        //fakeJoystickOuterCircleInitialPos = fakeJoystickOuterCircle.position;
        //fakeJoystickButtonInitialPos = fakeJoystickButton.position;
       // joystickImages = joystick.gameObject.GetComponentsInChildren<Image>();

        //Debug.Log(joystickImages.Length);

    }

    // Update is called once per frame
    void Update()
    {
      
        if(!isPlayerMoving)
        {
            timer -= Time.deltaTime;
        }


        if (isGamePlaying)
        {
            float moveHorizontal = joystick.Horizontal; //Input.GetAxis("Horizontal");
            float moveVertical = joystick.Vertical; // Input.GetAxis("Vertical");

            // float mH = fakeJoystick.Horizontal;
            // float mV = fakeJoystick.Vertical;


            /* if (Input.touchCount > 0)
             {
                 moveHorizontal = Input.touches[0].deltaPosition.x;
                 moveVertical = Input.touches[0].deltaPosition.y;

                 moveHorizontal *= touchSensitivity;
                 moveVertical *= touchSensitivity;
             }


             */
           

            if (moveHorizontal == 0 && moveVertical == 0)
            {

                isPlayerMoving = false;
                playerRadius.enabled = true;
                animator.SetBool("Run", false);
                ActivateJoystickUi(false);

                //fakeJoystickOuterCircle.position = fakeJoystickOuterCircleInitialPos;
                //fakeJoystickButton.position = fakeJoystickButtonInitialPos;


            }

            else
            {
                isPlayerMoving = true;
                playerRadius.enabled = false;
                timer = waitTimeBeforeAttack;
                animator.SetBool("Run", true);
                ActivateJoystickUi(true);

                //Vector2 
                /*
                Vector3 offset = fakeJoystickButtonInitialPos - new Vector3(moveHorizontal, moveVertical, 0f);
                Vector3 dir = Vector2.ClampMagnitude(offset, 1.0f);

                fakeJoystickButton.position = new Vector3(fakeJoystickButtonInitialPos.x + dir.x,
                    fakeJoystickButtonInitialPos.y + dir.y,
                    fakeJoystickButtonInitialPos.z) * -1;*/
            }


            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            if (isPlayerMoving)
                transform.rotation = Quaternion.LookRotation(movement);


            transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);

            /*
            if (Input.GetMouseButtonDown(0))
            {
                pointA = new Vector2(fakeJoystickButtonInitialPos.x, fakeJoystickButtonInitialPos.y);//Camera.main.ScreenToWorldPoint(new Vector3(moveHorizontal, moveVertical, Camera.main.transform.position.z));

                //fakeJoystickButton.position = pointA * -1;
               // fakeJoystickOuterCircle.transform.position = pointA * -1;
                //circle.GetComponent<SpriteRenderer>().enabled = true;
               // outerCircle.GetComponent<SpriteRenderer>().enabled = true;
            }
            if (Input.GetMouseButton(0))
            {
                touchStart = true;
                pointB = new Vector2(moveHorizontal, moveVertical);
                    //Camera.main.ScreenToWorldPoint(new Vector3(moveHorizontal, moveVertical, Camera.main.transform.position.z));
            }
            else
            {
                touchStart = false;
            }*/

            if (Input.GetMouseButtonDown(0))
            {
                pointA = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));

               circle.transform.position = pointA * -1;
                outerCircle.transform.position = pointA * -1;
                circle.GetComponent<SpriteRenderer>().enabled = true;
                outerCircle.GetComponent<SpriteRenderer>().enabled = true;
            }
            if (Input.GetMouseButton(0))
            {
                touchStart = true;
                pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            }
            else
            {
                touchStart = false;
            }

        }
    }

    private void FixedUpdate()
    {
        if (touchStart)
        {
            Vector2 offset = pointB - pointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
           // moveCharacter(direction * -1);

            circle.transform.position = new Vector2(pointA.x + direction.x, pointA.y + direction.y) * -1;
        }
        else
        {
           // circle.GetComponent<SpriteRenderer>().enabled = false;
            //outerCircle.GetComponent<SpriteRenderer>().enabled = false;
        }

    }
    /*
    private void FixedUpdate()
    {
        if (touchStart)
        {
            Vector2 offset = pointB - pointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
            //moveCharacter(direction * -1);

            fakeJoystickButton.transform.position = new Vector2(fakeJoystickButtonInitialPos.x + direction.x,
                fakeJoystickButtonInitialPos.x + direction.y) * -1;
        }
        else
        {
           // circle.GetComponent<SpriteRenderer>().enabled = false;
            //outerCircle.GetComponent<SpriteRenderer>().enabled = false;
        }

    }
    void moveCharacter(Vector2 direction)
    {
       // player.Translate(direction * speed * Time.deltaTime);
    }*/

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

    private void ActivateJoystickUi(bool val)
    {
        foreach(Image image in joystickImages)
        {
            if(val)
            {
                image.color = joystickInActionColor;
            }

            else
            {
                image.color = joystickNotInActionColor;
            }
        }


    }

    public void PausePlayer(bool val)
    {
        if(val)
        {
            isGamePlaying = false;
            
           // playerRadius.enabled = true;
            animator.SetBool("Run", false);
            //ActivateJoystickUi(false);
        }

        else
        {
            isGamePlaying = true;
            //isPlayerMoving = true;
            //playerRadius.enabled = false;
            //timer = waitTimeBeforeAttack;
            animator.SetBool("Run", true);
            //ActivateJoystickUi(true);
        }
    }

    

}


