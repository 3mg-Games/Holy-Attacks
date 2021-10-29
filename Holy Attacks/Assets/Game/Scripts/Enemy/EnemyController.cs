using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject confused;

    float durationOfConfusion = 3f;

    bool isReadyToBeAttacked = false;
   //bool hasEnemyBeenAttacked =  false; //false
    bool hasEnemyStopped = false;
    bool isEnemyAttacking = false;


    bool isConfused = false;
    GameSession gameSession;

    PlayerController player;

    

    GameObject target;

    

    //Coroutine enemyAttack;
    float timer;
    float timerIniitalVal = 2f;
    float timerIniitalInitialVal = 2f;
    float timerDecrementVal;

    //HealthBar healthBar;

    float health = 2f;

    bool isHealthTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        timer = timerIniitalVal;
        timerDecrementVal = 0.4f;
       agent.enabled = false;
       player = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
       gameSession = FindObjectOfType<GameSession>();
        durationOfConfusion = gameSession.GetDurationOfEnemyConfusion();
       // healthBar = transform.GetComponentInChildren<HealthBar>();
        //healthBar.SetMaxValue(health);

    }

    // Update is called once per frame
    void Update()
    {
       /* if(isReadyToBeAttacked && !player.GetIsPlayerMoving() && !hadEnemyBeenAttacked)
        {
            isReadyToBeAttacked = false;
            hadEnemyBeenAttacked = true;
            player.AttackEnemy(transform);
        }*/
        /*
        if(isReadyToBeAttacked && !player.GetIsPlayerMoving() && !hasEnemyBeenAttacked)
        {
            isReadyToBeAttacked = false;
            hasEnemyBeenAttacked = true;
            gameSession.AddEnemiesToBeAttacked(gameObject);
        }*/
        /*
        if(isHealthTriggered)
        {
            health -= Time.deltaTime;
            healthBar.SetValue(health);

            if(health <= 0f)
            {
                isHealthTriggered = false;
            }
        }*/

        if (agent.enabled && !isConfused)
        {

            if (target != null)
            {
                agent.SetDestination(target.transform.position);

                
                if (agent.remainingDistance < 1.5f)
                {
                    timer -= Time.deltaTime;

                    if(!(tag == "Enemy Boss"))
                    agent.isStopped = true;
                    //agent.enabled = false;
                    animator.SetBool("Punch", true);
                    isHealthTriggered = true;
                    hasEnemyStopped = true;
                    
                    if(timer <= 0f)
                    {
                        if (tag == "Enemy Boss")
                        {
                            timerIniitalVal = timerIniitalInitialVal;
                            timer = timerIniitalVal;

                            Debug.Log("I am booss");
                            GameObject newTar = gameSession.GetNewFollower(gameObject);
                            animator.SetBool("Punch", false);
                            agent.isStopped = false;
                            SetTarget(newTar);

                            KillFollower();
                        }


                        else
                            KillFollower();
                    }
                    //enemyAttack = StartCoroutine(KillFollower());
                }

                else if (hasEnemyStopped && agent.remainingDistance >= 1.5f)
                {
                    //StopCoroutine(enemyAttack);
                    hasEnemyStopped = false;
                    timer = timerIniitalVal =  timerIniitalVal - timerDecrementVal;
                    //Debug.Log("resume");
                    agent.isStopped = false;
                    animator.SetBool("Punch", false);
                    isHealthTriggered = false;
                    //  agent.SetDestination(target.transform.position);
                }
            }

            else
            {
                GameObject newTar = gameSession.GetNewFollower(gameObject);
                animator.SetBool("Punch", false);
                agent.isStopped = false;
                SetTarget(newTar);

            }
        }
        

    }

    private void KillFollower()
    {
      //  yield return new WaitForSeconds(2f);

        gameSession.KillEnemy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
       
        
            if (other.tag == "Player" && !isReadyToBeAttacked)
            {
                isReadyToBeAttacked = true;
                gameSession.AddEnemiesToBeAttacked(gameObject);
            }

            if (!isEnemyAttacking && other.tag == "Civilian" && !isConfused)
            {
                isEnemyAttacking = true;
                //agent.enabled = false;
                animator.SetBool("Punch", true);
                isHealthTriggered = true;
        }

            if (other.tag == "Player Projectile" && !isConfused)
            {
            //Debug.Log("projectile hit");
            isConfused = true;
            confused.SetActive(true);
            StartCoroutine(EnemyConfused());
                
                
            }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player" && isReadyToBeAttacked)
        {
            isReadyToBeAttacked = false;
            //hasEnemyBeenAttacked = false;
            gameSession.RemoveEnemyFromList(gameObject, true);
        }
    }

    private IEnumerator EnemyConfused()
    {
      // isReadyToBeAttacked = false;            //uncomment
      
       //isEnemyAttacking = false;                //uncomment

        animator.SetBool("Confused", true);

        //bool isEn = false;
        /*if (agent.enabled)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }*/

        if(agent.enabled)
        {
            agent.isStopped = true;
        }

        yield return new WaitForSeconds(durationOfConfusion);

        animator.SetBool("Confused", false);
        //agent.enabled = true;
        if (agent.enabled)
       {
            //agent.enabled = true;
           agent.isStopped = false;
            //;
            //;
        }
        timer = timerIniitalVal;

        //isReadyToBe

        isConfused = false;
        confused.SetActive(false);
    }

    public void SetTarget(GameObject target)
    {
        if (!isConfused)
        {
            agent.enabled = true;
            this.target = target;
            if (this.target != null)
            {
                agent.SetDestination(this.target.transform.position);
                animator.SetBool("Run", true);
                
            }
        }
    }

    public void EnableHealthBar()
    {
        isHealthTriggered = true;
    }






}


