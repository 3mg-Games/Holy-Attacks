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
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<PlayerController>();
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

        else if(isCivilianAttacking)
        {
            if(target != null)
                agent.SetDestination(target.position);

            else
            {
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
        if(other.tag == "Enemy" && !player.GetIsPlayerMoving() && isCivilianAttacking)
        {

            StartCoroutine(StartPunching(other));
            //Debug.Log("tregiefisjdlfjsdlf");
            //gameSession.RemoveEnemyFromList(other.gameObject);
           

            
        }
    }

    private IEnumerator StartPunching(Collider other)
    {
        animator.SetBool("Punch", true);
        GameObject target = other.gameObject;
        //other.enabled = false;


        yield return new WaitForSeconds(2f);    //serialize time after which enemy die

        if(target != null)
        {
            other.enabled = false;
            gameSession.RemoveFollower(gameObject);
            gameSession.KillEnemy(target);
            Destroy(gameObject);
        }

        else
        {
            SetTargetASPlayer();
        }

        //Destroy(gameObject);

    }

    public void SetTargetASPlayer()
    {
        animator.SetBool("Punch", false);
        isCivilianAttacking = false;
        followPlayer = true;
        target = playerTarget;
        agent.stoppingDistance = civilianStoppingDistanceFromPlayer;
    }

}
