using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.UI;

public class CivilianController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] GameObject poofVfx;
    [SerializeField] GameObject plus1Vfx;
    [SerializeField] SpriteRenderer radius;
    float waitTimeBeforeConversion;

    IEnumerator co = null;
    IEnumerator corutine = null;

    Transform target, playerTarget;
    bool followPlayer = false;
    GameSession gameSession;
    PlayerController player;

    float civilianStoppingDistanceFromPlayer = 0f;
    bool isCivilianAttacking = false;

    float agentInitialStoppingDistance = 0f;

    bool isCivilianHasting = false;
    float hasteTimer = 0f;
    float civilianInitialSpeed;

    CivilianWait civilianWait;

    float waitTimer;
    bool isCivilianWaiting = false;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<PlayerController>();
        agentInitialStoppingDistance = agent.stoppingDistance;
        civilianInitialSpeed = agent.speed;

        waitTimer = waitTimeBeforeConversion = gameSession.civilianWaitTimeBeforeConversion;
        
        civilianWait =  GetComponentInChildren<CivilianWait>();
        civilianWait.SetMaxValue(waitTimer);
        // material = this.transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

        if(isCivilianWaiting)
        {
            waitTimer -= Time.deltaTime;
            civilianWait.SetValue(waitTimer);

            if(waitTimer <= 0f)
            {
                isCivilianWaiting = false;
            }
        }

        if(isCivilianHasting)
        {
            hasteTimer -= Time.deltaTime;

            if(hasteTimer <= 0)
            {
                agent.speed = civilianInitialSpeed;
                isCivilianHasting = false;
            }
        }
        
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

    public void StartConversion(Transform transform, Material material)
    {
        corutine = SetTarget(transform, material);

        civilianWait.gameObject.transform.GetChild(0).gameObject.SetActive(true);

        isCivilianWaiting = true;
        StartCoroutine(corutine);
    }

    public void StopConversion()
    {
        StopCoroutine(corutine);
        

        isCivilianWaiting = false;

        waitTimer = waitTimeBeforeConversion;
        civilianWait.SetMaxValue(waitTimer);
        civilianWait.gameObject.transform.GetChild(0).gameObject.SetActive(false);


    }
   

    private IEnumerator SetTarget(Transform transform, Material material)
    {

        yield return new WaitForSeconds(waitTimeBeforeConversion);

        radius.enabled = false;

        this.transform.GetChild(4).gameObject.SetActive(false);
        this.transform.GetChild(0).gameObject.SetActive(true);


        var p = gameObject.transform.position;
        Vector3 pos = new Vector3(p.x, p.y + 1f, p.z);

        GameObject poof = Instantiate(poofVfx, pos, Quaternion.identity);
        Destroy(poof, 1.5f);
        this.transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().material = material;

        playerTarget= target = transform;
        followPlayer = true;
        gameSession.AddFollower(gameObject);

        int currFollowerCount = gameSession.GetNumFollowers();
        float followerSpacingInc = gameSession.GetFollowerSpacingIncrement();
        agent.stoppingDistance += followerSpacingInc * currFollowerCount;

        civilianStoppingDistanceFromPlayer = agent.stoppingDistance;

        // p = gameObject.transform.position;
        // pos = new Vector3(p.x, p.y + 2f, p.z);

        //pos = new Vector3(pos.x, pos.y + 2f, pos.z);
        GameObject plusOne = Instantiate(plus1Vfx, transform.position, Quaternion.identity, this.transform);
        //plusOne.transform.parent = transform;
        plusOne.transform.localPosition = new Vector3(0, 2.5f, 0);
        plusOne.transform.parent = null;
        plusOne.GetComponent<Animator>().enabled = true;
        Destroy(plusOne, 1f);

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
                co = StartPunching(other.transform.parent.gameObject, other);
                StartCoroutine(co);
               // StartCoroutine(StartPunching(other.transform.parent.gameObject, other));
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
        target.GetComponent<EnemyController>().EnableHealthBar();

        yield return new WaitForSeconds(2f);    //serialize time after which enemy die

        if(target != null)
        {
            otherCollider2.enabled = false;
            gameSession.KillEnemy(target);
            //if(!(target.tag == "Enemy Boss"))
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


    public void CivilianHaste(float percentageInc, float hasteTime)
    {
        agent.speed = agent.speed + agent.speed * percentageInc / 100f;
        hasteTimer = hasteTime;
        isCivilianHasting = true;
    }

    public void SetTargetAsNull()
    {
        StopCoroutine(co);
       // target = null;
        //isCivilianAttacking = true;
    }

    public void Win()
    {
       
        followPlayer = false;
        isCivilianAttacking = false;
        agent.isStopped = true;
        agent.enabled = false;

        animator.SetTrigger("Win");
    }
}
