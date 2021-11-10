﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
  //  [SerializeField] CharacterController controller;
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] SpriteRenderer playerRadius;
    [SerializeField] SpriteRenderer attackRadius;
    [SerializeField] Animator animator;
    [SerializeField] LayerMask layer;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileForce = 10f;
    [SerializeField] Transform projectilePos;
    [SerializeField] float maxProjectileDistance;
    [SerializeField] float waitTimeBeforeAttack = 0.4f;

    [SerializeField] float touchSensitivity = 0.3f;

    [SerializeField] Image[] joystickImages;
    [SerializeField] Color joystickInActionColor;   
    [SerializeField] Color joystickNotInActionColor;

    [SerializeField] GameObject shockWave;
    [SerializeField] Color shockWaveCharging;
    [SerializeField] Color shockWaveActive;
    [SerializeField] Image shockWaveImage;
    [SerializeField] ShockWave shockWaveScript;
    [SerializeField] float shockWaveChargeTimer = 5f;

    //[SerializeField] float rotationSpeed = 1.0f;
    // [SerializeField] float rotationStep = 10f;

    [SerializeField] Joystick joystick;

    [SerializeField] Canvas parentCanvas;

    [SerializeField] GameObject staffGlow;
    [SerializeField] Animator staffAnimator;

    [SerializeField] GameObject tutorial;


    bool isTutorail = true;

    
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
    Vector3 outerCircleLocalPos;

    float shockWaveTimer;
    bool isShockWaveActive = false;

    bool isFirstTouch = false;

    float touchHorizontal = 0f, touchVertical = 0f;

    Touch touch;

    bool kiss = false;

    Vector2 mouse = Vector2.zero;

    bool usingMouse = false;

    Vector3 p = Vector3.zero;
    Vector2 p2 = Vector2.zero;

    float hasteTimer = 0f;

    bool playerHaste = false;

    float playerInitialSpeed;

    GameSession gameSession;
    bool areSpellRemaining = true;

    float moveHorizontal, moveVertical;

    float initialMoveSpeed;

    bool isBoundary = false;

    bool isPlayerSummoning = false;
    
    GameObject[] civilians;

    //Vector3 
    //Vector3 fakeJoystickOuterCircleInitialPos, fakeJoystickButtonInitialPos;
    // Start is called before the first frame update
    void Start()
    {
       // joystick = FindObjectOfType<Joystick>();
        timer = waitTimeBeforeAttack;
        circleLocalPos = circle.transform.localPosition;
        outerCircleLocalPos = outerCircle.transform.localPosition;
        shockWaveTimer = shockWaveChargeTimer;
        shockWaveScript.SetMaxValue(shockWaveTimer);

        playerInitialSpeed = movementSpeed;

        gameSession = FindObjectOfType<GameSession>();
        initialMoveSpeed = movementSpeed;

        civilians = GameObject.FindGameObjectsWithTag("Civilian");
        //fakeJoystickOuterCircleInitialPos = fakeJoystickOuterCircle.position;
        //fakeJoystickButtonInitialPos = fakeJoystickButton.position;
       // joystickImages = joystick.gameObject.GetComponentsInChildren<Image>();

        //Debug.Log(joystickImages.Length);

    }

    public bool IsPlayerSummoning
    {
        get
        {
            return isPlayerSummoning;
        }

        set
        {
            isPlayerSummoning = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
      
        if(playerHaste)
        {
            hasteTimer -= Time.deltaTime;

            if(hasteTimer <= 0)
            {
                movementSpeed = playerInitialSpeed;
                playerHaste = false;
            }
        }



        if(isShockWaveActive)
        {
            shockWaveTimer -= Time.deltaTime;
            shockWaveScript.SetValue(shockWaveChargeTimer - shockWaveTimer);

            if(shockWaveTimer <= 0)
            {
                shockWaveImage.color = shockWaveActive;

                shockWaveTimer = shockWaveChargeTimer;
                shockWaveScript.SetMaxValue(shockWaveChargeTimer);

                isShockWaveActive = false;

                    //start fade away
                
            }
        }

        if(!isPlayerMoving)
        {
            timer -= Time.deltaTime;
        }


        if (isGamePlaying)
        {
            moveHorizontal = joystick.Horizontal; //Input.GetAxis("Horizontal");
             moveVertical = joystick.Vertical; // Input.GetAxis("Vertical");
            
            // float mH = fakeJoystick.Horizontal;
            // float mV = fakeJoystick.Vertical;

            /*
             if (Input.touchCount > 0)
             {
                usingMouse = false;
               // Debug.Log("touch");
                touch = Input.GetTouch(0);
                kiss = touch.tapCount > 0 ? true : false;
                // if you touch
                if (kiss)
                {
                    touchHorizontal = touch.position.x;
                    touchVertical = touch.position.y;
                    //touchForward = touch.position.z;
                    if (!isFirstTouch)
                        isFirstTouch = true;

                    touchStart = true;

                }
                // if touch ended
                if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                {
                    touchStart = false;
                    isFirstTouch = false;
                    // do something
                }

             }*/

             if(Input.GetMouseButtonDown(0))
            {
                if(isTutorail)
                {
                    isTutorail = false;
                    Destroy(tutorial);
                }

                usingMouse = true;
                mouse = Input.mousePosition;
                kiss = Input.GetMouseButton(0) ? true : false;
                // if you touch
                if (kiss)
                {
                    touchHorizontal = mouse.x;
                    touchVertical = mouse.y;
                    //touchForward = touch.position.z;
                    if (!isFirstTouch)
                        isFirstTouch = true;

                    touchStart = true;

                }
                // if touch ended
                if (Input.GetMouseButtonUp(0))
                {
                    touchStart = false;
                    isFirstTouch = false;
                    
                    // do something
                }
            }

            


             
           

            if (moveHorizontal == 0 && moveVertical == 0)
            {

                isPlayerMoving = false;
                //playerRadius.enabled = true;
                attackRadius.enabled = false;
                ActivateCivilianRadius(true);
               // playerRadius.gameObject.SetActive(true);
                animator.SetBool("Run", false);
                ActivateJoystickUi(false);

                shockWave.gameObject.SetActive(true);

                if(isPlayerSummoning)
                {
                    animator.SetBool("Summon", true);
                }

               
                //fakeJoystickOuterCircle.position = fakeJoystickOuterCircleInitialPos;
                //fakeJoystickButton.position = fakeJoystickButtonInitialPos;

                //circle.Translate(circleLocalPos, Space.Self);
                //              shockWave.enabled = true;

                //outerCircle.transform.
                
                outerCircle.transform.localPosition = outerCircleLocalPos;
                //circle.transform.localPosition = circleLocalPos;
                circle.transform.localPosition = Vector3.zero;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    AttackEnemy();
                }

            }

            else
            {
                isPlayerMoving = true;
                animator.SetBool("Summon", false);
                //  playerRadius.enabled = false;
                //playerRadius.gameObject.SetActive(false);
                attackRadius.enabled = true;
                ActivateCivilianRadius(false);

                timer = waitTimeBeforeAttack;
                animator.SetBool("Run", true);
                ActivateJoystickUi(true);

               

                shockWave.gameObject.SetActive(false);


                

                if (!usingMouse)
                {/*
                    //p = new Vector3(touchHorizontal, touchVertical, 0f);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        parentCanvas.transform as RectTransform, p,
        parentCanvas.worldCamera,
        out p);

                    outerCircleLocalPos = p;

                    circleLocalPos = p;*/
                }
                else
                {
                    var temp = Input.mousePosition;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        parentCanvas.transform as RectTransform, temp,
        parentCanvas.worldCamera,
        out p2);
                    outerCircleLocalPos = p2;

                    circleLocalPos = p2;
                }
                    

                

                if (isFirstTouch)
                {
                    isFirstTouch = false;

                    if (!usingMouse)
                    {
                        
                        outerCircle.transform.localPosition = outerCircleLocalPos;
                        circle.transform.localPosition = circleLocalPos;
                    }
                    else
                    {

                        outerCircle.transform.localPosition = outerCircleLocalPos;
                       // Debug.Log(outerCircle.transform.localPosition);
                        circle.transform.localPosition = circleLocalPos;
                    }


                }

                if(touchStart)
                {
                    Vector3 dir = new Vector3(moveHorizontal, moveVertical, 0f);

                    Vector3 newPos = dir.normalized * 150;

                    newPos = Vector3.ClampMagnitude(newPos, 120f);
                    circle.localPosition = newPos;
                }

                
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

            if(isBoundary)
            {
                RaycastHit hit;
                

                if (Physics.Raycast(transform.position, transform.forward, out hit, 2f, layer))
                {
                    if (hit.transform.gameObject.tag == "Boundary")
                    {
                        //if (hit.transform.tag == "Boundary")
                        //{
                        movementSpeed = 0f;
                     //   Debug.Log("Boundary sdf sdf sd");
                        //  }

                        //  Debug.Log(hit.collider.isTrigger.Equals("dfsdf"));
                    }

                }
                else
                {
                    movementSpeed = initialMoveSpeed;
                    isBoundary = false;
                }
            }


            transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);

            

            

        }
    }



    
    public void PlayerHaste(float percentageInc, float hasteTime)
    {
        movementSpeed = movementSpeed + movementSpeed * percentageInc / 100f;
        hasteTimer = hasteTime;
        playerHaste = true;
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
        if (!isShockWaveActive && areSpellRemaining)
        {
            //animator.SetTrigger("Magic");
            gameSession.DecrementSpell();
            staffGlow.SetActive(true);
            staffAnimator.SetTrigger("Attack");
           // ShockWave();
        }
    }

    public void ShockWave()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectilePos.position, Quaternion.identity);
        Vector3 dir = transform.forward;
        projectile.GetComponent<Rigidbody>().AddForce(dir.normalized * projectileForce, ForceMode.Impulse);
        projectile.GetComponent<Projectile>().SetMaxDistance(maxProjectileDistance);

        shockWaveImage.color = shockWaveCharging;   //stop fadeaway

        isShockWaveActive = true;
    }

    public void SetAreSpellsRemaining(bool val)
    {
        areSpellRemaining = val;
    }

    private void DeactivateSpell()
    {

        shockWaveImage.gameObject.SetActive(false);
    }

    public void DeactivateGlow(bool val)
    {
        if (val)
            staffGlow.SetActive(false);
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

    public void Win()
    {
        animator.SetTrigger("Win");
        isGamePlaying = false;
        DeactivateSpell();
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Boundary")
        {
            //Debug.Log("dsf");
            //  movementSpeed = 0f;
            isBoundary = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if(other.tag == "")
        if (other.tag == "Boundary")
        {
            isBoundary = false;
        }
    }

    private void ActivateCivilianRadius(bool activate)
    {
        foreach(GameObject civilian in civilians)
        {
            if (civilian != null)
            {
                GameObject radius = civilian.transform.GetChild(3).gameObject;
                radius.SetActive(activate);
            }
        }
    }


}


