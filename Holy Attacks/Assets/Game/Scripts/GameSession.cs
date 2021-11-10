using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using System;
using UnityEngine.SceneManagement;
//using Unity.S

public class GameSession : MonoBehaviour
{
    [SerializeField] float followerSpacingIncrement = 0.5f;
    [SerializeField] TextMeshProUGUI numOfFollowersText;
    [SerializeField] GameObject pause;
    [SerializeField] GameObject resume;
    [SerializeField] CinemachineVirtualCamera originalCam;
    [SerializeField] float durationOfEnemyConfusion = 3f;
  //  [SerializeField] int numOfFollowersNeededToEliminateBoss = 3;
    [SerializeField] TextMeshProUGUI spellText;
    [SerializeField] int totalSpell = 3;
    [SerializeField] GameObject minusOneVfx;
    [SerializeField] GameObject poofVfx;
    public float civilianWaitTimeBeforeConversion = 1f;
   
         


    [SerializeField] GameObject continueButton;
    //[SerializeField] float dist

    bool isZoomedOut = false;

     public int numFollowers;
    List<GameObject> followers = new List<GameObject>();

     int numEnemiesToBeAttacked;
    List<GameObject> enemiesToBeAttacked = new List<GameObject>();

    bool isMobAttacking = false;

    bool gotoNextEnemy = false;
    int i;

    PlayerController player;

    List<GameObject> onStayFollowers = new List<GameObject>();

    int numEnemies = 0;
    int numSpellRemaining;

    bool isBug = true;

    SceneLoader sceneLoader;
    // Start is called before the first frame update
    void Awake()
    {
        numFollowers = 0;
        numEnemiesToBeAttacked = 0;
        i = 0;
        numSpellRemaining = totalSpell;
        spellText.text = numSpellRemaining.ToString();
        
        //civilianWaitTimeBeforeConversion = 
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        sceneLoader = FindObjectOfType<SceneLoader>();
    }
    /*
    public void AddOnStayFollower(GameObject follower)
    {
        onStayFollowers.Add(follower);
    }

    public void RemoveOnStayFollower(GameObject follower)
    {
        onStayFollowers.Remove(follower);
    }*/

    // Update is called once per frame
    public void IncNumOfEnemies()
    {
        numEnemies++;
    }

    public void DecNumEnemies()
    {
        numEnemies--;
    }

    private void LateUpdate()
    {

        if (numFollowers > 5 && !isZoomedOut)
        {
            isZoomedOut = true;
           // originalCam.Priority = 1;
        }

    }

    public void DecrementSpell()
    {
        numSpellRemaining--;
        spellText.text = numSpellRemaining.ToString();

        if (numSpellRemaining <= 0)
            player.SetAreSpellsRemaining(false);
    }

    void Update()
    {
        numOfFollowersText.text = numFollowers.ToString();


        //Debug.Log(numEnemiesToBeAttacked);
        if (numEnemiesToBeAttacked > 0 && !isMobAttacking && !player.GetIsPlayerMoving())  //remove numEnemeiestobeattacked
        {
            isMobAttacking = true;
            StartCoroutine(MobAttack());
            Debug.Log("mob attacking");
        }
    }

    public int GetNumFollowers()
    {
        return numFollowers;
    }

    public void AddFollower(GameObject gameObject)
    {
        followers.Add(gameObject);
        IncrementNumFollowers();
    }

    private void RemoveFollower(GameObject follower)
    {
        //if (!isBug)
        //{
            // Debug.Log("bahenchod");
            if (follower != null)
            {

                //var p = follower.transform.position;
                //Vector3 pos = new Vector3(p.x, p.y + 2f, p.z);

                GameObject minusOne = Instantiate(minusOneVfx, follower.transform.position, Quaternion.identity, follower.transform);
                //plusOne.transform.parent = transform;
                minusOne.transform.localPosition = new Vector3(0, 2.5f, 0);
                minusOne.transform.parent = null;
                minusOne.GetComponent<Animator>().enabled = true;
                Destroy(minusOne, 1f);

                //GameObject minusOne = Instantiate(minusOneVfx, pos, Quaternion.identity);
                //Destroy(minusOne, 1f);


                followers.Remove(follower);
                DecrementNumFollowers();



                Destroy(follower);
            }
       // }

        //else
           // isBug = false;
    }

    public float GetFollowerSpacingIncrement()
    {
        return followerSpacingIncrement;
    }

    private void IncrementNumFollowers()
    {
        numFollowers++;
        //numOfFollowersText.text = numFollowers.ToString();
    }

    private void DecrementNumFollowers()
    {
        numFollowers--;
        //numOfFollowersText.text = numFollowers.ToString();
    }

    public List<GameObject> GetFollowers()
    {
        return followers;
    }

    public void AddEnemiesToBeAttacked(GameObject target)
    {
        if (target != null)
        {
            enemiesToBeAttacked.Add(target);
            numEnemiesToBeAttacked++;
        }
    }

    

