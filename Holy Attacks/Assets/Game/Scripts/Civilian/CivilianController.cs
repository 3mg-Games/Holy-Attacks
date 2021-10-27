using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivilianController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;

    
   

    Transform target, playerTarget;
    bool followPlayer = false;
    GameSession gameSession;
    PlayerController player;

    float civilianStoppingDistanceFromPlayer = 0f;
    bool isCivilianAttacking = false;

    float agentInitialStoppingDistance = 0f;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<PlayerController>();
        agentInitialStoppingDistance = agent.stoppingDistance;
       // material = this.transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(followPlayer)
        {
            agent.SetDestination(target.position);
            //agent.Resume

        }

        else if(isCivilianAttacking && !followPlayer)
        {
            if(target != null)
                agent.SetDestination(target.position);

            else
            {
                animator.SetBool("Punch", false);
                GameObject newTarget = gameSession.GetNewEnemy();  //error
                if (newTarget != null)
                {
                    SetTargetAsEnemy(newTarget.transform);
                }
                else
                    SetTargetASPlayer();
            }
        }

        //animation

        if(agent.velocity.magnitude == 0)
        {
            animator.SetBool("Run", false);
        }

        else
        {
            animator.SetBool("Run", true);
        }
    }

   

    public void SetTarget(Transform transform, Material material)
    {
        
        this.transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().material = material;

        playerTarget= target = transform;
        followPlayer = true;
        gameSession.AddFollower(gameObject);

        int currFollowerCount = gameSession.GetNumFollowers();
        float followerSpacingInc = gameSession.GetFollowerSpacingIncrement();
        agent.stoppingDistance += followerSpacingInc * currFollowerCount;

        civilianStoppingDistanceFromPlayer = agent.stoppingDistance;

        //this.material = material;
        /*SkinnedMeshRenderer rend = transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>();
        Material mat = rend.material;
           mat = material;*/
    }

    public void SetTargetAsEnemy(Transform transform)
    {
        target = transform;
        isCivilianAttacking = true;
        followPlayer = false;
        agent.stoppingDistance = 0;
        //follow
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Civilian Trigger" && !player.GetIsPlayerMoving() && isCivilianAttacking)
        {
            isCivilianAttacking = false;

            if (other != null && other.transform.parent.gameObject != null
                && other.transform.parent.gameObject == target.gameObject)
            {
                // Debug.Log("Punch");
               // isCivilianAttacking = false;
                StartCoroutine(StartPunching(other.transform.parent.gameObject, other));
                //Debug.Log("tregiefisjdlfjsdlf");
                //gameSession.RemoveEnemyFromList(other.gameObject);
            }

            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Civilian Trigger" && !player.GetIsPlayerMoving() && isCivilianAttacking)
        {
            isCivilianAttacking = false;
            if (other != null && other.transform.parent.gameObject != null
                && other.transform.parent.gameObject == target.gameObject)
            {
                animator.SetBool("Punch", true);
                //isCivilianAttacking = false;
                // Debug.Log("Punch");
                // StartCoroutine(StartPunching(other.transform.parent.gameObject, other));
                //Debug.Log("tregiefisjdlfjsdlf");
                //gameSession.RemoveEnemyFromList(other.gameObject);
                // StartCoroutine(OnStayPunch(other.transform.parent.gameObject, other));

            }

           


        }
    }

    private IEnumerator OnStayPunch(GameObject other, Collider otherCollider2)
    {
        animator.SetBool("Punch", true);
        GameObject target = other;

        float t = Random.Range(1.90f, 2f);
        yield return new WaitForSeconds(t);    //serialize time after which enemy die

        if (target != null)
        {
            otherCollider2.enabled = false;
            gameSession.KillEnemy(target);
            target = null;
            isCivilianAttacking = true;

        }

        else
        {
            animator.SetBool("Punch", false);
            GameObject newTarget = gameSession.GetNewEnemy();
            if (newTarget != null)
            {
                SetTargetAsEnemy(newTarget.transform);
            }
            else
                SetTargetASPlayer();

            isCivilianAttacking = true;
        }

    }

    private IEnumerator StartPunching(GameObject other, Collider otherCollider2)
    {
        animator.SetBool("Punch", true);
        GameObject target = other;
        //other.enabled = false;


        yield return new WaitForSeconds(2f);    //serialize time after which enemy die

        if(target != null)
        {
            otherCollider2.enabled = false;
            gameSession.KillEnemy(target);
            target = null;
            isCivilianAttacking = true;
           // gameSession.RemoveFollower(gameObject);
           
           // Destroy(gameObject);
        }

        else
        {
            animator.SetBool("Punch", false);
            GameObject newTarget = gameSession.GetNewEnemy();
            if(newTarget != null)
            {
                SetTargetAsEnemy(newTarget.transform);
            }
            else
                SetTargetASPlayer();

            isCivilianAttacking = true;
        }

        //Destroy(gameObject);

    }

    public void SetTargetASPlayer()
    {
        animator.SetBool("Punch", false);
        animator.SetBool("Run", true);
        isCivilianAttacking = false;
        followPlayer = true;
        target = playerTarget;

        int currFollowerCount = gameSession.GetFollowerNumber(gameObject);
        float followerSpacingInc = gameSession.GetFollowerSpacingIncrement();
        agent.stoppingDistance = agentInitialStoppingDistance + followerSpacingInc * currFollowerCount;

        civilianStoppingDistanceFromPlayer = agent.stoppingDistance;

        //agent.stoppingDistance = civilianStoppingDistanceFromPlayer;
    }

}
