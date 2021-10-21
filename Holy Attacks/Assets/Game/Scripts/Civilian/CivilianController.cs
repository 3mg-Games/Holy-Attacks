using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivilianController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Material material;

    Transform target, playerTarget;
    bool followPlayer = false;
    GameSession gameSession;
    PlayerController player;

    bool isCivilianAttacking = false;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<PlayerController>();
       // material = transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material;
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
                isCivilianAttacking = false;
                followPlayer = true;
                target = playerTarget;
            }
        }

        if(agent.velocity.magnitude == 0)
        {
            animator.SetBool("Run", false);
        }

        else
        {
            animator.SetBool("Run", true);
        }
    }

   

    public void SetTarget(Transform transform, Material ma)
    {
        //transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material = material;

        playerTarget= target = transform;
        followPlayer = true;
        gameSession.AddFollower(gameObject);
        int currFollowerCount = gameSession.GetNumFollowers();
        float followerSpacingInc = gameSession.GetFollowerSpacingIncrement();
        agent.stoppingDistance += followerSpacingInc * currFollowerCount;


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
        //follow
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && !player.GetIsPlayerMoving())
        {
            //Debug.Log("tregiefisjdlfjsdlf");
            gameSession.RemoveEnemyFromList(other.gameObject);
            gameSession.RemoveFollower(gameObject);
            Destroy(gameObject);
        }
    }

}
