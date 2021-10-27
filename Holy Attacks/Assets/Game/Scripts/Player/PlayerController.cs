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

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileForce = 10f;
    [SerializeField] Transform projectilePos;
    [SerializeField] float maxProjectileDistance;
    [SerializeField] float waitTimeBeforeAttack = 0.4f;

    [SerializeField] float touchSensitivity = 0.3f;

    [SerializeField] Image[] joystickImages;
    [SerializeField] Color joystickInActionColor;   
    [SerializeField] Color joystickNotInActionColor;

    [SerializeField] Image shockWave;
    [SerializeField] Color shockWaveCharging;
    [SerializeField] Color shockWaveActive;
    [SerializeField] float shockWaveChargeTimer = 5f;

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

    Vector3 circleLocalPos;

    float shockWaveTimer;
    bool isShockWaveActive = false;
    //Vector3 fakeJoystickOuterCircleInitialPos, fakeJoystickButtonInitialPos;
    // Start is called before the first frame update
    void Start()
    {
       // joystick = FindObjectOfType<Joystick>();
        timer = waitTimeBeforeAttack;
        circleLocalPos = circle.transform.localPosition;
        shockWaveTimer = shockWaveChargeTimer;
        //fakeJoystickOuterCircleInitialPos = fakeJoystickOuterCircle.position;
        //fakeJoystickButtonInitialPos = fakeJoystickButton.position;
       // joystickImages = joystick.gameObject.GetComponentsInChildren<Image>();

        //Debug.Log(joystickImages.Length);

    }

    // Update is called once per frame
    void Update()
    {
      
        if(isShockWaveActive)
        {
            shockWaveTimer -= Time.deltaTime;

            if(shockWaveTimer <= 0)
            {
                isShockWaveActive = false;
                shockWave.color = shockWaveActive;
                
            }
        }

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

                shockWave.gameObject.SetActive(true);

                //fakeJoystickOuterCircle.position = fakeJoystickOuterCircleInitialPos;
                //fakeJoystickButton.position = fakeJoystickButtonInitialPos;

                //circle.Translate(circleLocalPos, Space.Self);
//              shockWave.enabled = true;
                circle.transform.localPosition = circleLocalPos;

                if(Input.GetKeyDown(KeyCode.Space))
                {
                    AttackEnemy();
                }

            }

            else
            {
                isPlayerMoving = true;
                playerRadius.enabled = false;
                timer = waitTimeBeforeAttack;
                animator.SetBool("Run", true);
                ActivateJoystickUi(true);

                Vector3 dir = new Vector3(moveHorizontal, moveVertical, 0f);

                Vector3 newPos = dir.normalized * 100;

                newPos = Vector3.ClampMagnitude(newPos, 98f);

                shockWave.gameObject.SetActive(false);
                circle.localPosition = newPos;
                //circle.transform.Translate(dir.normalized * 10, Space.Self);

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

            

            

        }
    }

    
 

    public bool GetIsPlayerMoving()
    {
        if (timer <= 0f && !isPlayerMoving)
            return false;

        else
            return true;
    }

    public void AttackEnemy()
    {
        if (!isShockWaveActive)
        {
            GameObject projectile = Instantiate(projectilePrefab, projectilePos.position, Quaternion.identity);
            Vector3 dir = transform.forward;
            projectile.GetComponent<Rigidbody>().AddForce(dir.normalized * projectileForce, ForceMode.Impulse);
            projectile.GetComponent<Projectile>().SetMaxDistance(maxProjectileDistance);

            shockWave.color = shockWaveCharging;
            shockWaveTimer = shockWaveChargeTimer;
            isShockWaveActive = true;
        }
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