    public void RemoveEnemyFromList(GameObject target, bool exit)
    {
        Debug.Log(target.name);
        if (target != null)
        {
            enemiesToBeAttacked.Remove(target);
            numEnemiesToBeAttacked--;

            if (exit)
            {
                if (numEnemiesToBeAttacked <= 0)
                {
                    foreach (GameObject follower in followers)
                    {
                        follower.GetComponent<CivilianController>().SetTargetASPlayer();
                    }
                }

                else if (!player.GetIsPlayerMoving())
                {
                    StartCoroutine(MobAttack());
                }
            }

        }
        
    }

    public void SetFollowersToNull()
    {
        foreach(GameObject follower in followers)
        {
            if(follower != null)
            {
                follower.GetComponent<CivilianController>().SetTargetAsNull();
            }
        }
    }

    public void KillEnemy(GameObject target)
    {
        EnemyController enemy = target.GetComponent<EnemyController>();
        // add extra code for boss here
        if (!(target.tag == "Enemy Boss"))
        {
            RemoveEnemyFromList(target, false);

            gotoNextEnemy = true;
        }

        else
        {
            // Debug.Log("Boss");
            //numOfFollowersNeededToEliminateBoss--;   // new code
           
            enemy.DecNumOfFollowersNeededToEliminateEnemy();
            SetFollowersToNull();

            if(enemy.GetNumOfFollowersNeededToEliminateEnemy() <= 0) // new code
            {
                RemoveEnemyFromList(target, false);

                gotoNextEnemy = true;
            }
        }
        
        

        GameObject closestFollower = GetNewTarget(followers, target);
        RemoveFollower(closestFollower);

        if(!(target.tag == "Enemy Boss"))
            Destroy(target);

        else
        {
            if (enemy.GetNumOfFollowersNeededToEliminateEnemy() <= 0)   
            {
                Destroy(target);
                KillAllEnemies();
                Win();
            }
        }
    }

    private void KillAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                var p = enemy.transform.position;
                var pos = new Vector3(p.x, p.y + 1f, p.z);
                GameObject poof = Instantiate(poofVfx, pos, Quaternion.identity);
                Destroy(poof, 1.5f);
                Destroy(enemy);
            }
        }
    }

    private void Win()
    {
        player.Win();

        foreach(GameObject follower in followers)
        {
            if(follower != null)
            {
                follower.GetComponent<CivilianController>().Win();
            }
        }

        continueButton.SetActive(true);
    }

    public void Continue()
    {
        //Scenemange
        sceneLoader.LoadParticularLevel(0);
    }

    private IEnumerator MobAttack()
    {
        
           // while (numEnemiesToBeAttacked > 0)
           // {
             //   gotoNextEnemy = false;
            //GameObject target = enemiesToBeAttacked[0];
                GameObject target = GetNewTarget(enemiesToBeAttacked, player.gameObject);
                if(target == null)
                 {
                     yield return null;
                 }        

        
                 
                for (int j = 0; j < numFollowers; j++)
                {
            GameObject follower = followers[j];
            if(follower != null)
            {
                follower.GetComponent<CivilianController>().SetTargetAsEnemy(target.transform);
            }
                    
                }
        //    i++;
        // yield return new WaitUntil(() => gotoNextEnemy == true);
                isMobAttacking = false;
                yield return null;
          //  }

       // isMobAttacking = false;
    }

    

    public GameObject GetNewEnemy()
    {
        GameObject target = GetNewTarget(enemiesToBeAttacked, player.gameObject);
        return target;
    }

    public GameObject GetNewFollower(GameObject enemy)
    {
        GameObject target = GetNewTarget(followers, enemy);
        return target;
    }
    

    private GameObject GetNewTarget(List<GameObject> targetList, GameObject target2)
    {
        

        if (targetList.Count == 0 || target2 == null)
            return null;

        else
        {

            
            Vector3 target2Pos = target2.transform.position;
            int itr = 0;
            
            while (itr < targetList.Count && targetList[itr] == null)
            {
                itr++;

            }

            if (itr >= targetList.Count)
                return null;


            GameObject target = targetList[itr];
            float shortestDistance = Vector3.Distance(target2Pos, targetList[itr].transform.position);

            itr++;

       

            for (; itr < targetList.Count; itr++)
            {
                float distance = Vector3.Distance(target2Pos, targetList[itr].transform.position);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    target = targetList[itr];
                }
            }

            return target;
        }
    }

    public void Pause()
    {
       player.PausePlayer(true);
        

        //put enemy in idle too
        Time.timeScale = 0;
        pause.SetActive(false);
        resume.SetActive(true);
    }

    public void Resume()
    {
        player.PausePlayer(false);
        Time.timeScale = 1;
        resume.SetActive(false);
        pause.SetActive(true);
    }

    public int GetFollowerNumber(GameObject follower)
    {
        int idx = followers.IndexOf(follower);
        return idx + 1;
    }

    public float GetDurationOfEnemyConfusion()
    {
        return durationOfEnemyConfusion;
    }
  
}
